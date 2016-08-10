using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App_Dominio.Component;

namespace DWM.Models.Repositories
{
    public class CredenciadoViewModel : Repository
    {
        [DisplayName("ID")]
        public int CredenciadoID { get; set; }

        [DisplayName("Condomino ID")]
        [Required(ErrorMessage = "Inform o Condômino ID")]
        public int CondominoID { get; set; }

        [DisplayName("Nome")]
        [StringLength(30, ErrorMessage = "Este campo só permite até 30 caracteres")]
        [Required(ErrorMessage = "Inform o Nome")]
        public string Nome { get; set; }

        [DisplayName("E-mail")]
        [StringLength(100, ErrorMessage = "E-mail deve ter no máximo 100 caracteres")]
        [Required(ErrorMessage = "E-mail deve ser informado")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Informe um e-mail válido")]
        [EmailAddress(ErrorMessage = "Informe o E-mail com um formato válido")]
        public string Email { get; set; }
        
        [DisplayName("Sexo")]
        [Required(ErrorMessage = "Inform o Sexo")]
        [StringLength(1, ErrorMessage = "Este campo só permite até 1 caracteres")]
        public string Sexo { get; set; }

        [DisplayName("Observação")]
        public string Observacao { get; set; }

        [DisplayName("Usuario ID")]
        public int UsuarioID { get; set; }
    }
}