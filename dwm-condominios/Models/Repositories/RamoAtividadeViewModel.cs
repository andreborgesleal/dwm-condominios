using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App_Dominio.Component;

namespace DWM.Models.Repositories
{
    public class RamoAtividadeViewModel : Repository
    {
        [DisplayName("ID")]
        public int RamoAtividadeID { get; set; }

        [DisplayName("Condomínio ID")]
        [Required(ErrorMessage = "Informe o Condomínio ID")]
        public int CondominioID { get; set; }

        [DisplayName("Descrição")]
        [Required(ErrorMessage = "Informe a Descrição")]
        [StringLength(50, ErrorMessage = "Descrição deve ter no máximo 50 caracteres")]
        public string Descricao { get; set; }
    }
}