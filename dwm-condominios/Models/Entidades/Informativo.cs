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

        [DisplayName("Condominio")]
        public System.Nullable<int> CondominioID { get; set; }

        [DisplayName("EdificacaoID")]
        public System.Nullable<int> EdificacaoID { get; set; }

        [DisplayName("GrupoCondomino")]
        public System.Nullable<int> GrupoCondominoID { get; set; }

        [DisplayName("DataInformativo")]
        public DateTime DataInformativo { get; set; }

        [DisplayName("DataPublicacao")]
        public DateTime DataPublicacao { get; set; }

        [DisplayName("DataExpiracao")]
        public System.Nullable<DateTime> DataExpiracao { get; set; }

        [DisplayName("Cabecalho")]
        public string Cabecalho { get; set; }

        [DisplayName("Resumo")]
        public string Resumo { get; set; }

        [DisplayName("MensagemDetalhada")]
        public string MensagemDetalhada { get; set; }

        [DisplayName("Midia1")]
        public string Midia1 { get; set; }

        [DisplayName("Midia2")]
        public string Midia2 { get; set; }

        [DisplayName("InformativoAnuncio")]
        public string InformativoAnuncio { get; set; }

    }
}