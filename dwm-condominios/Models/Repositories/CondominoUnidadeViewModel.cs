using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App_Dominio.Component;

namespace DWM.Models.Repositories
{
    public class CondominoUnidadeViewModel : Repository
    {
        [DisplayName("Condomínio ID")]
        [Required(ErrorMessage = "Informe o CondominioID")]
        public int CondominioID { get; set; }

        [DisplayName("Edificação ID")]
        [Required(ErrorMessage = "Informe a EdificacaoID")]
        public int EdificacaoID { get; set; }

        [DisplayName("Unidade ID")]
        [Required(ErrorMessage = "Informe a UnidadeID")]
        public int UnidadeID { get; set; }

        public string Codigo { get; set; }

        [DisplayName("Condomino ID")]
        public int CondominoID { get; set; }

        [DisplayName("Data de Início")]
        [Required(ErrorMessage = "Informe a Data de Início")]
        public System.DateTime DataInicio { get; set; }

        [DisplayName("Data Fim")]
        public System.DateTime? DataFim { get; set; }

        public virtual CondominoViewModel CondominoViewModel { get; set; }

        public string EdificacaoDescricao { get; set; }
    }
}