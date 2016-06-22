using App_Dominio.Controllers;
using App_Dominio.Security;
using DWM.Models.Enumeracoes;
using DWM.Models.Persistence;
using DWM.Models.Repositories;
using System.Web.Mvc;
using DWM.Models.Entidades;
using App_Dominio.Entidades;

namespace DWM.Controllers
{
    public class CondominoController : DwmRootController<CondominoUnidadeViewModel, CondominoUnidadeModel, ApplicationContext>
    {
        #region Inheritance
        public override int _sistema_id() { return (int)DWM.Models.Enumeracoes.Sistema.DWMCONDOMINIOS; }

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
            //ViewBag.ValidateRequest = true;
            if (ViewBag.ValidateRequest)
            {
                int _EdificacaoID = EdificacaoID == null || EdificacaoID == "" ? 0 : int.Parse(EdificacaoID);
                int _UnidadeID = UnidadeID == null || UnidadeID == "" ? 0 : int.Parse(UnidadeID);

                EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
                ViewBag.empresaId = security.getSessaoCorrente().empresaId;

                ListViewCondominoUnidade l = new ListViewCondominoUnidade();
                return this._List(index, pageSize, "Browse", l, _EdificacaoID, _UnidadeID, descricao);
            }
            else
                return View();
        }

        #endregion

        #endregion

        #region Enviar Token
        [AuthorizeFilter]
        public ActionResult EnviarToken()
        {
            ViewBag.ValidateRequest = true;
            if (ViewBag.ValidateRequest)
            {
                EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
                ViewBag.empresaId = security.getSessaoCorrente().empresaId;

                return View(new UnidadeViewModel());
            }
            return View();
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