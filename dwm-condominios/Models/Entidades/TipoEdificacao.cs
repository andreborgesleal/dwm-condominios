using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("TipoEdificacao")]
    public class TipoEdificacao
    {
        [Key]
        [DisplayName("ID")]
        public int TipoEdificacaoID { get; set; }

        [DisplayName("CondominioID")]
        public int CondominioID { get; set; }

        [DisplayName("Descrição")]
        public string Descricao { get; set; }
    }
}