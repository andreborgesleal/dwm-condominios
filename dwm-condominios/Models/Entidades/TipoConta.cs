using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("TipoConta")]
    public class TipoConta
    {
        [Key]
        [DisplayName("ID")]
        public int TipoContaID { get; set; }

        [DisplayName("Descricao")]
        public string Descricao { get; set; }
    }
}