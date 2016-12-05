using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("ChamadoMotivo")]
    public class ChamadoMotivo
    {
        [Key, Column(Order = 0)]
        [DisplayName("ID")]
        public int ChamadoMotivoID { get; set; }

        [Key, Column(Order = 1)]
        [DisplayName("CondominioID")]
        public int CondominioID { get; set; }

        [DisplayName("FilaAtendimentoID")]
        public int FilaAtendimentoID { get; set; }

        [DisplayName("Descricao")]
        public string Descricao { get; set; }

        [DisplayName("IndFixo")]
        public string IndFixo { get; set; }
    }
}