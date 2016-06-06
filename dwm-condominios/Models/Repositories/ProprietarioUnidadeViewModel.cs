using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App_Dominio.Component;

namespace DWM.Models.Repositories
{
    public class ProprietarioUnidadeViewModel : Repository
    {
        [DisplayName("ID")]
        public int ProprietarioID { get; set; }

        [DisplayName("Condomínio ID")]
        [Required(ErrorMessage = "Informe o Condomínio ID")]
        public int CondominioID { get; set; }

        [DisplayName("Unidade ID")]
        [Required(ErrorMessage = "Informe a Unidade ID")]
        public int UnidadeID { get; set; }

        [DisplayName("Edificação ID")]
        [Required(ErrorMessage = "Informe a Edificação ID")]
        public int EdificacaoID { get; set; }

        [DisplayName("Data Fim")]
        public System.DateTime DataFim { get; set; }
    }
}