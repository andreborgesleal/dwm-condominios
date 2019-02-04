using App_Dominio.Controllers;
using App_Dominio.Security;
using DWM.Models.Entidades;
using DWM.Models.Persistence;
using DWM.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dwm_condominios.Controllers
{
    public class ConvidadosController : DwmRootController<VisitanteAcessoViewModel, VisitanteAcessoModel, ApplicationContext>
    {
        #region Inheritance
        public override int _sistema_id() { return (int)DWM.Models.Enumeracoes.Sistema.DWMCONDOMINIOS; }

        public override string getListName()
        {
            return "Listar Convidados" +
                "";
        }
        #endregion

        #region List
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
        public ActionResult ListParam(int? index, int? pageSize = 50, string descricao = null,
                                        int? edificacaoId = null, int? unidadeId = null)
        {
            ViewBag.SessaoLocal = DWMSessaoLocal.GetSessaoLocal();
            if (ViewBag.ValidateRequest)
            {
                ListViewVisitanteAcesso l = new ListViewVisitanteAcesso();
                return this._List(index, pageSize, "Browse", l, null, edificacaoId, unidadeId);
            }
            else
                return View();
        }
        #endregion

    }
}