using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App_Dominio.Component;

namespace dwm_condominios.Models.Repositories
{
    public class UnidadeViewModel : Repository
    {
        [DisplayName("ID")]
        public int UnidadeID { get; set; }

        [DisplayName("Edificação ID")]
        [Required(ErrorMessage = "Informe a Edificação ID")]
        public int EdificacaoID { get; set; }

        [DisplayName("Condomínio ID")]
        [Required(ErrorMessage = "Informe o Condomínio ID")]
        public int CondominioID { get; set; }

        [DisplayName("Validador")]
        [StringLength(50, ErrorMessage = "Validador deve ter no máximo 50 caracteres")]
        public string Validador { get; set; }
    }
}