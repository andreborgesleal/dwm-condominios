using App_Dominio.Controllers;
using App_Dominio.Entidades;
using App_Dominio.Models;
using App_Dominio.Security;
using DWM.Models.Entidades;
using DWM.Models.Enumeracoes;
using DWM.Models.Persistence;
using DWM.Models.Repositories;
using System;
using System.Web.Mvc;

namespace DWM.Controllers
{
    public class DocumentosController : DwmRootController<ArquivoViewModel, ArquivoModel, DWM.Models.Entidades.ApplicationContext>
    {
        #region inheritance
        public override int _sistema_id() { return (int)DWM.Models.Enumeracoes.Sistema.DWMCONDOMINIOS; }
        public override string getListName()
        {
            return "Listar";
        }
        #endregion

        #region List
        [AuthorizeFilter]
        public override ActionResult List(int? index, int? pageSize = 50, string descricao = null)
        {
            return ListParam(index, 25, Request["data1"], Request["data2"], 
                                Request["EdificacaoID"], Request["UnidadeID"], 
                                Request["CondominoID"], Request["GrupoCondominoID"], descricao);
        }

        [AuthorizeFilter]
        public ActionResult ListParam(int? index, int? pageSize = 25, 
                                        string data1 = null, string data2 = null, 
                                        string EdificacaoID = null, string UnidadeID = null, string CondominoID = null,
                                        string GrupoCondominoID = null,
                                        string Nome = null)
        {
            if (ViewBag.ValidateRequest)
            {
                ListViewArquivo l = new ListViewArquivo();
                return _ListParam(l, index, pageSize, data1, data2, EdificacaoID, UnidadeID, CondominoID, GrupoCondominoID, Nome);
            }
            else
                return View();
        }

        [AuthorizeFilter]
        public ActionResult ListParamHome(int? index, int? pageSize = 12,
                                        string data1 = null, string data2 = null,
                                        string EdificacaoID = null, string UnidadeID = null, string CondominoID = null,
                                        string GrupoCondominoID = null,
                                        string Nome = null)
        {
            ViewBag.ValidateRequest = true;
            if (ViewBag.ValidateRequest)
            {
                ListViewArquivoHome l = new ListViewArquivoHome();
                return _ListParam(l, index, pageSize, data1, data2, EdificacaoID, UnidadeID, CondominoID, GrupoCondominoID, Nome);
            }
            else
                return View();
        }

        private ActionResult _ListParam(ListViewModelLocal<ArquivoViewModel> list, 
                                        int? index, int? pageSize = 12,
                                        string data1 = null, string data2 = null,
                                        string EdificacaoID = null, string UnidadeID = null, string CondominoID = null,
                                        string GrupoCondominoID = null,
                                        string Nome = null)
        {
            DateTime _data1 = data1 == null || data1 == "" ? Funcoes.Brasilia().AddMonths(-3) : Funcoes.StringToDate(data1).Value;
            DateTime _data2 = data2 == null || data2 == "" ? Funcoes.Brasilia().Date.AddDays(1).AddMinutes(-1) : Funcoes.StringToDate(data2).Value.Date.AddDays(1).AddMinutes(-1);
            int _EdificacaoID = EdificacaoID == null || EdificacaoID == "" ? 0 : int.Parse(EdificacaoID);
            int _UnidadeID = UnidadeID == null || UnidadeID == "" ? 0 : int.Parse(UnidadeID);
            int _CondominoID = CondominoID == null || CondominoID == "" ? 0 : int.Parse(CondominoID);
            int _GrupoCondominoID = GrupoCondominoID == null || GrupoCondominoID == "" ? 0 : int.Parse(GrupoCondominoID);

            EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
            Sessao sessaoCorrente = security.getSessaoCorrente();
            SessaoLocal SessaoLocal = DWMSessaoLocal.GetSessaoLocal();

            ViewBag.empresaId = sessaoCorrente.empresaId;
            ViewBag.CondominoID = SessaoLocal.CondominoID;

            if (SessaoLocal.CondominoID == 0)
                return this._List(index, pageSize, "Browse", list, _data1, _data2, _EdificacaoID, _UnidadeID, _CondominoID, _GrupoCondominoID, Nome);
            else
                return this._List(index, pageSize, "Browse", list, _data1, _data2, _EdificacaoID, _UnidadeID, SessaoLocal.CondominoID, _GrupoCondominoID, Nome);
        }

        #endregion

        #region Edit
        [AuthorizeFilter]
        public ActionResult Edit(string FileID)
        {
            return _Edit(new ArquivoViewModel() { FileID = FileID });
        }
        #endregion

        #region Delete
        [AuthorizeFilter]
        public ActionResult Delete(string FileID)
        {
            return _Edit(new ArquivoViewModel() { FileID = FileID });
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