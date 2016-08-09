using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("Informativo")]
    public class Informativo
    {
        [Key]
        [DisplayName("ID")]
        public int InformativoID { get; set; }

        [DisplayName("DataInformativo")]
        public DateTime DataInformativo { get; set; }

        [DisplayName("DataPublicacao")]
        public DateTime DataPublicacao { get; set; }

        [DisplayName("DataExpiracao")]
        public DateTime DataExpiracao { get; set; }

        [DisplayName("Cabecalho")]
        public string Cabecalho { get; set; }

        [DisplayName("Resumo")]
        public string Resumo { get; set; }

        [DisplayName("MensagemDetalhada")]
        public string MensagemDetalhada { get; set; }
    }
}