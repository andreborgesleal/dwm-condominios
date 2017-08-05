using App_Dominio.Component;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DWM.Models.Repositories
{
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

        [DisplayName("Valor")]
        [Required(ErrorMessage = "Valor do aluguel deve ser informado")]
        public decimal Valor { get; set; }

        [DisplayName("Observacao")]
        public string Observacao { get; set; }

        #region Atributos fora da classe
        public int LimitePessoas { get; set; }
        public string DescricaoEspaco { get; set; }
        #endregion
    }
}