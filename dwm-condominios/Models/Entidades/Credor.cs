using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DWM.Models.Entidades
{
    [Table("Credor")]
    public class Credor
    {
        [Key]
        [DisplayName("ID")]
        public int credorId { get; set; }

        [DisplayName("CondominioID")]
        public int CondominioID { get; set; }

        [DisplayName("GrupoCredorID")]
        public Nullable<int> grupoCredorId { get; set; }

        [DisplayName("Nome")]
        public string nome { get; set; }

        [DisplayName("Ind_Tipo_Pessoa")]
        public string ind_tipo_pessoa { get; set; }

        [DisplayName("CPF_CNPJ")]
        public string cpf_cnpj { get; set; }

        [DisplayName("Dt_Inclusao")]
        public DateTime dt_inclusao { get; set; }

        [DisplayName("Dt_Alteracao")]
        public DateTime dt_alteracao { get; set; }

        [DisplayName("Codigo")]
        public string codigo { get; set; }

        [DisplayName("Endereco")]
        public string endereco { get; set; }

        [DisplayName("Complemento")]
        public string complemento { get; set; }

        [DisplayName("Cidade")]
        public string cidade { get; set; }

        [DisplayName("UF")]
        public string uf { get; set; }

        [DisplayName("CEP")]
        public string cep { get; set; }

        [DisplayName("Bairro")]
        public string bairro { get; set; }

        [DisplayName("Fone1")]
        public string fone1 { get; set; }

        [DisplayName("Fone2")]
        public string fone2 { get; set; }

        [DisplayName("Fone3")]
        public string fone3 { get; set; }

        [DisplayName("Email")]
        public string email { get; set; }

        [DisplayName("Sexo")]
        public string sexo { get; set; }

        [DisplayName("Dt_Nascimento")]
        public Nullable<DateTime> dt_nascimento { get; set; }

        [DisplayName("Observacao")]
        public string observacao { get; set; }
    }
}