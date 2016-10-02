using App_Dominio.Controllers;
using App_Dominio.Security;
using DWM.Models.Enumeracoes;
using DWM.Models.Persistence;
using DWM.Models.Repositories;
using System.Web.Mvc;

namespace DWM.Controllers
{
    public class EmailTemplateController : DwmRootController<EmailTemplateViewModel, EmailTemplateModel, DWM.Models.Entidades.ApplicationContext>
    {
        public override int _sistema_id() { return (int)Sistema.DWMCONDOMINIOS; }
        public override string getListName()
        {
            return "Listar";
        }

        #region List
        [AuthorizeFilter]
        public override ActionResult List(int? index, int? pageSize = 50, string descricao = null)
        {
            if (ViewBag.ValidateRequest)
            {
                ListViewEmailTemplates ema = new ListViewEmailTemplates();
                return this._List(index, pageSize, "Browse", ema, descricao);
            }
            else
                return View();
        }
        #endregion

        #region Edit
        [AuthorizeFilter]
        public ActionResult Edit(int EmailTemplateID)
        {
            return _Edit(new EmailTemplateViewModel() { EmailTemplateID = EmailTemplateID });
        }
        #endregion

        #region Delete
        [AuthorizeFilter]
        public ActionResult Delete(int EmailTemplateID)
        {
            return Edit(EmailTemplateID);
        }
        #endregion

        #region CrudEmailTemplateModal
        public JsonResult CrudEmailTemplateModal(string Descricao)
        {
            return JSonCrud(new EmailTemplateViewModel() { Descricao = Descricao });
        }
        #endregion

        public JsonResult getNames()
        {
            return JSonTypeahead(null, new ListViewEmailTemplates());
        }
    }
}