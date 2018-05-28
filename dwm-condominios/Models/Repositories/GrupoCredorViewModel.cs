using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App_Dominio.Component;

namespace DWM.Models.Repositories
{
    public class GrupoCredorViewModel : Repository
    {
        [DisplayName("ID")]
        public int grupoCredorId { get; set; }

        [DisplayName("CondominioID")]
        public int CondominioID { get; set; }

        [DisplayName("Nome")]
        [Required(ErrorMessage = "Nome do grupo deve ser informado")]
        [StringLength(30, ErrorMessage = "Nome do grupo deve ter no máximo 30 caracteres")]
        public string nome { get; set; }
    }
}