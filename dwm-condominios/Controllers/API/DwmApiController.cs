using App_Dominio.Component;
using App_Dominio.Enumeracoes;
using DWM.Models.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace DWM.Controllers.API
{
    public class Credenciais
    {
        public string login { get; set; }
        public string senha { get; set; }
        public string ip { get; set; }
    }

    public class DwmApiController : ApiController
    {
        protected int _sistema_id() { return (int)DWM.Models.Enumeracoes.Sistema.DWMCONDOMINIOS; }

        protected string PageSize = "10";

        protected Repository ValidarToken(Repository value)
        {
            if (value == null)
            {
                value.mensagem = new App_Dominio.Contratos.Validate()
                {
                    Code = 997,
                    Message = "Parãmetro inválido (Null)",
                };
                return value;
            }

            Auth a = new Auth()
            {
                Code = value.mensagem == null || value.mensagem.Code == null ? 0 : value.mensagem.Code.Value,
                Mensagem = value.mensagem == null || value.mensagem.Code == null ? "" : value.mensagem.Message,
                Token = value.sessionId
            };
            // Validar Token
            SegurancaAPIController seg = new SegurancaAPIController();
            a = seg.Validar(a);

            value.mensagem = new App_Dominio.Contratos.Validate()
            {
                Code = a.Code,
                Message = a.Mensagem,
            };

            return value;
        }

        protected Auth ValidarToken(Auth value)
        {
            // Validar Token
            SegurancaAPIController seg = new SegurancaAPIController();
            return seg.Validar(value);
        }

    }
}