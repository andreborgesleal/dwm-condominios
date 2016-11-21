using App_Dominio.Controllers;
using App_Dominio.Models;
using App_Dominio.Pattern;
using App_Dominio.Security;
using DWM.Models.BI;
using DWM.Models.Entidades;
using DWM.Models.Enumeracoes;
using DWM.Models.Persistence;
using DWM.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DWM.Controllers
{
    public class FilaAtendimentoController : DwmRootController<FilaAtendimentoViewModel, FilaAtendimentoModel, ApplicationContext>
    {
        #region Inheritance
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
            if (ViewBag.ValidateRequest)
            {
                ListViewFilaAtendimento list = new ListViewFilaAtendimento();
                return this._List(index, pageSize, "Browse", list, descricao);
            }
            else
                return View();
        }
        #endregion

        #region Create
        public override void BeforeCreate(ref FilaAtendimentoViewModel value, FormCollection collection)
        {
            base.BeforeCreate(ref value, collection);
            value.VisibilidadeCondomino = collection["VisibilidadeCondomino"] == "on" ? "S" : "N";
        }
        #endregion

        #region Edit
        [AuthorizeFilter]
        public ActionResult Edit(int FilaAtendimentoID)
        {
            return _Edit(new FilaAtendimentoViewModel() { FilaAtendimentoID = FilaAtendimentoID });
        }

        public override void BeforeEdit(ref FilaAtendimentoViewModel value, FormCollection collection)
        {
            BeforeCreate(ref value, collection);
        }
        #endregion

        #region Delete
        [AuthorizeFilter]
        public ActionResult Delete(int FilaAtendimentoID)
        {
            return Edit(FilaAtendimentoID);
        }

        public override void BeforeDelete(ref FilaAtendimentoViewModel value, FormCollection collection)
        {
            BeforeCreate(ref value, collection);
        }

        #endregion
    }
}