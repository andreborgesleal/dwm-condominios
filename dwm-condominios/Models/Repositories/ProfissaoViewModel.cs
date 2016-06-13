using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App_Dominio.Component;

namespace DWM.Models.Repositories
{
    public class ProfissaoViewModel : Repository
    {
        [DisplayName("ID")]
        public int ProfissaoID { get; set; }

        [DisplayName("Descricao")]
        [Required(ErrorMessage = "Descrição deve ser informada")]
        [StringLength(30, ErrorMessage = "Descrição deve ter no máximo 30 caracteres")]
        public string Nome { get; set; }
    }
}