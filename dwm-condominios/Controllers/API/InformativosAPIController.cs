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
using App_Dominio.Enumeracoes;

namespace DWM.Controllers.API
{
    public class InformativosAPIController : DwmApiController
    {
        [HttpPost]
        [AuthorizeFilter]
        public IEnumerable<InformativoViewModel> ListInformativos(Auth value)
        {
            // Validar Token
            Auth a = ValidarToken(value);
            if (a.Code != 0)
            {
                InformativoViewModel informativoViewModel = new InformativoViewModel()
                {
                    mensagem = new Validate()
                    {
                        Code = 202,
                        Message = "Acesso Negado. Suas credencias não estão autorizadas para executar esta operação."
                    }
                };
                List<InformativoViewModel> ret = new List<InformativoViewModel>
                {
                    informativoViewModel
                };
                return ret;
            }
            //if (a.Code != 0)
            //    //return new List<InformativoViewModel>();
            //    throw new Exception(a.Mensagem);

            // Listar
            Facade<InformativoViewModel, InformativoModel, ApplicationContext> facade = new Facade<InformativoViewModel, InformativoModel, ApplicationContext>();
            IEnumerable<InformativoViewModel> list = facade.List(new ListViewInformativoAPI(), 0, int.Parse(PageSize), value.Token);
            return list;
        }

        [HttpPost]
        [AuthorizeFilter]
        public IEnumerable<InformativoViewModel> ListInformativosByID(InformativoAPIModel value)
        {
            // Validar Token
            Auth a = ValidarToken(value);
            if (a.Code != 0)
            {
                InformativoViewModel informativoViewModel = new InformativoViewModel()
                {
                    mensagem = new Validate()
                    {
                        Code = 202,
                        Message = "Acesso Negado. Suas credencias não estão autorizadas para executar esta operação."
                    }
                };
                List<InformativoViewModel> ret = new List<InformativoViewModel>
                {
                    informativoViewModel
                };
                return ret;
            }
            //if (a.Code != 0)
            //    //return new List<InformativoViewModel>();
            //    throw new Exception(a.Mensagem);

            // Listar
            Facade<InformativoViewModel, InformativoModel, ApplicationContext> facade = new Facade<InformativoViewModel, InformativoModel, ApplicationContext>();
            IEnumerable<InformativoViewModel> list = facade.List(new ListViewInformativoByIDAPI(), 0, int.Parse(PageSize), value.Token, value.InformativoID);
            return list;
        }


        [HttpPost]
        [ResponseType(typeof(InformativoViewModel))]
        public InformativoViewModel Create(InformativoViewModel value)
        {
            value = (InformativoViewModel)ValidarToken(value);
            if (value.mensagem.Code != 0)
                return value;

            value.uri = this.ControllerContext.Controller.GetType().Name.Replace("Controller", "") + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
            FactoryLocalhost<InformativoViewModel, ApplicationContext> facade = new FactoryLocalhost<InformativoViewModel, ApplicationContext>();
            return facade.Execute(new InformativoCadastrarBI(), value, value.sessionId);
        }

        [HttpPost]
        [ResponseType(typeof(InformativoViewModel))]
        public InformativoViewModel Delete(InformativoViewModel value)
        {
            value = (InformativoViewModel)ValidarToken(value);
            if (value.mensagem.Code != 0)
                return value;

            value.uri = this.ControllerContext.Controller.GetType().Name.Replace("Controller", "") + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
            FacadeLocalhost<InformativoViewModel, InformativoModel, ApplicationContext> facade = new FacadeLocalhost<InformativoViewModel, InformativoModel, ApplicationContext>();
            return facade.Save(value, Crud.EXCLUIR);
        }

        [HttpPost]
        [ResponseType(typeof(InformativoComentarioViewModel))]
        public InformativoComentarioViewModel Append(InformativoComentarioViewModel value)
        {
            // Validar Token
            value = (InformativoComentarioViewModel)ValidarToken(value);
            if (value.mensagem.Code != 0)
                return value;

            

            // Insert
            value.uri = this.ControllerContext.Controller.GetType().Name.Replace("Controller", "") + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
            FacadeLocalhost<InformativoComentarioViewModel, InformativoComentarioModel, ApplicationContext> facade = new FacadeLocalhost<InformativoComentarioViewModel, InformativoComentarioModel, ApplicationContext>();
            value = facade.Save(value, App_Dominio.Enumeracoes.Crud.INCLUIR);

            return value;
        }


    }
}