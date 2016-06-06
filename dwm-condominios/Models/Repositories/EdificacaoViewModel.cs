using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App_Dominio.Component;

namespace DWM.Models.Repositories
{
    public class EdificacaoViewModel : Repository
    {
        [DisplayName("ID")]
        public int EdificacaoID { get; set; }

        [DisplayName("Tipo Edificação ID")]
        [Required(ErrorMessage = "Informe o Tipo Edifição")]
        public int TipoEdificacao { get; set; }

        [DisplayName("Condomínio ID")]
        [Required(ErrorMessage = "Informe o Condomínio ID")]
        public int CondominioID { get; set; }

        [DisplayName("Descrição")]
        [StringLength(40, ErrorMessage = "Descrição deve ter no máximo 40 caracteres")]
        [Required(ErrorMessage = "Informe a Descrição")]
        public string Descricao { get; set; }

        [DisplayName("Código")]
        [StringLength(10, ErrorMessage = "Código deve ter no máximo 10 caracteres")]
        public string Codigo { get; set; }
    }
}