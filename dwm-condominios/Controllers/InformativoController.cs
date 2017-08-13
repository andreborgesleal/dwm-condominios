using App_Dominio.Controllers;
using App_Dominio.Models;
using App_Dominio.Pattern;
using App_Dominio.Security;
using DWM.Models.BI;
using DWM.Models.Entidades;
using DWM.Models.Enumeracoes;
using DWM.Models.Pattern;
using DWM.Models.Persistence;
using DWM.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DWM.Controllers
{
    public class InformativoController : DwmRootController<InformativoViewModel, InformativoModel, ApplicationContext>
    {
        #region Inheritance
        public override int _sistema_id() { return (int)DWM.Models.Enumeracoes.Sistema.DWMCONDOMINIOS; }

        public override string getListName()
        {
            return "Listar";
        }
        #endregion

        #region Create
        public override InformativoViewModel Insert(InformativoViewModel value)
        {
            FactoryLocalhost<InformativoViewModel, ApplicationContext> facade = new FactoryLocalhost<InformativoViewModel, ApplicationContext>();
            return facade.Execute(new InformativoCadastrarBI(), value);
        }

        public override void OnCreateError(ref InformativoViewModel value, FormCollection collection)
        {
            base.OnCreateError(ref value, collection);
            value.Cabecalho = collection["cabecalho"] ?? "";
        }
        #endregion
        
        #region List
        [AuthorizeFilter]
        public override ActionResult List(int? index, int? pageSize = 50, string descricao = null)
        {
            return ListParam(index, PageSize);
        }

        [AuthorizeFilter]
        public ActionResult ListParam(int? index, int? pageSize = 50, string data1 = "", string data2 = "")
        {
            if (ViewBag.ValidateRequest)
            {
                if (data1 == null || data1 == "")
                {
                    data1 = "01" + DateTime.Today.AddMonths(-1).ToString("/MM/yyyy");
                    data2 = Convert.ToDateTime(DateTime.Today.AddMonths(1).ToString("yyyy-MM-") + "01").AddDays(-1).ToString("dd/MM/yyyy");
                }

                //InformativoCadastrarBI list = new InformativoCadastrarBI();
                ListViewInformativo list = new ListViewInformativo();
                return this._List(index, pageSize, "Browse", list, Funcoes.StringToDate(data1).Value, Funcoes.StringToDate(data2).Value);
            }
            else
                return View();
        }
        #endregion

        #region Edit
        [AuthorizeFilter]
        public ActionResult Edit(int informativoID)
        {
            return _Edit(new InformativoViewModel() { InformativoID = informativoID });
        }
        #endregion

        #region Delete
        [AuthorizeFilter]
        public ActionResult Delete(int informativoID)
        {
            return Edit(informativoID);
        }
        #endregion

        #region Retorna o modelo do template
        [AllowAnonymous]
        public JsonResult GetNames(int EmailTemplateID)
        {
            Facade<EmailTemplateViewModel, EmailTemplateModel, ApplicationContext> facade = new Facade<EmailTemplateViewModel, EmailTemplateModel, ApplicationContext>();
            var results = facade.getObject(new EmailTemplateViewModel() { EmailTemplateID = EmailTemplateID });

            return new JsonResult()
            {
                Data = results,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }
        #endregion
    }
}