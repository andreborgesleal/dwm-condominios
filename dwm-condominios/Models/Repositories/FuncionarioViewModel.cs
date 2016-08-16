using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App_Dominio.Component;

namespace DWM.Models.Repositories
{
    public class FuncionarioViewModel : Repository
    {
        [DisplayName("ID")]
        public int FuncionarioID { get; set; }

        [DisplayName("Condomínio")]
        [Required(ErrorMessage = "Condomínio deve ser informado")]
        public int CondominioID { get; set; }

        [DisplayName("Edificação")]
        [Required(ErrorMessage = "Edificação deve ser informada")]
        public int EdificacaoID { get; set; }

        [DisplayName("Unidade")]
        [Required(ErrorMessage = "Unidade deve ser informada")]
        public int UnidadeID { get; set; }

        [DisplayName("Condôminio")]
        [Required(ErrorMessage = "Condômino deve ser informado")]
        public int CondominoID { get; set; }

        [DisplayName("Nome")]
        [Required(ErrorMessage = "Informe o Nome do funcionário")]
        [StringLength(40, ErrorMessage = "Nome deve ter no máximo 40 caracteres")]
        public string Nome { get; set; }

        [DisplayName("Função")]
        [StringLength(30, ErrorMessage = "Função deve ter no máximo 30 caracteres")]
        public string Funcao { get; set; }

        [DisplayName("Sexo")]
        [Required(ErrorMessage = "Informe o Sexo")]
        public string Sexo { get; set; }

        [DisplayName("Dia")]
        [Required(ErrorMessage = "Informe o Dia")]
        public string Dia { get; set; }

        [DisplayName("Início")]
        public string HoraInicial { get; set; }

        [DisplayName("Fim")]
        public string HoraFinal { get; set; }

        [DisplayName("RG")]
        [StringLength(20, ErrorMessage = "RG deve ter no máximo 20 caracteres")]
        public string RG { get; set; }
    }
}