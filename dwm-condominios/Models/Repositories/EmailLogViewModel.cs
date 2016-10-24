using App_Dominio.Component;
using System.ComponentModel;

namespace DWM.Models.Repositories
{
    public class EmailLogViewModel : Repository
    {
        [DisplayName("ID")]
        public int EmailLogID { get; set; }

        [DisplayName("Tipo")]
        public int EmailTipoID { get; set; }

        public string Descricao_EmailTipo { get; set; }

        [DisplayName("Condomínio")]
        public int CondominioID { get; set; }

        [DisplayName("Edificação")]
        public System.Nullable<int> EdificacaoID { get; set; }

        public string Descricao_Edificacao { get; set; }

        [DisplayName("Unidade")]
        public System.Nullable<int> UnidadeID { get; set; }

        [DisplayName("Grupo Condômino")]
        public System.Nullable<int> GrupoCondominoID { get; set; }

        public string Descricao_GrupoCondomino { get; set; }

        [DisplayName("Data")]
        public System.DateTime DataEmail { get; set; }

        [DisplayName("Assunto")]
        public string Assunto { get; set; }

        [DisplayName("Mensagem do E-mail")]
        public string EmailMensagem { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }
    }
}