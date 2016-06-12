using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App_Dominio.Component;

namespace DWM.Models.Repositories
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

        [DisplayName("Data de Expiração")]
        public System.DateTime? DataExpiracao { get; set; }

        [DisplayName("Nome do Condômino")]
        [StringLength(60, ErrorMessage = "Nome deve ter no máximo 60 caracteres")]
        public string NomeCondomino { get; set; }

        [DisplayName("Email")]
        [StringLength(100, ErrorMessage = "E-mail deve ter no máximo 100 caracteres")]
        [Required(ErrorMessage = "E-mail deve ser informado")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Informe um e-mail válido")]
        [EmailAddress(ErrorMessage = "Informe o E-mail com um formato válido")]
        public string Email { get; set; }

        public string EdificacaoDescricao { get; set; }
    }
}