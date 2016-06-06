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

        [DisplayName("Tipo Edificacao ID")]
        public int TipoEdificacao { get; set; }

        [DisplayName("Condomínio ID")]
        public int CondominioID { get; set; }

        [DisplayName("Descrição")]
        public string Descricao { get; set; }

        [DisplayName("Código")]
        public string Codigo { get; set; }
    }
}