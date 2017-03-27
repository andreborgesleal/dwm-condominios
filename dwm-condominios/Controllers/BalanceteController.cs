using App_Dominio.Controllers;
using App_Dominio.Entidades;
using App_Dominio.Models;
using App_Dominio.Security;
using DWM.Models.Entidades;
using DWM.Models.Enumeracoes;
using DWM.Models.Persistence;
using DWM.Models.Repositories;
using System;
using System.Web.Mvc;

namespace DWM.Controllers
{
    public class BalanceteController : DwmRootController<BalanceteViewModel, BalanceteModel, ApplicationContext>
    {
        #region inheritance
        public override int _sistema_id() { return (int)DWM.Models.Enumeracoes.Sistema.DWMCONDOMINIOS; }
        public override string getListName()
        {
            return "Listar";
        }
        #endregion

        #region List
        [AuthorizeFilter]
        public override ActionResult List(int? index, int? pageSize = 50, string descricao = null)
        {
            if (ViewBag.ValidateRequest)
            {
                ListViewBalanceteMensal bal = new ListViewBalanceteMensal();
                return this._List(index, pageSize, "Browse", bal, descricao);
            }
            else
                return View();
        }

        #endregion

        #region Edit
        [AuthorizeFilter]
        public ActionResult Edit(int CondominioID, int planoContaID)
        {
            return _Edit(new BalanceteViewModel() { CondominioID = CondominioID, planoContaID = planoContaID });
        }
        #endregion

        #region Delete
        [AuthorizeFilter]
        public ActionResult Delete(int CondominioID, int planoContaID)
        {
            return _Edit(new BalanceteViewModel() { CondominioID = CondominioID, planoContaID = planoContaID });
        }
        #endregion
    }
}