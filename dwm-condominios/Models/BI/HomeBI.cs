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
                Facade<InformativoViewModel, InformativoModel, ApplicationContext> facadeCob = new Facade<InformativoViewModel, InformativoModel, ApplicationContext>();
                home.Informativo = listViewInformativo.Bind(0, 50, );
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