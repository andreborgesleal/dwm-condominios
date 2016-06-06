using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App_Dominio.Component;

namespace DWM.Models.Repositories
{
    public class ContaVirtualViewModel : Repository
    {
        [DisplayName("ID")]
        public int ContaVirtualID { get; set; }

        [DisplayName("ID")]
        public int MembroID { get; set; }

        public string Nome { get; set; }
        public string Email { get; set; }
        public string CPF { get; set; }
        public string Telefone { get; set; }

        [DisplayName("Tipo Conta")]
        public int TipoContaID { get; set; }

        public string Descricao { get; set; }

        [DisplayName("Data de Abertura")]
        public System.DateTime DataAbertura { get; set; }

        [DisplayName("Data de Encerramento")]
        public System.Nullable<System.DateTime> DataEncerramento { get; set; }

        [DisplayName("Saldo")]
        public System.Decimal ValorSaldo { get; set; }

        public MembroViewModel MembroViewModel { get; set; }
    }
}