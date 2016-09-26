using App_Dominio.Controllers;
using App_Dominio.Security;
using DWM.Models.Enumeracoes;
using DWM.Models.Persistence;
using DWM.Models.Repositories;
using System.Web.Mvc;

namespace DWM.Controllers
{
    public class GrupoCondominoController : DwmRootController<GrupoCondominoViewModel, GrupoCondominoModel, DWM.Models.Entidades.ApplicationContext> // DwmRootController<GrupoCondominoViewModel, GrupoCondominoModel>
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
                ListViewGrupoCondominos gru = new ListViewGrupoCondominos();
                return this._List(index, pageSize, "Browse", gru, descricao);
            }
            else
                return View();
        }
        #endregion

        #region Create
        public override void BeforeCreate(ref GrupoCondominoViewModel value, FormCollection collection)
        {
            base.BeforeCreate(ref value, collection);
            value.PrivativoAdmin = collection["PrivativoAdmin"] == "on" ? "S" : "N";
        }

        #endregion

        #region Edit
        [AuthorizeFilter]
        public ActionResult Edit(int GrupoCondominoID)
        {
            return _Edit(new GrupoCondominoViewModel() { GrupoCondominoID = GrupoCondominoID });
        }

        public override void BeforeEdit(ref GrupoCondominoViewModel value, FormCollection collection)
        {
            BeforeCreate(ref value, collection);
        }
        #endregion

        #region Delete
        [AuthorizeFilter]
        public ActionResult Delete(int GrupoCondominoID)
        {
            return Edit(GrupoCondominoID);
        }

        public override void BeforeDelete(ref GrupoCondominoViewModel value, FormCollection collection)
        {
            BeforeCreate(ref value, collection);
        }
        #endregion

        #region CrudGrupoCondominoModal
        public JsonResult CrudGrupoCondominoModal(string Descricao)
        {
            return JSonCrud(new GrupoCondominoViewModel() { Descricao = Descricao });
        }
        #endregion

        public JsonResult getNames()
        {
            return JSonTypeahead(null, new ListViewGrupoCondominos());
        }

    }
}