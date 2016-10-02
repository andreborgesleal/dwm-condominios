using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("EmailTemplate")]
    public class EmailTemplate
    {
        [Key]
        [DisplayName("ID")]
        public int EmailTemplateID { get; set; }

        [DisplayName("EmailTipoID")]
        public int EmailTipoID { get; set; }

        [DisplayName("CondominioID")]
        public int CondominioID { get; set; }

        [DisplayName("Nome")]
        public string Nome { get; set; }

        [DisplayName("EmailMensagem")]
        public string EmailMensagem { get; set; }
    }
}