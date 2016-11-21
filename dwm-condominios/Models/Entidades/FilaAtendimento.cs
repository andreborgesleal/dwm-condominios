using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("FilaAtendimento")]
    public class FilaAtendimento
    {
        [Key]
        [DisplayName("ID")]
        public int FilaAtendimentoID { get; set; }

        [DisplayName("CondominioID")]
        public int CondominioID { get; set; }

        [DisplayName("Descricao")]
        public string Descricao { get; set; }

        [DisplayName("Visibilidade")]
        public string VisibilidadeCondomino { get; set; }
    }
}