using App_Dominio.Controllers;
using App_Dominio.Security;
using DWM.Models.Enumeracoes;
using DWM.Models.Persistence;
using DWM.Models.Repositories;
using System.Web.Mvc;

namespace DWM.Controllers
{
    public class GrupoFornecedoresController : RootController<GrupoCredorViewModel, GrupoCredorModel>
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
                ListViewGrupoCredor l = new ListViewGrupoCredor();
                return this._List(index, pageSize, "Browse", l, descricao);
            }
            else
                return View();
        }

        [AuthorizeFilter]
        public ActionResult _ListGrupoCredorModal(int? index, int? pageSize = 50, string descricao = null)
        {
            if (ViewBag.ValidateRequest)
            {
                LookupGrupoCredorFiltroModel l = new LookupGrupoCredorFiltroModel();
                return this.ListModal(index, pageSize, l, "Descrição", descricao);
            }
            else
                return View();
        }
        #endregion

        #region edit
        [AuthorizeFilter]
        public ActionResult Edit(int grupoCredorId)
        {
            return _Edit(new GrupoCredorViewModel() { grupoCredorId = grupoCredorId });
        }
        #endregion

        #region Delete
        [AuthorizeFilter]
        public ActionResult Delete(int grupoCredorId)
        {
            return Edit(grupoCredorId);
        }
        #endregion

        #region CrudGrupoCredorModal
        public JsonResult CrudGrupoCredorModal(string descricao)
        {
            return JSonCrud(new GrupoCredorViewModel() { nome = descricao });
        }
        #endregion

        public JsonResult getNames()
        {
            return JSonTypeahead(null, new ListViewGrupoCredor());
        }

    }
}