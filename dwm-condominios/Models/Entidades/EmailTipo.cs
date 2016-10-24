using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("EmailTipo")]
    public class EmailTipo
    {
        [Key, Column(Order = 0)]
        [DisplayName("ID")]
        public int EmailTipoID { get; set; }

        [Key, Column(Order = 1)]
        [DisplayName("CondominioID")]
        public int CondominioID { get; set; }

        [DisplayName("Descricao")]
        public string Descricao { get; set; }

        [DisplayName("Assunto")]
        public string Assunto { get; set; }

        [DisplayName("IndTipoFixo")]
        public string IndTipoFixo { get; set; }
    }
}