using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("TipoCredenciado")]
    public class TipoCredenciado
    {
        [Key]
        [DisplayName("ID")]
        public int TipoCredenciadoID { get; set; }

        [DisplayName("CondominioID")]
        public int CondominioID { get; set; }

        [DisplayName("Descricao")]
        public string Descricao { get; set; }
    }
}