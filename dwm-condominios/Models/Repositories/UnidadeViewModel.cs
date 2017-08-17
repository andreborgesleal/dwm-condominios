using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App_Dominio.Component;

namespace DWM.Models.Repositories
{
    public class UnidadeViewModel : Repository
    {
        [DisplayName("Condomínio ID")]
        public int CondominioID { get; set; }

        [DisplayName("Edificação")]
        [Required(ErrorMessage = "Informe a Edificação")]
        public int EdificacaoID { get; set; }

        public string DescricaoEdificacao { get; set; }

        [DisplayName("Unidade")]
        [Required(ErrorMessage = "Informe a Unidade")]
        public int UnidadeID { get; set; }

        [DisplayName("Código")]
        [Required(ErrorMessage = "Informe o Código da Unidade")]
        public string Codigo { get; set; }

        [DisplayName("Tipo Unidade")]
        [Required(ErrorMessage = "Informe o tipo de unidade: Residencial ou Comercial")]
        public string TipoUnidade { get; set; }

        [DisplayName("Tipo Condômino")]
        [Required(ErrorMessage = "Informe o tipo do condômino: Pessoa Física ou Pessoa Jurídica")]
        public string TipoCondomino { get; set; }

        [DisplayName("Nº Vagas Garagem")]
        [Required(ErrorMessage = "Informe o número de vagas de garagem")]
        public int NumVagas { get; set; }

        [DisplayName("Validador")]
        public string Validador { get; set; }

        [DisplayName("Data Expiração")]
        public System.DateTime? DataExpiracao { get; set; }

        [DisplayName("Nome")]
        [StringLength(60, ErrorMessage = "Nome deve ter no máximo 60 caracteres")]
        public string NomeCondomino { get; set; }

        [DisplayName("E-mail")]
        [StringLength(100, ErrorMessage = "E-mail deve ter no máximo 100 caracteres")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Informe um e-mail válido")]
        [EmailAddress(ErrorMessage = "Informe o E-mail com um formato válido")]
        public string Email { get; set; }

        public string EdificacaoDescricao { get; set; }
    }
}