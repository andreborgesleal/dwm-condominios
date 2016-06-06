using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("Parametro")]
    public class Parametro
    {
        [Key]
        [DisplayName("ID")]
        public int ParamID { get; set; }

        [DisplayName("Condomínio ID")]
        public int CondominioID { get; set; }

        [DisplayName("Nome")]
        public string Nome { get; set; }

        [DisplayName("Descrição")]
        public string Descricao { get; set; }

        [DisplayName("Tipo")]
        public string Tipo { get; set; }

        [DisplayName("Valor")]
        public string Valor { get; set; }
    }
}