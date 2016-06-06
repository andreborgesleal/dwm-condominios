using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App_Dominio.Component;

namespace DWM.Models.Repositories
{
    public class MembroViewModel : Repository
    {
        [DisplayName("ID")]
        public int MembroID { get; set; }

        [DisplayName("Nome*")]
        [Required(ErrorMessage = "Nome do apostador deve ser informado")]
        [StringLength(30, ErrorMessage = "Nome do apostador deve ter no mínimo 10 e no máximo 30 caracteres", MinimumLength = 10)]
        public string Nome { get; set; }

        [DisplayName("E-Mail*")]
        [StringLength(100, ErrorMessage = "E-mail deve ter no máximo 100 caracteres")]
        [Required(ErrorMessage = "E-mail deve ser informado")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Informe um e-mail válido")]
        [EmailAddress(ErrorMessage = "Informe o E-mail com um formato válido")]
        public string Email { get; set; }

        [DisplayName("Telefone*")]
        [Required(ErrorMessage = "Telefone deve ser informado")]
        public string Telefone { get; set; }

        [DisplayName("CPF*")]
        [Required(ErrorMessage = "CPF deve ser informado")]
        public string CPF { get; set; }

        [DisplayName("Banco")]
        public System.Nullable<int> Banco { get; set; }

        [DisplayName("Agencia")]
        public string Agencia { get; set; }

        [DisplayName("Conta")]
        public string Conta { get; set; }

        [DisplayName("Situacao")]
        public string IndSituacao { get; set; }

        [DisplayName("Avatar")]
        public string Avatar { get; set; }
    }
}