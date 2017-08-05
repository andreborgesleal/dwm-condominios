using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("AluguelEspaco")]
    public class AluguelEspaco
    {
        [Key]
        [DisplayName("AluguelID")]
        public int AluguelID { get; set; }

        [DisplayName("EspacoID")]
        public int EspacoID { get; set; }

        [DisplayName("CondominioID")]
        public int CondominioID { get; set; }

        [DisplayName("EdificacaoID")]
        public int EdificacaoID { get; set; }

        [DisplayName("UnidadeID")]
        public int UnidadeID { get; set; }

        [DisplayName("CondominoID")]
        public int CondominoID { get; set; }

        [DisplayName("CredenciadoID")]
        public System.Nullable<int> CredenciadoID { get; set; }

        [DisplayName("DataEvento")]
        public DateTime DataEvento { get; set; }

        [DisplayName("DataReserva")]
        public DateTime DataReserva { get; set; }

        [DisplayName("DataAutorizacao")]
        public System.Nullable<DateTime> DataAutorizacao { get; set; }

        [DisplayName("Valor")]
        public decimal Valor { get; set; }

        [DisplayName("Observacao")]
        public string Observacao { get; set; }
    }
}