using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App_Dominio.Component;

namespace DWM.Models.Repositories
{
    public class ParametroViewModel : Repository
    {
        [DisplayName("ID")]
        public int ParamID { get; set; }

        [DisplayName("Condomínio ID")]
        [Required(ErrorMessage = "Informe o Condomínio ID")]
        public int CondominioID { get; set; }

        [DisplayName("Nome")]
        [Required(ErrorMessage = "Informe o Nome")]
        [StringLength(30, ErrorMessage = "Nome deve ter no máximo 30 caracteres")]
        public string Nome { get; set; }

        [DisplayName("Descrição")]
        [StringLength(100, ErrorMessage = "Descrição deve ter no máximo 100 caracteres")]
        public string Descricao { get; set; }

        [DisplayName("Tipo")]
        [Required(ErrorMessage = "Informe o Tipo")]
        [StringLength(1, ErrorMessage = "Tipo deve ter no máximo 1 caractere")]
        public string Tipo { get; set; }

        [DisplayName("Valor")]
        [Required(ErrorMessage = "Informe o Valor")]
        [StringLength(100, ErrorMessage = "Valor deve ter no máximo 100 caracteres")]
        public string Valor { get; set; }
    }
}