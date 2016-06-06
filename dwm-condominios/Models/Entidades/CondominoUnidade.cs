using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("CondominoUnidade")]
    public class CondominoUnidade
    {
        [DisplayName("Condomínio ID")]
        public int CondominioID { get; set; }

        [DisplayName("Edificação ID")]
        public int EdificacaoID { get; set; }

        [DisplayName("Unidade ID")]
        public int UnidadeID { get; set; }

        [DisplayName("Condomino ID")]
        public int CondominoID { get; set; }

        [DisplayName("Data Início")]
        public System.DateTime DataInicio { get; set; }

        [DisplayName("Data Fim")]
        public System.DateTime DataFim { get; set; }
    }
}