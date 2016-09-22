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

        [DisplayName("Condomínio")]
        public System.Nullable<int> CondominioID { get; set; }

        [DisplayName("Edificação")]
        public System.Nullable<int> EdificacaoID { get; set; }

        public string descricao_edificacao { get; set; }

        [DisplayName("Grupo")]
        public System.Nullable<int> GrupoCondominoID { get; set; }

        public string descricao_GrupoCondomino { get; set; }

        public DateTime DataInformativo { get; set; }

        [DisplayName("Publicação")]
        [Required(ErrorMessage = "Informe a data de publicação do informativo")]
        public DateTime DataPublicacao { get; set; }

        [DisplayName("Expiracao")]
        public System.Nullable<DateTime> DataExpiracao { get; set; }

        [DisplayName("Cabeçalho")]
        [Required(ErrorMessage = "Informe o Cabeçalho")]
        [StringLength(30, ErrorMessage = "Cabeçalho deve ter no máximo 30 caracteres")]
        public string Cabecalho { get; set; }

        [DisplayName("Resumo")]
        [StringLength(500, ErrorMessage = "Resumo deve ter no máximo 500 caracteres")]
        [Required(ErrorMessage = "Informe o Resumo")]
        public string Resumo { get; set; }

        [DisplayName("Mensagem Detalhada")]
        public string MensagemDetalhada { get; set; }

        [DisplayName("Mídia1")]
        public string Midia1 { get; set; }

        [DisplayName("Mídia2")]
        public string Midia2 { get; set; }

        public string InformativoAnuncio { get; set; }

    }
}