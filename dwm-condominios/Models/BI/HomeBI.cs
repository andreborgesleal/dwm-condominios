using System;
using System.Collections.Generic;
using App_Dominio.Contratos;
using App_Dominio.Entidades;
using App_Dominio.Component;
using DWM.Models.Repositories;
using DWM.Models.Entidades;
using System.Web.Mvc;
using DWM.Models.Persistence;
using App_Dominio.Enumeracoes;
using App_Dominio.Security;
using System.Linq;
using System.Data.Entity.Infrastructure;
using App_Dominio.Models;
using App_Dominio.Pattern;

namespace dwm_condominios.Models.BI
{
    public class HomeBI : DWMContext<ApplicationContext>, IProcess<HomeViewModel, ApplicationContext>
    {
        #region Constructor
        public HomeBI() { }

        public HomeBI(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            Create(_db, _seguranca_db);
        }
        #endregion

        public IEnumerable<HomeViewModel> List(params object[] param)
        {
            throw new NotImplementedException();
        }

        public IPagedList PagedList(int? index, int pageSize = 50, params object[] param)
        {
            throw new NotImplementedException();
        }

        public HomeViewModel Run(Repository value)
        {
            HomeViewModel home = (HomeViewModel) value;
            
            try
            {
                #region Informativo
                ListViewInformativo listViewInformativo = new ListViewInformativo(this.db, this.seguranca_db);

                string data1 = "01" + DateTime.Today.AddMonths(-1).ToString("/MM/yyyy");
                string data2 = Convert.ToDateTime(DateTime.Today.AddMonths(1).ToString("yyyy-MM-") + "01").AddDays(-1).ToString("dd/MM/yyyy");

                home.Informativos = listViewInformativo.Bind(0, 4, Funcoes.StringToDate(data1).Value, Funcoes.StringToDate(data2).Value);
                #endregion
            }
            catch (Exception ex)
            {
                home.mensagem = new Validate() { Code = 999, MessageBase = ex.Message, Message = "Ocorreu um erro na recuperação dos dados" };
            }

            return home;
        }


    }
}