using App_Dominio.Controllers;
using App_Dominio.Security;
using DWM.Models.Entidades;
using DWM.Models.Pattern;
using DWM.Models.Persistence;
using DWM.Models.Repositories;
using dwm_condominios.Models.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DWM.Controllers
{
    public class LimpezaInspecaoController : DwmRootController<LimpezaInspecaoViewModel, LimpezaInspecaoModel, ApplicationContext>
    {
        #region Inheritance
        public override int _sistema_id() { return (int)DWM.Models.Enumeracoes.Sistema.DWMCONDOMINIOS; }

        public override string getListName()
        {
            return "Listar Ítens de Limpeza";
        }
        #endregion

        #region List
        [AuthorizeFilter]
        public ActionResult ListLaudo(int? index, int? pagesize = 50, string descricao = null)
        {

            return View();
        }


        [AuthorizeFilter]
        public override ActionResult List(int? index, int? pageSize = 50, string descricao = null)
        {
            if (ViewBag.ValidateRequest)
            {
                if (descricao != null)
                    return ListParam(index, pageSize, descricao);
                else
                    return ListParam(index, pageSize);
            }
            else
                return View();
        }

        [AuthorizeFilter]
        public ActionResult ListParam(int? index, int? pageSize = 50, string descricao = null)
        {
            ViewBag.empresaId = DWMSessaoLocal.GetSessaoLocal().empresaId;
            ViewBag.unidades = DWMSessaoLocal.GetSessaoLocal().Unidades;
            if (ViewBag.ValidateRequest)
            {
                ListViewLimpezaInspecao l = new ListViewLimpezaInspecao();
                return this._List(index, pageSize, "Browse", l, null);
            }
            else
                return View();
        }
        #endregion

        #region Edit
        [AuthorizeFilter]
        public ActionResult Edit(int limpezaInspecaoID, int limpezaRequisitoID)
        {
            return RedirectToAction("Edit", "LimpezaInspecaoItem", new { LimpezaInspecaoID = limpezaInspecaoID, LimpezaRequisitoID = limpezaRequisitoID });
            //return _Edit(new LimpezaInspecaoViewModel() { LimpezaInspecaoID = limpezaInspecaoID });
        }
        #endregion

        #region Delete
        [AuthorizeFilter]
        public ActionResult Delete(int limpezaInspecaoID, int limpezaRequisitoID)
        {
            return RedirectToAction("Delete", "LimpezaInspecaoItem", new { LimpezaInspecaoID = limpezaInspecaoID, LimpezaRequisitoID = limpezaRequisitoID });
        }
        #endregion
    }
}