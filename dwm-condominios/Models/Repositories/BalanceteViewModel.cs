using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using App_Dominio.Component;

namespace DWM.Models.Repositories
{
    public class BalanceteViewModel : Repository
    {
        [DisplayName("Condomínio ID")]
        public int CondominioID { get; set; }

        [DisplayName("Conta Contábil")]
        public int planoContaID { get; set; }

        [DisplayName("Descrição")]
        [Required(ErrorMessage = "Descrição da conta deve ser informada")]
        [StringLength(60, ErrorMessage = "Descrição da conta deve ter no máximo 60 caracteres")]
        public string descricao { get; set; }

        [DisplayName("Natureza")]
        [Required(ErrorMessage = "Natureza da conta deve ser informada")]
        public string Natureza { get; set; }

        public SaldoContabilViewModel SaldoContabil { get; set; }

        public IEnumerable<SaldoContabilViewModel> SaldosContabeis { get; set; }
    }
}