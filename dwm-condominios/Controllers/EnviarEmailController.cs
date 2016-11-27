using App_Dominio.Controllers;
using App_Dominio.Models;
using App_Dominio.Pattern;
using DWM.Models.BI;
using DWM.Models.Entidades;
using DWM.Models.Enumeracoes;
using DWM.Models.Persistence;
using DWM.Models.Repositories;
using System;
using System.Web.Mvc;


namespace dwm_condominios.Controllers
{
    public class EnviarEmailController : DwmRootController<EmailLogViewModel, EmailLogModel, DWM.Models.Entidades.ApplicationContext>
    {
        #region Constructor
        public override int _sistema_id() { return (int)Sistema.DWMCONDOMINIOS; }
        public override string getListName()
        {
            return "Listar";
        }
        #endregion

        #region List
        public override ActionResult List(int? index, int? PageSize, string descricao = null)
        {
            return ListParam(index, PageSize);
        }

        public ActionResult ListParam(int? index, int? pageSize = 50, string data1 = "", string data2 = "")
        {
            if (ViewBag.ValidateRequest)
            {
                if (data1 == null || data1 == "")
                {
                    data1 = "01" + DateTime.Today.AddMonths(-1).ToString("/MM/yyyy");
                    data2 = Convert.ToDateTime(DateTime.Today.AddMonths(1).ToString("yyyy-MM-") + "01").AddDays(-1).ToString("dd/MM/yyyy");
                }

                ListViewEmailLog list = new ListViewEmailLog();
                return this._List(index, pageSize, "Browse", list, Funcoes.StringToDate(data1).Value, Funcoes.StringToDate(data2).Value);
            }
            else
                return View();
        }
        #endregion

        #region Create
        public override EmailLogViewModel Insert(EmailLogViewModel value)
        {
            value.EmailTipoID = (int)Enumeradores.EmailTipo.OUTROS;
            Factory<EmailLogViewModel, ApplicationContext> facade = new Factory<EmailLogViewModel, ApplicationContext>();
            return facade.Execute(new EmailNotificacaoBI(), value);
        }
        
        #endregion

        #region Retorna o modelo do template
        [AllowAnonymous]
        public JsonResult GetNamesEmailTemplate(int EmailTemplateID)
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

        #region Retorno as Unidades de uma dada Edificação
        [AllowAnonymous]
        public JsonResult GetNames(string term, int tag)
        {
            var results = new BindDropDownList().Unidades("Selecione...", "", term, tag);

            return new JsonResult()
            {
                Data = results,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }
        #endregion
    }
}