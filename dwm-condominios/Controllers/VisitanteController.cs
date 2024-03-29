﻿using App_Dominio.Controllers;
using App_Dominio.Models;
using App_Dominio.Pattern;
using App_Dominio.Security;
using DWM.Models.BI;
using DWM.Models.Entidades;
using DWM.Models.Enumeracoes;
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
    public class VisitanteController : DwmRootController<VisitanteViewModel, VisitanteModel, ApplicationContext>
    {
        #region Inheritance
        public override int _sistema_id() { return (int)DWM.Models.Enumeracoes.Sistema.DWMCONDOMINIOS; }

        public override string getListName()
        {
            return "Listar Visitantes";
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
            ViewBag.empresaId = DWMSessaoLocal.GetSessaoLocal().empresaId;
            ViewBag.unidades = DWMSessaoLocal.GetSessaoLocal().Unidades;
            if (ViewBag.ValidateRequest)
            {
                ListViewVisitante l = new ListViewVisitante();
                return this._List(index, pageSize, "Browse", l, null, edificacaoId, unidadeId);
            }
            else
                return View();
        }
        #endregion

        #region Create
        public override void BeforeCreate(ref VisitanteViewModel value, FormCollection collection)
        {
            if (value.PrestadorCondominio == "N")
            {
                value.VisitanteUnidadeViewModel = new List<VisitanteUnidadeViewModel>
                {
                    new VisitanteUnidadeViewModel()
                    {
                        CondominioID = DWMSessaoLocal.GetSessaoLocal().empresaId,
                        EdificacaoID = value.EdificacaoID.GetValueOrDefault(),
                        UnidadeID = value.UnidadeID.GetValueOrDefault(),
                        empresaId = DWMSessaoLocal.GetSessaoLocal().empresaId,
                    }
                };
                //value.Fotografia = collection["Fotografia"];
            }
        }

        [AuthorizeFilter]
        public override ActionResult Create()
        {
            ViewBag.empresaId = DWMSessaoLocal.GetSessaoLocal().empresaId;
            ViewBag.unidades = DWMSessaoLocal.GetSessaoLocal().Unidades;
            ViewBag.op = (Request["op"] != null && "IU".Contains(Request["op"]))  ? Request["op"] : "";

            if (ViewBag.ValidateRequest)
            {
                Facade<VisitanteViewModel, VisitanteModel, ApplicationContext> facade = new Facade<VisitanteViewModel, VisitanteModel, ApplicationContext>();
                GetCreate();
                return View(facade.CreateRepository(Request));
            }
            else
                return null;
        }

        public override ActionResult AfterCreate(VisitanteViewModel value, FormCollection collection)
        {
            if (collection["op"] != null && collection["op"] == "I")
                return RedirectToAction("Create", "Portaria");
            else if (collection["op"] != null && collection["op"] == "U")
                return RedirectToAction("Edit", "Portaria");
            else
                return base.AfterCreate(value, collection);
        }
        #endregion

        #region Edit
        [AuthorizeFilter]
        public ActionResult Edit(int visitanteID)
        {
            // Se o usuário logado for um condômino, verifica se o VisitanteID é do respectivo Condômimo (se for portaria ou administração, pode editar)
            FactoryLocalhost<VisitanteViewModel, ApplicationContext> factory = new FactoryLocalhost<VisitanteViewModel, ApplicationContext>();
            VisitanteViewModel value = factory.Execute(new VisitanteChecarEdicaoBI(), new VisitanteViewModel() { VisitanteID = visitanteID });
            if (value.mensagem.Code == -2)
            {
                Error("Visitante não autorizado para edição");
                return RedirectToAction("Browse");
            }

            ViewBag.op = (Request["op"] != null && Request["op"] == "I") ? Request["op"] : "";
            return _Edit(new VisitanteViewModel() { VisitanteID = visitanteID });
        }

        public override ActionResult AfterEdit(VisitanteViewModel value, FormCollection collection)
        {
            return AfterCreate(value, collection);
        }
        #endregion

        #region Delete
        [AuthorizeFilter]
        public ActionResult Delete(int visitanteID)
        {
            return Edit(visitanteID);
        }
        #endregion

        #region Retorno as Unidades de uma dada Edificação
        [AllowAnonymous]
        public JsonResult GetNames(string term, int tag)
        {
            var results = new BindDropDownList().Unidades2("Selecione...", "", term, tag);

            return new JsonResult()
            {
                Data = results,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }
        #endregion

        #region Callbacks Filters
        public override void OnCreateError(ref VisitanteViewModel value, FormCollection collection)
        {
            if (value.PrestadorCondominio == "N")
            {
                value.EdificacaoID = int.Parse(collection["EdificacaoID"]);
                value.UnidadeID = int.Parse(collection["UnidadeID"]);
                ViewBag.unidades = DWMSessaoLocal.GetSessaoLocal().Unidades;
            }
        }

        public override void OnEditError(ref VisitanteViewModel value, FormCollection collection)
        {
            if (value.PrestadorCondominio == "N")
            {
                value.EdificacaoID = int.Parse(collection["EdificacaoID"]);
                value.UnidadeID = int.Parse(collection["UnidadeID"]);
                ViewBag.unidades = DWMSessaoLocal.GetSessaoLocal().Unidades;
            }
        }
        #endregion
    }
}