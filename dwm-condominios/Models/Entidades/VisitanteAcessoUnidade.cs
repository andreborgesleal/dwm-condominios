using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("VisitanteAcessoUnidade")]
    public class VisitanteAcessoUnidade
    {
        [Key, ForeignKey("VisitanteAcesso")]
        [DisplayName("AcessoID")]
        public int AcessoID { get; set; }

        [DisplayName("CondominioID")]
        public int CondominioID { get; set; }

        [DisplayName("EdificacaoID")]
        public int EdificacaoID { get; set; }

        [DisplayName("UnidadeID")]
        public int UnidadeID { get; set; }

        [DisplayName("CondominoID")]
        public int CondominoID { get; set; }

        [DisplayName("CredenciadoID")]
        public System.Nullable<int> CredenciadoID { get; set; }

        public virtual VisitanteAcesso VisitanteAcesso { get; set; }
    }
}