using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("CondominoUnidade")]
    public class CondominoUnidade
    {
        [Key, Column(Order = 0)]
        [DisplayName("CondominioID")]
        public int CondominioID { get; set; }

        [Key, Column(Order = 1)]
        [DisplayName("EdificacaoID")]
        public int EdificacaoID { get; set; }

        [Key, Column(Order = 2)]
        [DisplayName("UnidadeID")]
        public int UnidadeID { get; set; }

        [Key, Column(Order = 3)]
        [DisplayName("CondominoID")]
        public int CondominoID { get; set; }

        [DisplayName("DataInicio")]
        public System.DateTime DataInicio { get; set; }

        [DisplayName("DataFim")]
        public System.DateTime? DataFim { get; set; }
    }
}