using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("Unidade")]
    public class Unidade
    {
        [Key]
        [DisplayName("ID")]
        public int UnidadeID { get; set; }

        [DisplayName("Edificação ID")]
        public int EdificacaoID { get; set; }

        [DisplayName("Condomínio ID")]
        public int CondominioID { get; set; }

        [DisplayName("Validador")]
        public string Validador { get; set; }
    }
}