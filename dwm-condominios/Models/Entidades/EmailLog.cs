using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("EmailLog")]
    public class EmailLog
    {
        [Key]
        [DisplayName("EmailLogID")]
        public int EmailLogID { get; set; }

        [DisplayName("EmailTipoID")]
        public int EmailTipoID { get; set; }

        [DisplayName("CondominioID")]
        public int CondominioID { get; set; }

        [DisplayName("EdificacaoID")]
        public System.Nullable<int> EdificacaoID { get; set; }

        [DisplayName("UnidadeID")]
        public System.Nullable<int> UnidadeID { get; set; }

        [DisplayName("GrupoCondominoID")]
        public System.Nullable<int> GrupoCondominoID { get; set; }

        [DisplayName("DataEmail")]
        public System.DateTime DataEmail { get; set; }

        [DisplayName("Assunto")]
        public string Assunto { get; set; }

        [DisplayName("EmailMensagem")]
        public string EmailMensagem { get; set; }
    }
}