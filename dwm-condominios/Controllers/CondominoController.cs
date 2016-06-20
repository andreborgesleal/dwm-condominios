using App_Dominio.Controllers;
using App_Dominio.Security;
using DWM.Models.Enumeracoes;
using DWM.Models.Persistence;
using DWM.Models.Repositories;
using System.Web.Mvc;
using DWM.Models.Entidades;
using System;
using System.Collections.Generic;
using App_Dominio.Pattern;
using App_Dominio.Enumeracoes;
using DWM.Models.BI;
using App_Dominio.Models;
using App_Dominio.Contratos;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using DWM.Models.Persistence;

namespace DWM.Controllers
{
    public class CondominoController : DwmRootController<CondominoUnidadeViewModel, CondominoUnidadeModel, ApplicationContext>
    {
        #region Inheritance
        public override int _sistema_id() { return (int)Sistema.DWMCONDOMINIOS; }
        public override string getListName()
        {
            return "Condômino";
        }

        #region List
        public override ActionResult List(int? index, int? pageSize = 50, string descricao = null)
        {
            return ListParam(index, pageSize, Request["EdificacaoID"], Request["UnidadeID"], descricao);
        }

        [AuthorizeFilter]
        public ActionResult ListParam(int? index, int? pageSize = 50, string EdificacaoID = null, string UnidadeID = null, string descricao = null)
        {
            ViewBag.ValidateRequest = true;
            if (ViewBag.ValidateRequest)
            {
                int _EdificacaoID = EdificacaoID == null || EdificacaoID == "" ? 0 : int.Parse(EdificacaoID);
                int _UnidadeID = UnidadeID == null || UnidadeID == "" ? 0 : int.Parse(UnidadeID);

                ListViewCondominoUnidade l = new ListViewCondominoUnidade();
                return this._List(index, pageSize, "Browse", l, _EdificacaoID, _UnidadeID, descricao);
            }
            else
                return View();
        }

        #endregion

        #endregion


    }
}