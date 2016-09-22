using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("GrupoCondomino")]
    public class GrupoCondomino
    {
        [Key]
        [DisplayName("ID")]
        public int GrupoCondominoID { get; set; }

        [DisplayName("CondominioID")]
        public int CondominioID { get; set; }

        [DisplayName("Descricao")]
        public string Descricao { get; set; }

        [DisplayName("Objetivo")]
        public string Objetivo { get; set; }
    }
}