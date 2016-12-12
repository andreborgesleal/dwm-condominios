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
                List<InformativoViewModel> ret = new List<InformativoViewModel>();
                ret.Add(informativoViewModel);
                return ret;
            }
            //if (a.Code != 0)
            //    //return new List<InformativoViewModel>();
            //    throw new Exception(a.Mensagem);

            // Listar
            PageSize = PageSize == null || PageSize == "" ? "8" : PageSize;
            DateTime Data1 = Funcoes.Brasilia().AddDays(-20);
            DateTime Data2 = Funcoes.Brasilia().Date;
            Facade<InformativoViewModel, InformativoModel, ApplicationContext> facade = new Facade<InformativoViewModel, InformativoModel, ApplicationContext>();
            IEnumerable<InformativoViewModel> list = facade.List(new ListViewInformativo(), 0, int.Parse(PageSize), value.Token, Data1, Data2);

            //int contador = 0;
            //foreach (InformativoViewModel info in list)
            //{
            //    if (info.DataExpiracao.HasValue)
            //        list.ElementAt(contador)._DataExpiracao = info.DataExpiracao.Value.ToString("dd/MM/yyyy");

            //    if (info.DataInformativo != null)
            //        list.ElementAt(contador)._DataInformativo = info.DataInformativo.ToString("dd/MM/yyyy");

            //    if (info.DataPublicacao != null)
            //        list.ElementAt(contador)._DataPublicacao = info.DataPublicacao.ToString("dd/MM/yyyy");

            //    contador++;
            //}
            return list;
        }

        [HttpPost]
        [ResponseType(typeof(InformativoComentarioViewModel))]
        public InformativoComentarioViewModel Create(InformativoComentarioViewModel value)
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