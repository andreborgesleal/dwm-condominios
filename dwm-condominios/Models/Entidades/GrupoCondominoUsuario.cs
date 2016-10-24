using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("GrupoCondominoUsuario")]
    public class GrupoCondominoUsuario
    {
        [Key, Column(Order = 0)]
        [DisplayName("GrupoCondominoID")]
        public int GrupoCondominoID { get; set; }

        [Key, Column(Order = 1)]
        [DisplayName("CondominoID")]
        public int CondominoID { get; set; }
    }
}