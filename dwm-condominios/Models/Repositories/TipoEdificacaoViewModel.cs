using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App_Dominio.Component;

namespace DWM.Models.Repositories
{
    public class TipoEdificacaoViewModel : Repository
    {
        [DisplayName("ID")]
        public int TipoEdificacaoID { get; set; }

        [DisplayName("Descrição")]
        [Required(ErrorMessage = "Informe a Descrição")]
        [StringLength(20, ErrorMessage = "Descrição deve ter no máximo 20 caracteres")]
        public string Descricao { get; set; }
    }
}