using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App_Dominio.Component;

namespace DWM.Models.Repositories
{
    public class TipoCredenciadoViewModel : Repository
    {
        [DisplayName("ID")]
        public int TipoCredenciadoID { get; set; }

        [DisplayName("Condomínio")]
        public int CondominioID { get; set; }

        [DisplayName("Descricao")]
        [StringLength(20, ErrorMessage = "Descrição deve ter no máximo 20 caracteres")]
        [Required(ErrorMessage = "Informe a Descrição")]
        public string Descricao { get; set; }
    }
}