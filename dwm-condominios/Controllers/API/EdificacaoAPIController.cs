using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using DWM.Models.Entidades;
using DWM.Models.Repositories;
using App_Dominio.Pattern;
using App_Dominio.Models;
using DWM.Models.Persistence;
using DWM.Models.API;
using DWM.Models.Pattern;
using App_Dominio.Security;
using System.Web.Http.Cors;
using App_Dominio.Contratos;
using DWM.Models.BI;

namespace DWM.Controllers.API
{
    public class EdificacaoAPIController : DwmApiController
    {
        public IEnumerable<EdificacaoViewModel> ListEdificacoes(Auth value)
        {
            // Validar Token
            Auth a = ValidarToken(value);
            if (a.Code != 0)
            {
                EdificacaoViewModel EdificacaoViewModel = new EdificacaoViewModel()
                {
                    mensagem = new Validate()
                    {
                        Code = 202,
                        Message = "Acesso Negado. Suas credencias não estão autorizadas para executar esta operação."
                    }
                };
                List<EdificacaoViewModel> ret = new List<EdificacaoViewModel>();
                ret.Add(EdificacaoViewModel);
                return ret;
            }


            // Listar
            PageSize = PageSize == null || PageSize == "" ? "8" : PageSize;
            Facade<EdificacaoViewModel, EdificacaoModel, ApplicationContext> facade = new Facade<EdificacaoViewModel, EdificacaoModel, ApplicationContext>();
            IEnumerable<EdificacaoViewModel> list = facade.List(new ListViewEdificacoes(), 0, int.Parse(PageSize), value.Token);

            return list;
        }
    }
}
