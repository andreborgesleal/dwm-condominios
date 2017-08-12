using App_Dominio.Contratos;
using App_Dominio.Controllers;
using App_Dominio.Pattern;
using App_Dominio.Security;
using DWM.Models.BI;
using DWM.Models.Entidades;
using DWM.Models.Enumeracoes;
using DWM.Models.Pattern;
using DWM.Models.Persistence;
using DWM.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace DWM.Controllers
{
    public class PortariaController : DwmRootController<VisitanteAcessoViewModel, VisitanteAcessoModel, ApplicationContext>
    {
        #region Inheritance
        public override int _sistema_id() { return (int)DWM.Models.Enumeracoes.Sistema.DWMCONDOMINIOS; }

        public override string getListName()
        {
            return "Listar Acesso";
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

        #region Edit
        [AuthorizeFilter]
        public ActionResult Edit(int AcessoID)
        {
            return _Edit(new VisitanteAcessoViewModel() { AcessoID = AcessoID });
        }
        #endregion

        #region Delete
        [AuthorizeFilter]
        public ActionResult Delete(int AcessoID)
        {
            return Edit(AcessoID);
        }
        #endregion

        #region Retorna as Unidades de uma dada Edificação
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
        public override ActionResult AfterCreate(VisitanteAcessoViewModel value, FormCollection collection)
        {
            if (!value.IsPortaria)
                try
                {
                    FactoryLocalhost<VisitanteAcessoViewModel, ApplicationContext> factory = new FactoryLocalhost<VisitanteAcessoViewModel, ApplicationContext>();
                    EmailPortariaBI bi = new EmailPortariaBI();
                    value.uri = this.ControllerContext.Controller.GetType().Name.Replace("Controller", "") + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
                    VisitanteAcessoViewModel a = factory.Execute(new EmailPortariaBI(), value);
                    if (a.mensagem.Code > 0)
                        throw new Exception(a.mensagem.Message);
                }
                catch (Exception ex)
                {
                    Error(ex.Message);
                }

            return RedirectToAction("Browse");
        }

        public override void OnCreateError(ref VisitanteAcessoViewModel value, FormCollection collection)
        {
            Validate error = value.mensagem;
            Facade<VisitanteAcessoViewModel, VisitanteAcessoModel, ApplicationContext> facade = new Facade<VisitanteAcessoViewModel, VisitanteAcessoModel, ApplicationContext>();
            GetCreate();
            value = facade.CreateRepository(Request);
            value.Interfona = collection["Interfona"];
            value.HoraInicio = collection["HoraInicio"];
            value.HoraLimite = collection["HoraLimite"];
            value.Observacao = collection["Observacao"];
            if (collection["DataAutorizacao"] != null && collection["DataAutorizacao"] != "")
                value.DataAutorizacao = DateTime.Parse(collection["DataAutorizacao"]);
            value.mensagem = error;
        }

        public override void OnDeleteError(ref VisitanteAcessoViewModel value, FormCollection collection)
        {
            OnEditError(ref value, collection);
        }

        public override void OnEditError(ref VisitanteAcessoViewModel value, FormCollection collection)
        {
            Validate error = value.mensagem;
            Facade<VisitanteAcessoViewModel, VisitanteAcessoModel, ApplicationContext> facade = new Facade<VisitanteAcessoViewModel, VisitanteAcessoModel, ApplicationContext>();
            GetCreate();
            value = facade.getObject(new VisitanteAcessoViewModel() { AcessoID = value.AcessoID });
            value.Interfona = collection["Interfona"];
            value.HoraInicio = collection["HoraInicio"];
            value.HoraLimite = collection["HoraLimite"];
            value.Observacao = collection["Observacao"];
            if (collection["DataAutorizacao"] != null && collection["DataAutorizacao"] != "")
                value.DataAutorizacao = DateTime.Parse(collection["DataAutorizacao"]);
            value.mensagem = error;
        }
        #endregion
    }
}