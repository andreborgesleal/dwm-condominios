using App_Dominio.Component;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DWM.Models.Repositories
{
    public class CredorTipoServicoViewModel : Repository
    {
        [Key]
        [DisplayName("ID")]
        public int TipoServicoID { get; set; }

        [DisplayName("CondominioID")]
        public int CondominioID { get; set; }

        [DisplayName("Descricao")]
        [StringLength(50)]
        [Required]
        public string Descricao { get; set; }

        [DisplayName("Situacao")]
        [StringLength(1)]
        public string Situacao { get; set; }
    }
}