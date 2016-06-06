using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("Condomino")]
    public abstract class Condomino
    {
        [Key]
        [DisplayName("ID")]
        public int CondominoID { get; set; }

        [DisplayName("Condomínio ID")]
        public int CondominioID { get; set; }

        [StringLength(60, ErrorMessage = "Este campo só permite até 60 caracteres")]
        [DisplayName("Nome")]
        public string Nome { get; set; }

        [StringLength(14, ErrorMessage = "Este campo só permite até 14 caracteres")]
        [DisplayName("Indicador Fiscal")]
        public string IndFiscal { get; set; }

        [StringLength(1, ErrorMessage = "Este campo só permite até 1 caractere")]
        [DisplayName("Indicador Proprietário")]
        public string IndProprietario { get; set; }

        [StringLength(11, ErrorMessage = "Este campo só permite até 11 caracteres")]
        [DisplayName("Telefone Particular 1")]
        public string TelParticular1 { get; set; }

        [StringLength(11, ErrorMessage = "Este campo só permite até 11 caracteres")]
        [DisplayName("Telefone Particular 2")]
        public string TelParticular2 { get; set; }

        [StringLength(11, ErrorMessage = "Este campo só permite até 11 caracteres")]
        [DisplayName("Telefone Particular 3")]
        public string TelParticular3 { get; set; }

        [StringLength(11, ErrorMessage = "Este campo só permite até 11 caracteres")]
        [DisplayName("Telefone Particular 4")]
        public string TelParticular4 { get; set; }

        [StringLength(100, ErrorMessage = "Este campo só permite até 100 caracteres")]
        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Usuário ID")]
        public int UsuarioID { get; set; }

        [DisplayName("Observação")]
        public string Observacao { get; set; }

        [DisplayName("Data de Cadastro")]
        public System.DateTime DataCadastro { get; set; }

        [StringLength(100, ErrorMessage = "Este campo só permite até 100 caracteres")]
        [DisplayName("Avatar")]
        public string Avatar { get; set; }
    }
}