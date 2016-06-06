using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("Cidade")]
    public class Cidade
    {
        [Key]
        [DisplayName("ID")]
        public int CidadeID { get; set; }

        [DisplayName("Nome")]
        public string Nome { get; set; }

        [DisplayName("UF")]
        public string UF { get; set; }
    }
}