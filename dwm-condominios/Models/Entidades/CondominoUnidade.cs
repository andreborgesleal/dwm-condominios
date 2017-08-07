using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("CondominoUnidade")]
    public class CondominoUnidade
    {
        public CondominoUnidade()
        {
            AluguelEspacos = new List<AluguelEspaco>();
        }

        [Key, Column(Order = 0)]
        [DisplayName("CondominioID")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CondominioID { get; set; }

        [Key, Column(Order = 1)]
        [DisplayName("EdificacaoID")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EdificacaoID { get; set; }

        [Key, Column(Order = 2)]
        [DisplayName("UnidadeID")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UnidadeID { get; set; }

        [Key, Column(Order = 3)]
        [DisplayName("CondominoID")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CondominoID { get; set; }

        [DisplayName("DataInicio")]
        public System.DateTime DataInicio { get; set; }

        [DisplayName("DataFim")]
        public System.DateTime? DataFim { get; set; }

        public virtual Condomino Condomino { get; set; }

        public virtual ICollection<AluguelEspaco> AluguelEspacos { get; set; }
    }
}