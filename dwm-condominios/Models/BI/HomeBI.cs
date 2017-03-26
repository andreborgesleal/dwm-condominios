using System;
using System.Collections.Generic;
using App_Dominio.Contratos;
using App_Dominio.Entidades;
using App_Dominio.Component;
using DWM.Models.Repositories;
using DWM.Models.Entidades;
using System.Web.Mvc;
using DWM.Models.Persistence;
using App_Dominio.Models;
using System.Linq;

namespace dwm_condominios.Models.BI
{
    public class HomeBI : DWMContextLocal, IProcess<HomeViewModel, ApplicationContext>
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
                home.ContabilidadeCompetencia = "Janeiro/2017";

                home.ValorInadimplenciaTotal = (decimal)468874.54;
                home.ValorInadimplenciaCompetencia = (decimal)28274.29;
                home.ValorSaldoAnterior = (decimal)18740.36;
                home.ValorSaldoAtual = (decimal)12230.57;
                home.ValorReceitaCompetenciaRealizada = (decimal)181569.97;
                home.ValorReceitaCompetenciaPlanejada = (decimal)205409.69;
                home.ValorDespesaCompetenciaRealizada = (decimal)188079.76;

                home.ValorInadimplenciaTotal = Math.Round(home.ValorInadimplenciaTotal/1000, 0);
                home.ValorInadimplenciaCompetencia = Math.Round(home.ValorInadimplenciaCompetencia / 1000, 0);
                home.ValorSaldoAnterior = Math.Round(home.ValorSaldoAnterior / 1000, 0);
                home.ValorReceitaCompetenciaRealizada = Math.Round(home.ValorReceitaCompetenciaRealizada / 1000, 0);
                home.ValorReceitaCompetenciaPlanejada = Math.Round(home.ValorReceitaCompetenciaPlanejada / 1000, 0);
                home.ValorDespesaCompetenciaRealizada = Math.Round(home.ValorDespesaCompetenciaRealizada / 1000, 0);

                home.ValorSaldoAtual = home.ValorSaldoAnterior + home.ValorReceitaCompetenciaRealizada - home.ValorDespesaCompetenciaRealizada;

                home.TotalUnidadesCadastradas = (from cu in db.CondominoUnidades
                                                 where cu.CondominioID == sessaoCorrente.empresaId
                                                        && cu.DataFim == null
                                                 select cu).Count();
                home.TotalCondominos = (from cu in db.CondominoUnidades
                                        join cre in db.Credenciados on cu.CondominoID equals cre.CondominoID
                                        where cu.CondominioID == sessaoCorrente.empresaId
                                                && cu.DataFim == null
                                                && cre.IndVisitantePermanente != "S"
                                        select cre).Count() + home.TotalUnidadesCadastradas; // total de credenciados diferentes de visitantes permanentes + total de titulares

                #region Informativo
                ListViewInformativo listViewInformativo = new ListViewInformativo(this.db, this.seguranca_db);

                string data1 = "01" + DateTime.Today.AddMonths(-1).ToString("/MM/yyyy");
                string data2 = Funcoes.Brasilia().Date.ToString("dd/MM/yyyy");

                home.Informativos = listViewInformativo.Bind(0, 6, Funcoes.StringToDate(data1).Value, Funcoes.StringToDate(data2).Value, SessaoLocal.GrupoCondominoID, SessaoLocal.Unidades);
                #endregion

                #region Documentos p/ download
                DateTime _data1 = Funcoes.Brasilia().AddMonths(-3) ;
                DateTime _data2 = Funcoes.Brasilia().Date.AddDays(1).AddMinutes(-1);
                int _EdificacaoID = 0;
                int _UnidadeID = 0;
                int _CondominoID = 0;
                int _GrupoCondominoID = 0;

                ListViewArquivoHome l = new ListViewArquivoHome(this.db, this.seguranca_db);
                if (SessaoLocal.CondominoID == 0)
                    home.Documentos = l.getPagedList(0, 12, _data1, _data2, _EdificacaoID, _UnidadeID, _CondominoID, _GrupoCondominoID, "");
                else
                    home.Documentos = l.getPagedList(0, 12, _data1, _data2, _EdificacaoID, _UnidadeID, SessaoLocal.CondominoID, _GrupoCondominoID, "");
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