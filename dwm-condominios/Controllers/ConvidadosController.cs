using App_Dominio.Contratos;
using App_Dominio.Controllers;
using App_Dominio.Enumeracoes;
using App_Dominio.Pattern;
using App_Dominio.Security;
using DWM.Models.BI;
using DWM.Models.Entidades;
using DWM.Models.Pattern;
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
            ViewBag.aluguelId = Request["AluguelID"];
            ViewBag.dataEvento = Request["DataEvento"];

            ViewBag.SessaoLocal = DWMSessaoLocal.GetSessaoLocal();
            if (ViewBag.ValidateRequest)
            {
                ListViewVisitanteAcessoEspaco l = new ListViewVisitanteAcessoEspaco();
                return this._List(index, pageSize, "Browse", l, ViewBag.aluguelId, ViewBag.dataEvento);
            }
            else
                return View();
        }
        #endregion

        [AuthorizeFilter]
        public override ActionResult Create()
        {
            ViewBag.op = (Request["op"] != null && "IU".Contains(Request["op"])) ? Request["op"] : "";
            ViewBag.AluguelID = Request["AluguelID"];
            ViewBag.DataEvento = Request["DataEvento"];

            if (ViewBag.ValidateRequest)
            {
                Facade<VisitanteAcessoViewModel, VisitanteAcessoModel, ApplicationContext> facade = new Facade<VisitanteAcessoViewModel, VisitanteAcessoModel, ApplicationContext>();
                GetCreate();
                return View(facade.CreateRepository(Request));
            }
            else
                return null;
        }

        #region Edit
        [AuthorizeFilter]
        public ActionResult Edit(int AcessoID)
        {
            // Se o usuário logado for um condômino, não pode editar o acesso de outro condômino. 
            FactoryLocalhost<VisitanteAcessoViewModel, ApplicationContext> factory = new FactoryLocalhost<VisitanteAcessoViewModel, ApplicationContext>();
            VisitanteAcessoViewModel value = factory.Execute(new VisitanteAcessoChecarEdicaoBI(), new VisitanteAcessoViewModel() { AcessoID = AcessoID });
            if (value.mensagem.Code == -2)
            {
                Error("Registro não autorizado para edição");
                return RedirectToAction("Browse");
            }

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

        public override VisitanteAcessoViewModel Insert(VisitanteAcessoViewModel value)
        {
            FacadeLocalhost<VisitanteAcessoViewModel, VisitanteAcessoModel, ApplicationContext> facade = new FacadeLocalhost<VisitanteAcessoViewModel, VisitanteAcessoModel, ApplicationContext>();
            return facade.Save(value, Crud.INCLUIR);
        }

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

            Facade<VisitanteAcessoViewModel, VisitanteAcessoModel, ApplicationContext> facade = new Facade<VisitanteAcessoViewModel, VisitanteAcessoModel, ApplicationContext>();
            GetCreate();
            value = facade.CreateRepository(Request);
            value.Interfona = collection["Interfona"];
            value.HoraInicio = collection["HoraInicio"];
            value.HoraLimite = collection["HoraLimite"];
            value.Observacao = collection["Observacao"];

            value.DataAutorizacao = Convert.ToDateTime(collection["DataAutorizacao"]);
            value.AluguelID = int.Parse(collection["AluguelID"]);

            ViewBag.AluguelID = int.Parse(collection["AluguelID"]); ;
            ViewBag.DataEvento = Convert.ToDateTime(collection["DataAutorizacao"]); ;

            return RedirectToAction("Create", new { value.AluguelID, DataEvento = value.DataAutorizacao.ToString("yyyy-MM-dd") });
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

            value.DataAutorizacao = Convert.ToDateTime(collection["DataAutorizacao"]);
            value.AluguelID = int.Parse(collection["AluguelID"]);

            ViewBag.AluguelID = int.Parse(collection["AluguelID"]); ;
            ViewBag.DataEvento = Convert.ToDateTime(collection["DataAutorizacao"]); ;

            if (collection["DataAutorizacao"] != null && collection["DataAutorizacao"] != "")
                value.DataAutorizacao = DateTime.Parse(collection["DataAutorizacao"]);
            value.mensagem = error;
        }

        public override void OnDeleteError(ref VisitanteAcessoViewModel value, FormCollection collection)
        {
            OnEditError(ref value, collection);
        }
        #endregion
    }
}