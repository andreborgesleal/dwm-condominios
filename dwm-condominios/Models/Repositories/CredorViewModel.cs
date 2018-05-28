using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App_Dominio.Component;
using System;

namespace DWM.Models.Repositories
{
    public class CredorViewModel : Repository
    {
        [DisplayName("ID")]
        public int credorId { get; set; }

        [DisplayName("Grupo Credor")]
        public Nullable<int> grupoCredorId { get; set; }

        [DisplayName("Nome")]
        [Required(ErrorMessage = "Nome do credor deve ser informado")]
        [StringLength(60, ErrorMessage = "Nome do credor deve ter no mínimo 5 e no máximo 60 caracteres", MinimumLength = 5)]
        public string nome { get; set; }

        [DisplayName("Tipo")]
        public string ind_tipo_pessoa { get; set; }

        [DisplayName("CondominioID")]
        public int CondominioID { get; set; }

        [DisplayName("CPF/CNPJ")]
        public string cpf_cnpj { get; set; }

        [DisplayName("Dt_Inclusao")]
        public DateTime dt_inclusao { get; set; }

        [DisplayName("Dt_Alteracao")]
        public DateTime dt_alteracao { get; set; }

        [DisplayName("Código")]
        [StringLength(10, ErrorMessage = "Código livre deve ter no máximo 10 caracteres")]
        public string codigo { get; set; }

        [DisplayName("Endereco")]
        [StringLength(50, ErrorMessage = "Endereço deve ter no máximo 50 caracteres")]
        public string endereco { get; set; }

        [DisplayName("Complemento")]
        [StringLength(30, ErrorMessage = "Complemento deve ter no máximo 30 caracteres")]
        public string complemento { get; set; }

        [DisplayName("Cidade")]
        [StringLength(25, ErrorMessage = "Cidade deve ter no máximo 25 caracteres")]
        public string cidade { get; set; }

        [DisplayName("UF")]
        [StringLength(2, ErrorMessage = "Complemento deve ter no máximo 2 caracteres")]
        public string uf { get; set; }

        [DisplayName("CEP")]
        public string cep { get; set; }

        [DisplayName("Bairro")]
        [StringLength(25, ErrorMessage = "Bairro deve ter no máximo 25 caracteres")]
        public string bairro { get; set; }

        [DisplayName("Celular")]
        public string fone1 { get; set; }

        [DisplayName("Celular 2")]
        public string fone2 { get; set; }

        [DisplayName("Fone Comercial")]
        public string fone3 { get; set; }

        [DisplayName("E-mail")]
        [StringLength(100, ErrorMessage = "E-mail deve ter no máximo 100 caracteres")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Informe um e-mail válido")]
        [EmailAddress(ErrorMessage = "Informe o E-mail com um formato válido")]
        public string email { get; set; }

        [DisplayName("Sexo")]
        public string sexo { get; set; }

        [DisplayName("Nascimento")]
        public Nullable<DateTime> dt_nascimento { get; set; }

        [DisplayName("Observação")]
        [StringLength(4000, ErrorMessage = "Observação deve ter no máximo 4000 caracteres")]
        public string observacao { get; set; }

        [DisplayName("Grupo")]
        public string nome_grupo { get; set; }
    }
}