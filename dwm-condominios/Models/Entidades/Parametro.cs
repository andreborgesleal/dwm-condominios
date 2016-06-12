using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("Parametro")]
    public class Parametro
    {
        [Key, Column(Order = 0)]
        [DisplayName("CondominioID")]
        public int CondominioID { get; set; }

        [Key, Column(Order = 1)]
        [DisplayName("ID")]
        public int ParamID { get; set; }

        [DisplayName("Nome")]
        public string Nome { get; set; }

        [DisplayName("Descricao")]
        public string Descricao { get; set; }

        [DisplayName("Tipo")]
        public string Tipo { get; set; }

        [DisplayName("Valor")]
        public string Valor { get; set; }
    }
}