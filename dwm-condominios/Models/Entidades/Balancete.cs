using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("Balancete")]
    public class Balancete
    {
        [Key, Column(Order = 0)]
        [DisplayName("CondominioID")]
        public int CondominioID { get; set; }

        [Key, Column(Order = 1)]
        [DisplayName("PlanoConaID")]
        public int planoContaID { get; set; }

        [DisplayName("Descricao")]
        public string descricao { get; set; }

        [DisplayName("Natureza")]
        public string Natureza { get; set; }
    }
}