using App_Dominio.Controllers;
using App_Dominio.Models;
using App_Dominio.Pattern;
using App_Dominio.Security;
using DWM.Models.Entidades;
using DWM.Models.Enumeracoes;
using DWM.Models.Persistence;
using DWM.Models.Repositories;
using dwm_condominios.Models.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

// 

namespace dwm_condominios.Controllers
{
    public class AluguelController : DwmRootController<AluguelEspacoViewModel, AluguelEspacoModel, ApplicationContext>
    {
        #region Inheritance
        public override int _sistema_id() { return (int)DWM.Models.Enumeracoes.Sistema.DWMCONDOMINIOS; }

        public override string getListName()
        {
            return "Listar Alugueis";
        }
        #endregion

        #region List
        [AuthorizeFilter]
        public override ActionResult List(int? index, int? pageSize = 50, string descricao = null)
        {
            if (ViewBag.ValidateRequest)
            {
                ViewBag.CondominioID = DWMSessaoLocal.GetSessaoLocal().empresaId;
                if (descricao == null || String.IsNullOrEmpty(descricao))
                {
                    #region Recuperar o primeiro espaço
                    var espacos = new BindDropDownList().EspacosComuns("", "", (int)ViewBag.CondominioID);
                    descricao = espacos.FirstOrDefault().Value;
                    #endregion
                }
                return ListParam(index, pageSize, descricao);
            }
            else
                return View();
        }

        [AuthorizeFilter]
        public ActionResult ListParam(int? index, int? pageSize = 50, string descricao = null)
        {
            ViewBag.empresaId = DWMSessaoLocal.GetSessaoLocal().empresaId;
            ViewBag.unidades = DWMSessaoLocal.GetSessaoLocal().Unidades;
            ViewBag.CondominoID = DWMSessaoLocal.GetSessaoLocal().CondominoID;

            if (ViewBag.ValidateRequest)
            {
                Facade<AluguelEspacoViewModel, AluguelEspacoModel, ApplicationContext> facade = new Facade<AluguelEspacoViewModel, AluguelEspacoModel, ApplicationContext>();
                ViewBag.AluguelEspacoViewModel =  facade.CreateRepository(Request);
                ViewBag.TipoEdificacao = DWMSessaoLocal.GetTipoEdificacao(null).Descricao;
                ListViewAluguelEspaco l = new ListViewAluguelEspaco();
                return this._List(index, pageSize, "Browse", l, descricao);
            }
            else
                return View();
        }

        #region Minhas Reservas
        [AuthorizeFilter]
        public ActionResult MinhasReservas(int? index = 0, int pageSize = 50, string descricao = null)
        {
            if (ViewBag.ValidateRequest)
            {
                BindBreadCrumb(getListName(), ClearBreadCrumbOnBrowse());

                TempData.Remove("Controller");
                TempData.Add("Controller", this.ControllerContext.RouteData.Values["controller"].ToString());

                return ListMinhasReservas(index, this.PageSize, descricao);
            }
            else
                return null;
        }

        public ActionResult ListMinhasReservas(int? index, int? pageSize = 50, string descricao = null)
        {
            ViewBag.ValidateRequest = true;
            if (ViewBag.ValidateRequest)
            {
                if (descricao != null)
                    return ListReservas(index, pageSize, descricao);
                else
                    return ListReservas(index, pageSize);
            }
            else
                return View();
        }

        [AuthorizeFilter]
        public ActionResult ListReservas(int? index, int? pageSize = 50, string descricao = null)
        {
            ViewBag.empresaId = DWMSessaoLocal.GetSessaoLocal().empresaId;
            ViewBag.unidades = DWMSessaoLocal.GetSessaoLocal().Unidades;
            ViewBag.CondominoID = DWMSessaoLocal.GetSessaoLocal().CondominoID;
            if (ViewBag.ValidateRequest)
            {
                ViewBag.TipoEdificacao = DWMSessaoLocal.GetTipoEdificacao(null).Descricao;
                ListViewMinhasReservas l = new ListViewMinhasReservas();
                return this._List(index, pageSize, "Browse", l, null);
            }
            else
                return View();
        }
        #endregion

        #endregion

        [AuthorizeFilter]
        public override ActionResult Create()
        {
            ViewBag.empresaId = DWMSessaoLocal.GetSessaoLocal().empresaId;
            ViewBag.unidades = DWMSessaoLocal.GetSessaoLocal().Unidades;
            ViewBag.op = (Request["op"] != null && "IU".Contains(Request["op"])) ? Request["op"] : "";

            if (ViewBag.ValidateRequest)
            {
                Facade<AluguelEspacoViewModel, AluguelEspacoModel, ApplicationContext> facade = new Facade<AluguelEspacoViewModel, AluguelEspacoModel, ApplicationContext>();
                GetCreate();
                return View(facade.CreateRepository(Request));
            }
            else
                return null;
        }

        public override ActionResult AfterCreate(AluguelEspacoViewModel value, FormCollection collection)
        {
            
            if (collection ["IsCalendar"] == "S")
            {
                base.AfterCreate(value, collection);
                return RedirectToAction("Browse");
            }
            else
                return base.AfterCreate(value, collection);
        }

        public override ActionResult ViewForm(AluguelEspacoViewModel value, FormCollection collection)
        {
            if (collection["IsCalendar"] == "S")
            {
                return RedirectToAction("Browse");
            }
            else
                return base.ViewForm(value, collection);
        }
        #region Edit
        [AuthorizeFilter]
        public ActionResult Edit(int AluguelID)
        {
            // Se o usuário logado for um condômino, verifica se o VisitanteID é do respectivo Condômimo (se for portaria ou administração, pode editar)
            //FactoryLocalhost<VisitanteViewModel, ApplicationContext> factory = new FactoryLocalhost<VisitanteViewModel, ApplicationContext>();
            //VisitanteViewModel value = factory.Execute(new VisitanteChecarEdicaoBI(), new VisitanteViewModel() { VisitanteID = visitanteID });
            //if (value.mensagem.Code == -2)
            //{
            //    Error("Visitante não autorizado para edição");
            //    return RedirectToAction("Browse");
            //}

            //ViewBag.op = (Request["op"] != null && Request["op"] == "I") ? Request["op"] : "";
            return _Edit(new AluguelEspacoViewModel() { AluguelID = AluguelID });
        }

        public override void BeforeEdit(ref AluguelEspacoViewModel value, FormCollection collection)
        {
            value.uri = this.ControllerContext.Controller.GetType().Name.Replace("Controller", "") + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
            base.BeforeEdit(ref value, collection);
        }
        #endregion

        #region Delete
        [AuthorizeFilter]
        public ActionResult Delete(int AluguelID)
        {
            return Edit(AluguelID);
        }
        #endregion

        #region Retorno as Unidades de uma dada Edificação
        [AllowAnonymous]
        public JsonResult GetNames(string term, int tag)
        {
            var results = new BindDropDownList().Unidades("Selecione...", "", term, tag);

            return new JsonResult()
            {
                Data = results,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }
        #endregion
    }
}