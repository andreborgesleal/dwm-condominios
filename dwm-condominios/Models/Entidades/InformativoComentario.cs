using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("InformativoComentario")]
    public class InformativoComentario
    {
        [Key, Column(Order = 0)]
        [DisplayName("ID")]
        public int InformativoID { get; set; }

        [Key, Column(Order = 1)]
        [DisplayName("DataComentario")]
        public System.DateTime DataComentario { get; set; }

        [DisplayName("CondominoID")]
        public int CondominoID { get; set; }

        [DisplayName("Descricao")]
        public string Descricao { get; set; }

        [DisplayName("DataDesativacao")]
        public System.DateTime? DataDesativacao { get; set; }

        [DisplayName("Motivo")]
        public string Motivo { get; set; }
    }
}