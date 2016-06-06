using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App_Dominio.Component;

namespace DWM.Models.Repositories
{
    public class CondominoViewModel : Repository
    {
        [DisplayName("ID")]
        public int CondominoID { get; set; }

        [DisplayName("Condomínio ID")]
        [Required(ErrorMessage = "Informe o Condomínio ID")]
        public int CondominioID { get; set; }
        
        [DisplayName("Nome")]
        [StringLength(60, ErrorMessage = "Este campo só permite até 60 caracteres")]
        [Required(ErrorMessage = "Informe o Nome")]
        public string Nome { get; set; }
        
        [DisplayName("Indicador Fiscal")]
        [StringLength(14, ErrorMessage = "Este campo só permite até 14 caracteres")]
        [Required(ErrorMessage = "Informe o Indicador")]
        public string IndFiscal { get; set; }
        
        [DisplayName("Indicador Proprietário")]
        [StringLength(1, ErrorMessage = "Este campo só permite até 1 caractere")]
        [Required(ErrorMessage = "Informe o Indicador Proprietário")]
        public string IndProprietario { get; set; }
        
        [DisplayName("Telefone Particular 1")]
        [StringLength(11, ErrorMessage = "Este campo só permite até 11 caracteres")]
        public string TelParticular1 { get; set; }
        
        [DisplayName("Telefone Particular 2")]
        [StringLength(11, ErrorMessage = "Este campo só permite até 11 caracteres")]
        public string TelParticular2 { get; set; }
        
        [DisplayName("Telefone Particular 3")]
        [StringLength(11, ErrorMessage = "Este campo só permite até 11 caracteres")]
        public string TelParticular3 { get; set; }
        
        [DisplayName("Telefone Particular 4")]
        [StringLength(11, ErrorMessage = "Este campo só permite até 11 caracteres")]
        public string TelParticular4 { get; set; }
        
        [DisplayName("Email")]
        [StringLength(100, ErrorMessage = "Este campo só permite até 100 caracteres")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Informe um Email válido")]
        [Required(ErrorMessage = "Informe o Email")]
        public string Email { get; set; }

        [DisplayName("Usuário ID")]
        public int UsuarioID { get; set; }

        [DisplayName("Observação")]
        public string Observacao { get; set; }

        [DisplayName("Data de Cadastro")]
        public System.DateTime DataCadastro { get; set; }
        
        [DisplayName("Avatar")]
        [StringLength(100, ErrorMessage = "Este campo só permite até 100 caracteres")]
        public string Avatar { get; set; }
    }
}