using App_Dominio.Controllers;
using App_Dominio.Security;
using DWM.Models.Enumeracoes;
using DWM.Models.Persistence;
using DWM.Models.Repositories;
using System.Web.Mvc;

namespace DWM.Controllers
{
    public class UnidadeController : DwmRootController<UnidadeViewModel, UnidadeModel, DWM.Models.Entidades.ApplicationContext> 
    {
        #region inheritance
        public override int _sistema_id() { return (int)Sistema.DWMCONDOMINIOS; }
        public override string getListName()
        {
            return "Listar";
        }
        #endregion

        #region List
        [AuthorizeFilter]
        public override ActionResult List(int? index, int? pageSize = 50, string descricao = null)
        {
            if (ViewBag.ValidateRequest)
            {
                ViewBag.EdificacaoDescricao = DWM.Models.Entidades.DWMSessaoLocal.GetTipoEdificacao(0).Descricao.ToString();
                ListViewUnidades und = new ListViewUnidades();
                return this._List(index, pageSize, "Browse", und, descricao);
            }
            else
                return View();
        }
        #endregion

        #region Create
        public override void BeforeCreate(ref UnidadeViewModel value, FormCollection collection)
        {
            base.BeforeCreate(ref value, collection);
        }

        #endregion

        #region Edit
        [AuthorizeFilter]
        public ActionResult Edit(int EdificacaoID, int UnidadeID)
        {
            return _Edit(new UnidadeViewModel()
                            { CondominioID = DWM.Models.Entidades.DWMSessaoLocal.GetSessaoLocal().empresaId,
                                EdificacaoID = EdificacaoID,
                                UnidadeID = UnidadeID
                            });
        }

        public override void BeforeEdit(ref UnidadeViewModel value, FormCollection collection)
        {
            BeforeCreate(ref value, collection);
        }
        #endregion

        #region Delete
        [AuthorizeFilter]
        public ActionResult Delete(int EdificacaoID, int UnidadeID)
        {
            return Edit(EdificacaoID, UnidadeID);
        }

        public override void BeforeDelete(ref UnidadeViewModel value, FormCollection collection)
        {
            BeforeCreate(ref value, collection);
        }
        #endregion

    }
}