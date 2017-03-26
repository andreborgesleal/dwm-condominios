using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("Credenciado")]
    public class Credenciado
    {
        [Key]
        [DisplayName("ID")]
        public int CredenciadoID { get; set; }

        [DisplayName("CondominoID")]
        public int CondominoID { get; set; }

        [StringLength(30, ErrorMessage = "Este campo só permite até 30 caracteres")]
        [DisplayName("Nome")]
        public string Nome { get; set; }

        [StringLength(100, ErrorMessage = "Este campo só permite até 100 caracteres")]
        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("TipoCredenciadoID")]
        public int TipoCredenciadoID { get; set; }

        [StringLength(1, ErrorMessage = "Este campo só permite até 1 caracteres")]
        [DisplayName("Sexo")]
        public string Sexo { get; set; }

        [DisplayName("Observação")]
        public string Observacao { get; set; }

        [DisplayName("UsuarioID")]
        public int? UsuarioID { get; set; }

        [DisplayName("VisitantePermanente")]
        public string IndVisitantePermanente { get; set; }
    }
}