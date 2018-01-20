using App_Dominio.Entidades;
using App_Dominio.Security;
using DWM.Models.API;
using DWM.Models.Entidades;
using dwm_condominios.Models.API;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
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
                UsuarioID = 0,
                Mensagem = "",
                Token = "",
                
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

                string URL = "";
                using (var db = new ApplicationContext())
                {
                    URL = db.Parametros.Find(s.empresaId, (int)Models.Enumeracoes.Enumeradores.Param.URL_CONDOMINIO).Valor;
                }

                result.Code = Code;
                result.Mensagem = Mensagem;
                result.Token = s.sessaoId;
                result.UsuarioID = s.usuarioId;

                FileInfo f = new FileInfo(HttpContext.Current.Server.MapPath("~/Users_Data/Empresas/"+ s.empresaId + "/Avatar/" +  s.usuarioId + ".png"));
                if (f.Exists)
                    result.Avatar = URL + "/Users_Data/Empresas/" + s.empresaId + "/Avatar/" + s.usuarioId + ".png";
                else
                {
                    f = new FileInfo(HttpContext.Current.Server.MapPath("~/Users_Data/Empresas/" + s.empresaId + "/Avatar/" + s.usuarioId + ".jpg"));
                    if (f.Exists)
                        result.Avatar = URL + "/Users_Data/Empresas/" + 3 + "/Avatar/" + s.usuarioId + ".jpg";
                    else
                    {
                        f = new FileInfo(HttpContext.Current.Server.MapPath("~/Users_Data/Empresas/" + s.empresaId + "/Avatar/" + s.usuarioId + ".bmp"));
                        if (f.Exists)
                            result.Avatar = URL + "/Users_Data/Empresas/" + 3 + "/Avatar/" + s.usuarioId + ".bmp";
                        else
                            result.Avatar = "http://api.ning.com/files/XDvieCk-6Hj1PFXyHT13r7Et-ybLOKWFR9fYd15dBrqFQHv6gCVuGdr4GYjaO0u*h2E0p*c5ZVHE-H41wNz4uAGNfcH8LLZS/top_8_silhouette_male_120.jpg?width=30";
                    }
                }
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
                        UnidadeID = unidade.UnidadeID,
                        UsuarioID = s.usuarioId,
                        Avatar = "http://condomino.azurewebsites.net/Users_Data/Empresas/3/Avatar/" + s.usuarioId + ".jpg"
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
