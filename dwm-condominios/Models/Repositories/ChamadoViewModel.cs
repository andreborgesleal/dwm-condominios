using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App_Dominio.Component;
using System.Collections.Generic;
using System.Web.Mvc;

namespace DWM.Models.Repositories
{
    public class ChamadoViewModel : Repository
    {
        [DisplayName("ID")]
        public int ChamadoID { get; set; }

        [DisplayName("Motivo")]
        [Required(ErrorMessage = "Motivo do chamado deve ser informado")]
        public int ChamadoMotivoID { get; set; }

        public string DescricaoChamadoMotivo { get; set; }

        [DisplayName("Situação")]
        [Required(ErrorMessage = "Situação do chamado deve ser informada")]
        public int ChamadoStatusID { get; set; }

        public string DescricaoChamadoStatus { get; set; }

        [DisplayName("Fila Solicitante")]
        [Required(ErrorMessage = "Fila solicitante do chamado deve ser informada")]
        public int FilaSolicitanteID { get; set; }

        public string DescricaoFilaSolicitante { get; set; }

        [DisplayName("CondominioID")]
        public int CondominioID { get; set; }

        [DisplayName("Condômino")]
        public System.Nullable<int> CondominoID { get; set; }

        [DisplayName("Credenciado")]
        public System.Nullable<int> CredenciadoID { get; set; }

        public string NomeCondomino { get; set; }

        [DisplayName("Edificação")]
        public System.Nullable<int> EdificacaoID { get; set; }

        public string DescricaoEdificacao { get; set; }

        [DisplayName("Unidade")]
        public System.Nullable<int> UnidadeID { get; set; }

        [DisplayName("Data")]
        public System.DateTime DataChamado { get; set; }

        [DisplayName("Assunto")]
        [Required(ErrorMessage = "Assunto deve ser informado")]
        [StringLength(60, ErrorMessage = "Descrição do assunto deve ter no mínimo 10 e no máximo 60 caracteres", MinimumLength = 10)]
        public string Assunto { get; set; }

        [DisplayName("Usuário")]
        public int UsuarioID { get; set; }

        [DisplayName("Nome Usuário")]
        public string NomeUsuario { get; set; }

        [DisplayName("Login Usuário")]
        public string LoginUsuario { get; set; }

        public bool IsFornecedor { get; set; }

        [DisplayName("Prioridade")]
        [Required(ErrorMessage = "Prioridade deve ser informada")]
        public string Prioridade { get; set; }

        [DisplayName("Última Anotação")]
        public System.Nullable<System.DateTime> DataUltimaAnotacao { get; set; }

        [DisplayName("Mensagem")]
        [AllowHtml]
        [Required(ErrorMessage = "Mensagem do chamado deve ser informada")]
        public string MensagemOriginal { get; set; }

        [DisplayName("Fila de Atendimento")]
        public System.Nullable<int> FilaAtendimentoID { get; set; }

        public string DescricaoFilaAtendimento { get; set; }
        public int FilaCondominoID { get; set; }

        [DisplayName("Data do Redirecionamento")]
        public System.DateTime DataRedirecionamento { get; set; }

        [DisplayName("UsuarioFilaID")]
        public System.Nullable<int> UsuarioFilaID { get; set; }

        [DisplayName("NomeUsuarioFila")]
        public string NomeUsuarioFila { get; set; }

        [DisplayName("LoginUsuarioFila")]
        public string LoginUsuarioFila { get; set; }

        public string Solicitante
        {
            get
            {
                if (FilaSolicitanteID == FilaCondominoID)
                    return NomeUsuario;
                else
                    return DescricaoFilaSolicitante;
            }
        }

        public IEnumerable<ChamadoAnotacaoViewModel> Anotacoes{ get; set; }

        public virtual ChamadoFilaViewModel ChamadoFilaViewModel { get; set; }
        public IEnumerable<ChamadoFilaViewModel> Rotas { get; set; }

        public ChamadoAnexoViewModel ChamadoAnexoViewModel { get; set; }
        public virtual IEnumerable<ChamadoAnexoViewModel> Anexos { get; set; }

        public PagedList<CondominoUnidadeViewModel> Condominos { get; set; }
    }
}