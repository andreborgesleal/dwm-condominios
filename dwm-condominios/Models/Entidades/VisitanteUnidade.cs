using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("VisitanteUnidade")]
    public class VisitanteUnidade
    {
        [Key, Column(Order = 0)]
        [DisplayName("VisitanteID")]
        public int VisitanteID { get; set; }

        [Key, Column(Order = 1)]
        [DisplayName("CondominioID")]
        public int CondominioID { get; set; }

        [Key, Column(Order = 2)]
        [DisplayName("EdificacaoID")]
        public int EdificacaoID { get; set; }

        [Key, Column(Order = 3)]
        [DisplayName("UnidadeID")]
        public int UnidadeID { get; set; }

        [Key, Column(Order = 4)]
        [DisplayName("CondominoID")]
        public int CondominoID { get; set; }

        [DisplayName("CredenciadoID")]
        public System.Nullable<int> CredenciadoID { get; set; }

        public virtual Visitante Visitante { get; set; }
    }
}