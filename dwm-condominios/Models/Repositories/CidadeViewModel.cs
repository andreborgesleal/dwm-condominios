using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App_Dominio.Component;

namespace DWM.Models.Repositories
{
    public class CidadeViewModel : Repository
    {
        [DisplayName("ID")]
        public int CidadeID { get; set; }

        [DisplayName("Nome")]
        [Required(ErrorMessage ="Nome da cidade deve ser informada")]
        [StringLength(30, ErrorMessage = "Nome da cidade deve ter no máximo 30 caracteres")]
        public string Nome { get; set; }

        [DisplayName("UF")]
        [StringLength(2, ErrorMessage = "UF deve ter no máximo 2 caracteres")]
        public string UF { get; set; }
    }
}