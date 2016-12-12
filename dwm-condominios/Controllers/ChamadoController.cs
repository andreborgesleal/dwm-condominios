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
using App_Dominio.Contratos;
using App_Dominio.Repositories;

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

        public override void BeforeCreate(ref ChamadoViewModel value, FormCollection collection)
        {
            base.BeforeCreate(ref value, collection);

            #region Anexos
            value.Anexos = new List<ChamadoAnexoViewModel>();
            if (collection["File1ID"] != null && collection["File1ID"] != "")
            {
                ChamadoAnexoViewModel anexo1 = new ChamadoAnexoViewModel()
                {
                    FileID = collection["File1ID"],
                    DataAnexo = Funcoes.Brasilia(),
                    NomeOriginal = collection["NomeOriginal1"],
                };
                ((List<ChamadoAnexoViewModel>)value.Anexos).Add(anexo1);
            }
            if (collection["File2ID"] != null && collection["File2ID"] != "")
            {
                ChamadoAnexoViewModel anexo2 = new ChamadoAnexoViewModel()
                {
                    FileID = collection["File2ID"],
                    DataAnexo = Funcoes.Brasilia(),
                    NomeOriginal = collection["NomeOriginal2"],
                };
                ((List<ChamadoAnexoViewModel>)value.Anexos).Add(anexo2);
            }
            if (collection["File3ID"] != null && collection["File3ID"] != "")
            {
                ChamadoAnexoViewModel anexo3 = new ChamadoAnexoViewModel()
                {
                    FileID = collection["File3ID"],
                    DataAnexo = Funcoes.Brasilia(),
                    NomeOriginal = collection["NomeOriginal3"],
                };
                ((List<ChamadoAnexoViewModel>)value.Anexos).Add(anexo3);
            }
            #endregion

            #region Identificação do Condômino
            if (collection ["__EdificacaoID"] != null && collection["__EdificacaoID"] != "")
            {
                value.EdificacaoID = int.Parse(collection["__EdificacaoID"]);
                value.UnidadeID = int.Parse(collection["__UnidadeID"]);
                value.CondominoID = int.Parse(collection["__CondominoID"]);
            }
            #endregion

            if (Request ["_ChamadoMotivoID"] != null && Request["_ChamadoMotivoID"] != "")
                value.ChamadoMotivoID = int.Parse(collection["_ChamadoMotivoID"]);

            if (Request["_FilaSolicitanteID"] != null && Request["_FilaSolicitanteID"] != "")
                value.FilaSolicitanteID = int.Parse(collection["_FilaSolicitanteID"]);

            if (Request["_FilaAtendimentoID"] != null && Request["_FilaAtendimentoID"] != "")
            {
                value.FilaAtendimentoID = int.Parse(collection["_FilaAtendimentoID"]);
                value.ChamadoFilaViewModel = new ChamadoFilaViewModel()
                {
                    Data = Funcoes.Brasilia(),
                    FilaAtendimentoID = value.FilaAtendimentoID.Value
                };
            }

            value.MensagemOriginal = collection["MensagemOriginal"];
            value.uri = this.ControllerContext.Controller.GetType().Name.Replace("Controller", "") + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
        }

        public override void OnCreateError(ref ChamadoViewModel value, FormCollection collection)
        {
            base.OnCreateError(ref value, collection);
            FacadeLocalhost<ChamadoViewModel, ChamadoModel, ApplicationContext> facade = new FacadeLocalhost<ChamadoViewModel, ChamadoModel, ApplicationContext>();
            Validate validate = value.mensagem;

            value = facade.CreateRepository(Request);
            value.mensagem = validate;
            value.MensagemOriginal = collection["MensagemOriginal"] ?? "";
        }

        public override ActionResult AfterCreate(ChamadoViewModel value, FormCollection collection)
        {
            try
            {
                FactoryLocalhost<AlertaRepository, ApplicationContext> factory = new FactoryLocalhost<AlertaRepository, ApplicationContext>();
                AlertaBI bi = new AlertaBI();
                value.uri = this.ControllerContext.Controller.GetType().Name.Replace("Controller", "") + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
                AlertaRepository a = factory.Execute(new AlertaBI(), value);
                if (a.mensagem.Code > 0)
                    throw new Exception(a.mensagem.Message);
            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }

            return RedirectToAction("Browse");
        }


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