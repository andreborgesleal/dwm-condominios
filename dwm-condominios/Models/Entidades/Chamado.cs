using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("Chamado")]
    public class Chamado
    {
        [Key]
        public int ChamadoID { get; set; }

        [DisplayName("ChamadoMotivoID")]
        public int ChamadoMotivoID { get; set; }

        [DisplayName("ChamadoStatusID")]
        public int ChamadoStatusID { get; set; }

        [DisplayName("FilaSolicitanteID")]
        public int FilaSolicitanteID { get; set; }

        [DisplayName("CondominioID")]
        public int CondominioID { get; set; }

        [DisplayName("CondominoID")]
        public System.Nullable<int> CondominoID { get; set; }

        [DisplayName("CredenciadoID")]
        public System.Nullable<int> CredenciadoID { get; set; }

        [DisplayName("EdificacaoID")]
        public System.Nullable<int> EdificacaoID { get; set; }

        [DisplayName("UnidadeID")]
        public System.Nullable<int> UnidadeID { get; set; }

        [DisplayName("DataChamado")]
        public System.DateTime DataChamado { get; set; }

        [DisplayName("Assunto")]
        public string Assunto { get; set; }

        [DisplayName("UsuarioID")]
        public int UsuarioID { get; set; }

        [DisplayName("NomeUsuario")]
        public string NomeUsuario { get; set; }

        [DisplayName("LoginUsuario")]
        public string LoginUsuario { get; set; }

        [DisplayName("Prioridade")]
        public string Prioridade { get; set; }

        [DisplayName("DataUltimaAnotacao")]
        public System.Nullable<System.DateTime> DataUltimaAnotacao { get; set; }

        [DisplayName("MensagemOriginal")]
        public string MensagemOriginal { get; set; }

        [DisplayName("FilaAtendimentoID")]
        public System.Nullable<int> FilaAtendimentoID { get; set; }

        [DisplayName("DataRedirecionamento")]
        public System.DateTime DataRedirecionamento { get; set; }

        [DisplayName("UsuarioFilaID")]
        public System.Nullable<int> UsuarioFilaID { get; set; }

        [DisplayName("NomeUsuarioFila")]
        public string NomeUsuarioFila { get; set; }

        [DisplayName("LoginUsuarioFila")]
        public string LoginUsuarioFila { get; set; }

        public virtual ChamadoFila ChamadoFila { get; set; }

        public virtual ICollection<ChamadoAnexo> Anexos { get; set; }
    }
}