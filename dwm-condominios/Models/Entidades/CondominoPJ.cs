using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DWM.Models.Entidades
{
    public class CondominoPJ : Condomino
    {
        [DisplayName("Ramo de Atividade ID")]
        public int RamoAtividadeID { get; set; }

        [StringLength(40, ErrorMessage = "Este campo só permite até 40 caracteres")]
        [DisplayName("Nome Fantasia")]
        public string NomeFantasia { get; set; }

        [StringLength(40, ErrorMessage = "Este campo só permite até 40 caracteres")]
        [DisplayName("Administrador")]
        public string Administrador { get; set; }
    }
}