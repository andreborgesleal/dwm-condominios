using System.ComponentModel;
using App_Dominio.Component;

namespace DWM.Models.Repositories
{
    public class SaldoContabilViewModel : Repository
    {
        [DisplayName("Condomínio ID")]
        public int CondominioID { get; set; }

        [DisplayName("Conta Contábil ID")]
        public int planoContaID { get; set; }

        [DisplayName("Competencia")]
        public decimal Competencia { get; set; }

        [DisplayName("Saldo")]
        public decimal ValorSaldo { get; set; }
    }
}