using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("Unidade")]
    public class Unidade
    {
        [Key, Column(Order = 0)]
        [DisplayName("CondominioID")]
        public int CondominioID { get; set; }

        [Key, Column(Order = 1)]
        [DisplayName("EdificacaoID")]
        public int EdificacaoID { get; set; }

        [Key, Column(Order = 2)]
        [DisplayName("ID")]
        public int UnidadeID { get; set; }

        [DisplayName("Validador")]
        public string Validador { get; set; }

        [DisplayName("DataExpiracao")]
        public System.DateTime? DataExpiracao { get; set; }

        [DisplayName("NomeCondomino")]
        public string NomeCondomino { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }
    }
}