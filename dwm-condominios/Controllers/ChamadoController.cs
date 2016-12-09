using App_Dominio.Controllers;
using App_Dominio.Security;
using DWM.Models.Enumeracoes;
using DWM.Models.Persistence;
using DWM.Models.Repositories;
using System.Web.Mvc;
using DWM.Models.Entidades;
using App_Dominio.Entidades;
using App_Dominio.Pattern;
using DWM.Models.BI;
using System;
using App_Dominio.Enumeracoes;
using DWM.Models.Pattern;
using System.Collections.Generic;
using App_Dominio.Models;

namespace DWM.Controllers
{
    public class ChamadoController : DwmRootController<ChamadoViewModel, ChamadoModel, ApplicationContext>
    {
        #region Inheritance
        public override int _sistema_id() { return (int)DWM.Models.Enumeracoes.Sistema.DWMCONDOMINIOS; }

        public override string getListName()
        {
            return "Listar";
        }

        #region List
        public override ActionResult List(int? index, int? pageSize = 25, string descricao = null)
        {
            if (ViewBag.ValidateRequest)
            {
                ListViewChamado cham = new ListViewChamado();
                return this._List(index, pageSize, "Browse", cham, descricao);
            }
            else
                return View();
        }
        #endregion
        #endregion



    }
}