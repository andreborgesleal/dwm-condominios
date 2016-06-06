using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App_Dominio.Component;

namespace DWM.Models.Repositories
{
    public class CondominoPJViewModel : CondominoViewModel
    {
        [DisplayName("Ramo de Atividade ID")]
        public int RamoAtividadeID { get; set; }

        [DisplayName("Nome Fantasia")]
        [StringLength(40, ErrorMessage = "Este campo só permite até 40 caracteres")]
        public string NomeFantasia { get; set; }

        [DisplayName("Administrador")]
        [StringLength(40, ErrorMessage = "Este campo só permite até 40 caracteres")]
        public string Administrador { get; set; }
    }
}