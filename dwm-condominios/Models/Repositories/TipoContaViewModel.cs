using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App_Dominio.Component;

namespace DWM.Models.Repositories
{
    public class TipoContaViewModel : Repository
    {
        [DisplayName("ID")]
        public int TipoContaID { get; set; }

        [DisplayName("Descrição")]
        [Required(ErrorMessage = "Descrição da conta deve ser informada")]
        [StringLength(30, ErrorMessage = "Descrição da conta deve ter no máximo 20 caracteres")]
        public string Descricao { get; set; }

    }
}