using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App_Dominio.Component;

namespace DWM.Models.Repositories
{
    public class ChamadoStatusViewModel : Repository
    {
        [DisplayName("ID")]
        public int ChamadoStatusID { get; set; }

        [DisplayName("CondominioID")]
        public int CondominioID { get; set; }

        [DisplayName("Descricao")]
        [Required(ErrorMessage = "Descrição da situação deve ser informada")]
        [StringLength(25, ErrorMessage = "Descrição da situação deve ter no máximo 25 caracteres")]
        public string Descricao { get; set; }

        [DisplayName("IndFixo")]
        public string IndFixo { get; set; }
    }
}