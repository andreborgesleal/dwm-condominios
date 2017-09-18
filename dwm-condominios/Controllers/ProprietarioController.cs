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
using System.Web;
using System.Linq;
using System.IO;
using System.Web.Helpers;
using App_Dominio.Repositories;

namespace DWM.Controllers
{
    public class ProprietarioController : DwmRootController<ProprietarioViewModel, ProprietarioModel, ApplicationContext>
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
                ListViewProprietario pro = new ListViewProprietario();
                return this._List(index, pageSize, "Browse", pro, descricao);
            }
            else
                return View();
        }
        #endregion

        #endregion

        public override void BeforeCreate(ref ProprietarioViewModel value, FormCollection collection)
        {
            value.ProprietarioUnidades = new List<ProprietarioUnidadeViewModel>();
            ProprietarioUnidadeViewModel pu = new ProprietarioUnidadeViewModel();
            if (collection ["empresaId"] != null && collection["empresaId"] != "")
                pu.CondominioID = int.Parse(collection["empresaId"]);
            if (collection["EdificacaoID"] != null && collection["EdificacaoID"] != "")
                pu.EdificacaoID = int.Parse(collection["EdificacaoID"]);
            if (collection["UnidadeID"] != null && collection["UnidadeID"] != "")
                pu.UnidadeID = int.Parse(collection["UnidadeID"]);
            pu.ProprietarioID = value.ProprietarioID;
            ((List<ProprietarioUnidadeViewModel>)value.ProprietarioUnidades).Add(pu);
            base.BeforeCreate(ref value, collection);
        }

        #region Edit
        [AuthorizeFilter]
        public ActionResult Edit(int ProprietarioID)
        {
            return _Edit(new ProprietarioViewModel() { ProprietarioID = ProprietarioID });
        }
        #endregion

        #region Delete
        [AuthorizeFilter]
        public ActionResult Delete(int ProprietarioID)
        {
            return _Edit(new ProprietarioViewModel() { ProprietarioID = ProprietarioID });
        }
        #endregion

        #region Retorno as Unidades de uma dada Edificação
        [AllowAnonymous]
        public JsonResult GetNames(string term, int tag)
        {
            var results = new BindDropDownList().UnidadesSemProprietario("Selecione...", "", term, tag);

            return new JsonResult()
            {
                Data = results,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }
        #endregion
    }
}