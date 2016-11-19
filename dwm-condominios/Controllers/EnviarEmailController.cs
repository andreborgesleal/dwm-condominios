using App_Dominio.Controllers;
using App_Dominio.Models;
using DWM.Models.Enumeracoes;
using DWM.Models.Persistence;
using DWM.Models.Repositories;
using System;
using System.Web.Mvc;


namespace dwm_condominios.Controllers
{
    public class EnviarEmailController : DwmRootController<EmailLogViewModel, EmailLogModel, DWM.Models.Entidades.ApplicationContext>
    {

        public override int _sistema_id() { return (int)Sistema.DWMCONDOMINIOS; }
        public override string getListName()
        {
            return "Listar";
        }

        public override ActionResult List(int? index, int? PageSize, string descricao = null)
        {
            return ListParam(index, PageSize);
        }

        public ActionResult ListParam(int? index, int? pageSize = 50, string data1 = "", string data2 = "")
        {
            if (ViewBag.ValidateRequest)
            {
                if (data1 == null || data1 == "")
                {
                    data1 = "01" + DateTime.Today.AddMonths(-1).ToString("/MM/yyyy");
                    data2 = Convert.ToDateTime(DateTime.Today.AddMonths(1).ToString("yyyy-MM-") + "01").AddDays(-1).ToString("dd/MM/yyyy");
                }

                //InformativoCadastrarBI list = new InformativoCadastrarBI();
                ListViewInformativo list = new ListViewInformativo();
                return this._List(index, pageSize, "Browse", list, Funcoes.StringToDate(data1).Value, Funcoes.StringToDate(data2).Value);
            }
            else
                return View();
        }
    }
}