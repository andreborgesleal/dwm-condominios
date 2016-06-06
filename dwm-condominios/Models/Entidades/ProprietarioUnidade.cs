using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("ProprietarioUnidade")]
    public class ProprietarioUnidade
    {
        [Key]
        [DisplayName("ID")]
        public int ProprietarioID { get; set; }

        [DisplayName("Condominio ID")]
        public int CondominioID { get; set; }

        [DisplayName("Unidade ID")]
        public int UnidadeID { get; set; }

        [DisplayName("Edificação ID")]
        public int EdificacaoID { get; set; }

        [DisplayName("Data Fim")]
        public System.DateTime DataFim { get; set; }
    }
}