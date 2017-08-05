using App_Dominio.Controllers;
using App_Dominio.Pattern;
using App_Dominio.Security;
using DWM.Models.Entidades;
using DWM.Models.Enumeracoes;
using DWM.Models.Persistence;
using DWM.Models.Repositories;
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
            return "Listar Acesso de Visitantes e Prestadores";
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

        #region Retorna as Unidades de uma dada Edificação
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

        #region Callbacks Filters
        //public override void OnCreateError(ref VisitanteAcessoViewModel value, FormCollection collection)
        //{
        //    if (value.PrestadorCondominio == "N")
        //    {
        //        value.EdificacaoID = int.Parse(collection["EdificacaoID"]);
        //        value.UnidadeID = int.Parse(collection["UnidadeID"]);
        //        ViewBag.unidades = DWMSessaoLocal.GetSessaoLocal().Unidades;
        //    }
        //}

        //public override void OnDeleteError(ref VisitanteAcessoViewModel value, FormCollection collection)
        //{
        //    if (value.PrestadorCondominio == "N")
        //    {
        //        value.EdificacaoID = int.Parse(collection["EdificacaoID"]);
        //        value.UnidadeID = int.Parse(collection["UnidadeID"]);
        //        ViewBag.unidades = DWMSessaoLocal.GetSessaoLocal().Unidades;
        //    }
        //}

        //public override void OnEditError(ref VisitanteAcessoViewModel value, FormCollection collection)
        //{
        //    if (value.PrestadorCondominio == "N")
        //    {
        //        value.EdificacaoID = int.Parse(collection["EdificacaoID"]);
        //        value.UnidadeID = int.Parse(collection["UnidadeID"]);
        //        ViewBag.unidades = DWMSessaoLocal.GetSessaoLocal().Unidades;
        //    }
        //}
        #endregion
    }
}