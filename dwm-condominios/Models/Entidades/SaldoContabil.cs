using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("SaldoContabil")]
    public class SaldoContabil
    {
        [Key, Column(Order = 0)]
        [DisplayName("CondominioID")]
        public int CondominioID { get; set; }

        [Key, Column(Order = 1)]
        [DisplayName("PlanoConaID")]
        public int planoContaID { get; set; }

        [Key, Column(Order = 2)]
        [DisplayName("Competencia")]
        public decimal Competencia { get; set; }

        [DisplayName("Saldo")]
        public decimal ValorSaldo { get; set; }
    }
}