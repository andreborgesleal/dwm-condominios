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

        [DisplayName("CondominioID")]
        public int CondominioID { get; set; }

        [DisplayName("Nome")]
        public string Nome { get; set; }

        [DisplayName("IndicadorFiscal")]
        public string IndFiscal { get; set; }

        [DisplayName("IndicadorProprietario")]
        public string IndProprietario { get; set; }

        [DisplayName("Telefone1")]
        public string TelParticular1 { get; set; }

        [DisplayName("Telefone2")]
        public string TelParticular2 { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("UsuarioID")]
        public int? UsuarioID { get; set; }

        [DisplayName("Observacao")]
        public string Observacao { get; set; }

        [DisplayName("DataCadastro")]
        public System.DateTime DataCadastro { get; set; }

        [DisplayName("Avatar")]
        public string Avatar { get; set; }

        [DisplayName("IndicadorSituacao")]
        public string IndSituacao { get; set; }

        public string discriminator { get; set; }
    }
}