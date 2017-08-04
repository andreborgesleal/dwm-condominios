using App_Dominio.Controllers;
using App_Dominio.Entidades;
using App_Dominio.Enumeracoes;
using App_Dominio.Models;
using App_Dominio.Pattern;
using App_Dominio.Security;
using DWM.Models.BI;
using DWM.Models.Entidades;
using DWM.Models.Repositories;
using dwm_condominios.Models.BI;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace DWM.Controllers
{
    public class HomeController : SuperController
    {
        #region Inheritance
        public override int _sistema_id() { return (int)DWM.Models.Enumeracoes.Sistema.DWMCONDOMINIOS; }

        public override string getListName()
        {
            return "Detalhar";
        }

        public override ActionResult List(int? index, int? PageSize, string descricao = null)
        {
            throw new NotImplementedException();
        }
        #endregion

        public ActionResult Index()
        {
            return RedirectToAction("Default");
        }

        [AuthorizeFilter]
        public ActionResult Default(int? index, int? pageSize = 15, int? concursoId = null, DateTime? data1 = null)
        {
            if (ViewBag.ValidateRequest)
            {
                // Obter todos os Informativos

                #region ListPanorama
                HomeViewModel home = new HomeViewModel();

                Factory<HomeViewModel, ApplicationContext> factory = new Factory<HomeViewModel, ApplicationContext>();
                home = factory.Execute(new HomeBI(), home);
                ViewBag.UsuarioPortaria = home.UsuarioGrupos.Find(x => x.descricao == "Portaria") != null;
                return View(home);
                #endregion
            }
            else
                return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        #region Alerta - segurança
        public ActionResult ReadAlert(int? alertaId)
        {
            try
            {
                EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
                if (alertaId.HasValue && alertaId > 0)
                    security.ReadAlert(alertaId.Value);
            }
            catch
            {
                return null;
            }

            return null;
        }
        #endregion
    }
}