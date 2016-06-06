using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App_Dominio.Component;

namespace DWM.Models.Repositories
{
    public class FuncionarioViewModel : Repository
    {
        [DisplayName("ID")]
        public int FuncionarioID { get; set; }

        [DisplayName("Condomínio ID")]
        [Required(ErrorMessage = "Informe o Condomínio ID")]
        public int CondominioID { get; set; }

        [DisplayName("Edificação ID")]
        public int EdificacaoID { get; set; }

        [DisplayName("Unidade ID")]
        public int UnidadeID { get; set; }

        [DisplayName("Condômino ID")]
        public int CondominoID { get; set; }

        [DisplayName("Nome")]
        [Required(ErrorMessage = "Informe o Nome")]
        [StringLength(40, ErrorMessage = "Nome deve ter no máximo 40 caracteres")]
        public string Nome { get; set; }

        [DisplayName("Função")]
        [StringLength(30, ErrorMessage = "Função deve ter no máximo 30 caracteres")]
        public string Funcao { get; set; }

        [DisplayName("Sexo")]
        [Required(ErrorMessage = "Informe o Sexo")]
        [StringLength(1, ErrorMessage = "Sexo deve ter no máximo 1 caractere")]
        public string Sexo { get; set; }

        [DisplayName("Dia")]
        [Required(ErrorMessage = "Informe o Dia")]
        [StringLength(7, ErrorMessage = "Dia deve ter no máximo 7 caracteres")]
        public string Dia { get; set; }

        [DisplayName("Hora Inicial")]
        [StringLength(5, ErrorMessage = "Hora Inicial deve ter no máximo 5 caracteres")]
        public string HoraInicial { get; set; }

        [DisplayName("Hora Final")]
        [StringLength(5, ErrorMessage = "Hora Final deve ter no máximo 5 caracteres")]
        public string HoraFinal { get; set; }

        [DisplayName("RG")]
        [StringLength(20, ErrorMessage = "RG deve ter no máximo 20 caracteres")]
        public string RG { get; set; }
    }
}