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
using App_Dominio.Component;
using DWM.Models.BI;

namespace DWM.Controllers.API
{
    public class ChamadosAPIController : DwmApiController
    {

        [HttpPost]
        [AuthorizeFilter]
        public IEnumerable<ChamadoViewModel> ListChamados(Auth value)
        {
            // Validar Token
            Auth a = ValidarToken(value);
            if (a.Code != 0)
            {
                ChamadoViewModel chamadoViewModel = new ChamadoViewModel()
                {
                    mensagem = new Validate()
                    {
                        Code = 202,
                        Message = "Acesso Negado. Suas credencias não estão autorizadas para executar esta operação."
                    }
                };
                List<ChamadoViewModel> ret = new List<ChamadoViewModel>();
                ret.Add(chamadoViewModel);
                return ret;
            }
            

            // Listar
            PageSize = PageSize == null || PageSize == "" ? "8" : PageSize;
            Facade<ChamadoViewModel, ChamadoModel, ApplicationContext> facade = new Facade<ChamadoViewModel, ChamadoModel, ApplicationContext>();
            IEnumerable<ChamadoViewModel> list = facade.List(new ListViewChamado(), 0, int.Parse(PageSize), value.Token);

            
            return list;
        }

        //[HttpPost]
        //[AuthorizeFilter]
        //public ChamadoViewModel ListChamadoDetalhe(ChamadoViewModel value)
        //{
        //    // Validar Token
        //    ChamadoViewModel chamadoViewModel = (ChamadoViewModel) ValidarToken(value);
        //    if (chamadoViewModel.mensagem.Code != 0)
        //    {
        //        ChamadoViewModel cvm = new ChamadoViewModel()
        //        {
        //            mensagem = new Validate()
        //            {
        //                Code = chamadoViewModel.mensagem.Code,
        //                Message = "Acesso Negado. Suas credencias não estão autorizadas para executar esta operação."
        //            }
        //        };
        //        return cvm;
        //    }

        //    FactoryLocalhost<ChamadoViewModel, ApplicationContext> factory = new FactoryLocalhost<ChamadoViewModel, ApplicationContext>();
        //    ChamadoViewModel obj = factory.Execute(new ChamadoEditBI(), chamadoViewModel);
        //    return obj;
        //}


    }
}
