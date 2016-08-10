using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App_Dominio.Component;

namespace DWM.Models.Repositories
{
    public abstract class CondominoViewModel : Repository
    {
        [DisplayName("ID")]
        public int CondominoID { get; set; }

        [DisplayName("Condomínio ID")]
        [Required(ErrorMessage = "Informe o Condomínio")]
        public int CondominioID { get; set; }
        
        [DisplayName("Nome")]
        [StringLength(60, ErrorMessage = "Nome do condômino deve ter no mínimo 10 e no máximo 60 caracteres", MinimumLength = 10)]
        [Required(ErrorMessage = "Informe o Nome do Condômino")]
        public string Nome { get; set; }
        
        [DisplayName("CPF")]
        [Required(ErrorMessage = "Informe o CPF")]
        public string IndFiscal { get; set; }
        
        [DisplayName("Proprietário")]
        [Required(ErrorMessage = "Informe o Indicador Proprietário")]
        public string IndProprietario { get; set; }
        
        [DisplayName("Telefone 1")]
        [Required(ErrorMessage = "Telefone deve ser informado")]
        public string TelParticular1 { get; set; }
        
        [DisplayName("Telefone 2")]
        public string TelParticular2 { get; set; }
        
        [DisplayName("E-mail")]
        [StringLength(100, ErrorMessage = "E-mail deve ter no máximo 100 caracteres")]
        [Required(ErrorMessage = "E-mail deve ser informado")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Informe um e-mail válido")]
        [EmailAddress(ErrorMessage = "Informe o E-mail com um formato válido")]
        public string Email { get; set; }

        [DisplayName("Usuário ID")]
        public int? UsuarioID { get; set; }

        [DisplayName("Observação")]
        public string Observacao { get; set; }

        [DisplayName("Data Cadastro")]
        public System.DateTime DataCadastro { get; set; }
        
        [DisplayName("Avatar")]
        [StringLength(100, ErrorMessage = "Este campo só permite até 100 caracteres")]
        public string Avatar { get; set; }

        [DisplayName("Situação")]
        public string IndSituacao { get; set; }
    }
}