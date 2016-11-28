using System;
using System.ServiceModel.Web;
using WcfCondominiosService.Models;
using App_Dominio.Security;
using App_Dominio.Entidades;
using App_Dominio.Enumeracoes;

namespace WcfCondominiosService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Seguranca" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Seguranca.svc or Seguranca.svc.cs at the Solution Explorer and start debugging.
    public class Seguranca : ISeguranca
    {
        public int _sistema_id() { return (int)DWM.Models.Enumeracoes.Sistema.DWMCONDOMINIOS; }

        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "autenticar/{login}/{senha}/{ip}")]
        public Auth Autenticar(string login, string senha, string ip)
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
                Sessao s = security.Autenticar(login,senha, _sistema_id(), ip, ref Code, ref Mensagem);
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
                int length = ex.Message.IndexOf("\r\n");
                result.Code = int.Parse(ex.ParamName ?? "0");
                if (length >= 0)
                    result.Mensagem = ex.Message.Substring(0, length);
                else
                    result.Mensagem = ex.Message;
                result.Token = "-1";
            }
            catch (Exception ex)
            {
                result.Code = 999;
                result.Mensagem = ex.Message;
                result.Token = "-1";
            }

            return result;
        }

        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "validar/{token}")]
        public Auth Validar(string Token)
        {
            Auth result = new Auth()
            {
                Code = 0,
                Mensagem = "Sucesso!",
                Token = Token
            };

            EmpresaSecurity<App_DominioContext> security = new EmpresaSecurity<App_DominioContext>();
            try
            {
                #region Validar Token (sessão)
                if (!security.ValidarSessao(Token))
                {
                    result.Code = 202;
                    result.Mensagem = MensagemPadrao.Message(202).text;
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


    }
}
