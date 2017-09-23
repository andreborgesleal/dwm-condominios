using App_Dominio.Entidades;
using App_Dominio.Security;
using DWM.Models.API;
using DWM.Models.Entidades;
using dwm_condominios.Models.API;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;

namespace DWM.Controllers.API
{
    public class SegurancaAPIController : DwmApiController
    {
        [HttpPost]
        // POST: api/InformativosAPI
        public Auth Autenticar(Credenciais Credenciais)
        {
            Auth result = new Auth()
            {
                Code = 0,
                Mensagem = "",
                Token = ""
            };
            int Code = 0;
            string Mensagem = "";

            EmpresaSecurity<App_DominioContext> security = new EmpresaSecurity<App_DominioContext>();
            try
            {
                #region Autenticar
                Sessao s = security.Autenticar(Credenciais.login, Credenciais.senha, _sistema_id(), Credenciais.ip, ref Code, ref Mensagem);
                if (Code > 0)
                {
                    result.Code = Code;
                    result.Mensagem = Mensagem;
                    throw new ArgumentException(Mensagem);
                }
                #endregion

                result.Code = Code;
                result.Mensagem = Mensagem;
                result.Token = s.sessaoId;
            }
            catch (ArgumentException ex)
            {
                //int length = ex.Message.IndexOf("\r\n");
                result.Code = 202;
                result.Mensagem = ex.ParamName;
                //if (length >= 0)
                //    result.Mensagem = ex.Message.Substring(0, length);
                //else
                //    result.Mensagem = ex.Message;
                result.Token = "-1";
            }
            catch (Exception ex)
            {
                result.Code = 999;
                result.Mensagem = ex.Message;
                result.Token = "-1";
            }

            return result;
            //return CreatedAtRoute("DefaultApi", new { id = informativo.InformativoID }, informativo);
        }

        [HttpPost]
        public Auth Validar(Auth result)
        {
            result.Code = 0;
            result.Mensagem = "Sucesso!";

            EmpresaSecurity<App_DominioContext> security = new EmpresaSecurity<App_DominioContext>();
            try
            {
                #region Validar Token (sessão)
                if (!security.ValidarSessao(result.Token))
                {
                    result.Code = 202;
                    result.Mensagem = App_Dominio.Enumeracoes.MensagemPadrao.Message(202).text;
                    result.Token = "-1";
                }
                #endregion
            }
            catch (Exception ex)
            {
                result.Code = 999;
                result.Mensagem = ex.Message;
                result.Token = "-1";
            }

            return result;
        }

        [HttpPost]
        public IEnumerable<UsuarioLogadoToken> GetDadosUsuarioToken(Auth result)
        {
            result = this.Validar(result);
            List<UsuarioLogadoToken> response = new List<UsuarioLogadoToken>();

            if (result.Code == 0)
            {
                var s = DWMSessaoLocal.GetSessaoLocal(result.Token);
                foreach(var unidade in s.Unidades)
                {
                    response.Add(new UsuarioLogadoToken()
                    {
                        Code = 0,
                        EdificacaoID = unidade.EdificacaoID,
                        Mensagem = "Sucesso!",
                        Token = s.sessaoId,
                        Descricao = DWMSessaoLocal.GetDescricaoEdificacao(s.empresaId) + " " + unidade.UnidadeID,
                        UnidadeID = unidade.UnidadeID
                    });
                }
            }
            else
            {
                response.Add(new UsuarioLogadoToken()
                {
                    Code = 0,
                    EdificacaoID = 0,
                    Mensagem = "Não foi possível obter os dados!",
                    Token = result.Token,
                    UnidadeID = 0
                });
            }

            return response;
        }
    }
}
