using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("Profissao")]
    public class Profissao
    {
        [Key]
        [DisplayName("ID")]
        public int ProfissaoID { get; set; }

        [DisplayName("Descricao")]
        public string Descricao { get; set; }
    }
}