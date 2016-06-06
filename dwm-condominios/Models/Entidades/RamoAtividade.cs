using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("RamoAtividade")]
    public class RamoAtividade
    {
        [Key]
        [DisplayName("ID")]
        public int RamoAtividadeID { get; set; }

        [DisplayName("Condomínio ID")]
        public int CondominioID { get; set; }

        [DisplayName("Descrição")]
        public string Descricao { get; set; }
    }
}