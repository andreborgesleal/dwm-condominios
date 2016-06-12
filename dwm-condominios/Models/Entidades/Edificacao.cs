using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("Edificacao")]
    public class Edificacao
    {
        [Key]
        [DisplayName("ID")]
        public int EdificacaoID { get; set; }

        [DisplayName("TipoEdificacaoID")]
        public int TipoEdificacaoID { get; set; }

        [DisplayName("CondominioID")]
        public int CondominioID { get; set; }

        [DisplayName("Descricao")]
        public string Descricao { get; set; }

        [DisplayName("Codigo")]
        public string Codigo { get; set; }
    }
}