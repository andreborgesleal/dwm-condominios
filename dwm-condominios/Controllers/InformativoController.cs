using App_Dominio.Controllers;
using App_Dominio.Models;
using App_Dominio.Security;
using DWM.Models.Entidades;
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
                ListViewInformativo l = new ListViewInformativo();
                return this._List(index, pageSize, "Browse", l, Funcoes.StringToDate(data1).Value, Funcoes.StringToDate(data1).Value);
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
    }
}