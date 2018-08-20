using App_Dominio.Component;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Repositories
{
    public class Calendar
    {
        public string title { get; set; }
        public DateTime start { get; set; }
        public DateTime? end { get; set; }
        public bool allDay { get; set; }
    }

    public class AluguelEspacoViewModel : Repository
    {
        [DisplayName("AluguelID")]
        public int AluguelID { get; set; }

        [DisplayName("EspacoID")]
        [Required(ErrorMessage = "Espaço reservado deve ser informado")]
        public int EspacoID { get; set; }

        [DisplayName("CondominioID")]
        public int CondominioID { get; set; }

        [DisplayName("EdificacaoID")]
        [Required(ErrorMessage = "Edificação deve ser informada")]
        public int EdificacaoID { get; set; }

        [DisplayName("UnidadeID")]
        [Required(ErrorMessage = "Unidade deve ser informada")]
        public int UnidadeID { get; set; }

        [DisplayName("CondominoID")]
        [Required(ErrorMessage = "Condômino deve ser informado")]
        public int CondominoID { get; set; }

        [DisplayName("CredenciadoID")]
        public System.Nullable<int> CredenciadoID { get; set; }

        [DisplayName("DataEvento")]
        [Required(ErrorMessage = "Data do Evento deve ser informada")]
        public DateTime DataEvento { get; set; }

        [DisplayName("DataReserva")]
        public DateTime DataReserva { get; set; }

        [DisplayName("DataAutorizacao")]
        public System.Nullable<DateTime> DataAutorizacao { get; set; }

        [DisplayName("Data Revogação")]
        public System.Nullable<DateTime> DataRevogacao { get; set; }

        [DisplayName("Data Cancelamento")]
        public System.Nullable<DateTime> DataCancelamento { get; set; }

        [DisplayName("Valor")]
        [Required(ErrorMessage = "Valor do aluguel deve ser informado")]
        public decimal Valor { get; set; }

        [DisplayName("Observacao")]
        public string Observacao { get; set; }

        #region Atributos fora da classe
        public int LimitePessoas { get; set; }
        public string DescricaoEspaco { get; set; }

        public string DescricaoEdificacao { get; set; }

        public string NomeCondomino { get; set; }

        public string NomeCredenciado { get; set; }

        public string Situacao { get; set; }

        public string Status
        {
            get
            {
                if (DataAutorizacao == null && !DataCancelamento.HasValue && !DataRevogacao.HasValue)
                {
                    return "Reservado";
                }
                else if (DataAutorizacao.HasValue && !DataCancelamento.HasValue && !DataRevogacao.HasValue)
                {
                    return "Confirmado";
                }
                else if (DataCancelamento.HasValue)
                {
                    return "Cancelado";
                }
                else if (DataRevogacao.HasValue)
                {
                    return "Revogado";
                }
                else
                    return "";
            }
        }

        public Calendar calendar { get; set; }
        #endregion
    }
}