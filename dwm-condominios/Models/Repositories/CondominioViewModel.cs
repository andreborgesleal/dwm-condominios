using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App_Dominio.Component;

namespace DWM.Models.Repositories
{
    public class CondominioViewModel : Repository
    {
        [DisplayName("ID")]
        public int CondominioID { get; set; }

        [DisplayName("Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Informe um email válido")]
        [Required(ErrorMessage = "Informe o email")]
        [StringLength(100, ErrorMessage = "Email deve ter no máximo 100 caracteres")]
        public string Email { get; set; }
        
        [DisplayName("Razão Social")]
        [StringLength(80, ErrorMessage = "Razão Social deve ter no máximo 80 caracteres")]
        [Required(ErrorMessage = "Informe a Razão Social")]
        public string RazaoSocial { get; set; }

        [DisplayName("CNPJ")]
        [StringLength(14, ErrorMessage = "CNPJ deve ter no máximo 14 caracteres")]
        [Required(ErrorMessage = "Informe o CNPJ")]
        public string CNPJ { get; set; }
        
        [DisplayName("Endereço")]
        [StringLength(60, ErrorMessage = "Endereço deve ter no máximo 60 caracteres")]
        public string Endereco { get; set; }
        
        [DisplayName("Complemento")]
        [StringLength(20, ErrorMessage = "Complemento deve ter no máximo 20 caracteres")]
        public string Complemento { get; set; }
        
        [DisplayName("CEP")]
        [StringLength(8, ErrorMessage = "CEP deve ter no máximo 8 caracteres")]
        public string CEP { get; set; }
        
        [DisplayName("Logo")]
        [StringLength(100, ErrorMessage = "Logo deve ter no máximo 100 caracteres")]
        public string Logo { get; set; }
        
        [DisplayName("Path Logo")]
        [StringLength(80, ErrorMessage = "PathLogo deve ter no máximo 80 caracteres")]
        public string PathLogo { get; set; }
        
        [DisplayName("Latitude")]
        [StringLength(20, ErrorMessage = "Latitude deve ter no máximo 20 caracteres")]
        public string Latitude { get; set; }
        
        [DisplayName("Longitude")]
        [StringLength(20, ErrorMessage = "Longitude deve ter no máximo 20 caracteres")]
        public string Longitudode { get; set; }
        
        [DisplayName("Indicador Tipo Condômino")]
        [StringLength(1, ErrorMessage = "Indicador Tipo Condômino só permite 1 caractere")]
        [Required(ErrorMessage = "Informe o Indicador Tipo Condômino")]
        public string IndTipoCondomino { get; set; }
    }
}