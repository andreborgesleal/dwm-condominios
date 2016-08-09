using App_Dominio.Component;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DWM.Models.Repositories
{
    public class InformativoViewModel : Repository
    {
        [DisplayName("ID")]
        public int InformativoID { get; set; }

        [DisplayName("Data Informativo")]
        [Required(ErrorMessage = "Informe a Data do Informativo")]
        public DateTime DataInformativo { get; set; }

        [DisplayName("Data Publicacao")]
        public DateTime DataPublicacao { get; set; }

        [DisplayName("Data Expiracao")]
        public DateTime DataExpiracao { get; set; }

        [DisplayName("Cabeçalho")]
        [Required(ErrorMessage = "Informe o Cabeçalho")]
        [StringLength(30, ErrorMessage = "Cabeçalho deve ter no máximo 30 caracteres")]
        public string Cabecalho { get; set; }

        [DisplayName("Resumo")]
        [StringLength(500, ErrorMessage = "Resumo deve ter no máximo 500 caracteres")]
        [Required(ErrorMessage = "Informe o Resumo")]
        public string Resumo { get; set; }

        [DisplayName("Mensagem Detalhada")]
        [StringLength(4000, ErrorMessage = "Mensagem Detalhada deve ter no máximo 4000 caracteres")]
        public string MensagemDetalhada { get; set; }
    }
}