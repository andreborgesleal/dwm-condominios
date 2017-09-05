using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("Proprietario")]
    public class Proprietario
    {
        [Key]
        [DisplayName("ID")]
        public int ProprietarioID { get; set; }

        [DisplayName("Nome")]
        public string Nome { get; set; }

        [DisplayName("Ind Tipo Pessoa")]
        public string IndTipoPessoa { get; set; }

        [DisplayName("Ind Fiscal")]
        public string IndFiscal { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Telefone")]
        public string Telefone { get; set; }

        [DisplayName("Endereço")]
        public string Endereco { get; set; }

        [DisplayName("Complemento")]
        public string Complemento { get; set; }

        [DisplayName("Cidade ID")]
        public int CidadeID { get; set; }

        [DisplayName("UF")]
        public string UF { get; set; }

        [DisplayName("CEP")]
        public string CEP { get; set; }

        public virtual ICollection<ProprietarioUnidade> ProprietarioUnidades { get; set; }
    }
}