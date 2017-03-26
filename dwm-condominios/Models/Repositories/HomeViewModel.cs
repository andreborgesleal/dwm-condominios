using App_Dominio.Component;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace DWM.Models.Repositories
{
    public class HomeViewModel : Repository
    {
        public int TotalUnidadesCadastradas { get; set; }
        public int TotalUnidadesInformadas { get; set; }
        public int TotalCondominos { get; set; }
        public string ContabilidadeCompetencia { get; set; } // Ex: "Janeiro/2017"
        public decimal ValorInadimplenciaTotal { get; set; } 
        public decimal ValorInadimplenciaCompetencia { get; set; } // Valor da inadimplência dentro da competência
        public decimal ValorReceitaCompetenciaRealizada { get; set; } // Valor da receita realizada dentro da competência
        public decimal ValorReceitaCompetenciaPlanejada { get; set; } // Valor da receita planejada dentro da competência
        public decimal ValorDespesaCompetenciaRealizada { get; set; } // Valor da despesa realizada dentro da competência
        public decimal ValorDespesaCompetenciaPercentual { get; set; } // % do Valor da despesa realizada dentro da competência sobre a receita realizada na competência
        public decimal ValorSaldoAnterior { get; set; } // Saldo (receitas-despesas realizadas desde o início do exercício até o mês anterior da competência)
        public decimal ValorSaldoCompetencia { get; set; } // Valor da receita realizada dentro da competência - valor da despesa realizada dentro da competência
        public decimal ValorSaldoAtual { get; set; } // Saldo anterior + Saldo na competência
        public string isAdmin { get; set; }
        public IEnumerable<InformativoViewModel> Informativos { get; set; }
        public IPagedList Documentos { get; set; }


        //public IEnumerable<PanoramaViewModel> Panorama { get; set; }
        //public IEnumerable<MovimentoViewModel> Movimento { get; set; }
        //public IEnumerable<ConcursoViewModel> Concurso { get; set; }
        //public IEnumerable<VolanteViewModel> Volante { get; set; }
        //public ApostaViewModel apostaViewModel { get; set; }
        //public IEnumerable<MovimentoViewModel> Extrato { get; set; }
        //public string NumerosApostados { get; set; }
        //public IEnumerable<MensalidadeViewModel> Mensalidades { get; set; }
    }
}