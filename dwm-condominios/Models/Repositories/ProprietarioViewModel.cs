using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App_Dominio.Component;
using System.Collections.Generic;

namespace DWM.Models.Repositories
{
    public class ProprietarioViewModel : Repository
    {
        [DisplayName("ID")]
        public int ProprietarioID { get; set; }

        [DisplayName("Nome")]
        [Required(ErrorMessage = "Nome do proprietário deve ser informado")]
        [StringLength(50, ErrorMessage = "Nome deve ter no máximo 50 caracteres")]
        public string Nome { get; set; }

        [DisplayName("Ind Tipo Pessoa")]
        [Required(ErrorMessage = "Tipo Pessoa (Física ou Jurídica) dever ser informado")]
        [StringLength(1, ErrorMessage = "Ind Tipo Pessoa deve ter no máximo 1 caractere")]
        public string IndTipoPessoa { get; set; }

        [DisplayName("Ind Fiscal")]
        [Required(ErrorMessage = "O CPF/CNPJ deve ser informado.")]
        public string IndFiscal { get; set; }

        [DisplayName("Email")]
        [Required(ErrorMessage = "E-mail deve ser informado")]
        [StringLength(100, ErrorMessage = "Email deve ter no máximo 100 caracteres")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Informe um Email válido")]
        public string Email { get; set; }

        [DisplayName("Telefone")]
        public string Telefone { get; set; }

        [DisplayName("Endereço")]
        [StringLength(50, ErrorMessage = "Endereço deve ter no máximo 50 caracteres")]
        public string Endereco { get; set; }

        [DisplayName("Complemento")]
        [StringLength(25, ErrorMessage = "Complemento deve ter no máximo 25 caracteres")]
        public string Complemento { get; set; }

        [DisplayName("Cidade ID")]
        public int CidadeID { get; set; }

        public string CidadeDescricao { get; set; }

        [DisplayName("UF")]
        [StringLength(2, ErrorMessage = "UF deve ter no máximo 2 caracteres")]
        public string UF { get; set; }

        [DisplayName("CEP")]
        public string CEP { get; set; }

        public virtual IEnumerable<ProprietarioUnidadeViewModel> ProprietarioUnidades { get; set; }
    }
}