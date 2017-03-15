using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App_Dominio.Component;
using App_Dominio.Security;
using App_Dominio.Entidades;
using System.IO;
using System.Web;

namespace DWM.Models.Repositories
{
    public class ArquivoViewModel : Repository
    {
        [DisplayName("ID")]
        public string FileID { get; set; }

        [DisplayName("Condomínio")]
        public int CondominioID { get; set; }

        [DisplayName("Edificação")]
        [Required(ErrorMessage = "Informe a Edificação")]
        public System.Nullable<int> EdificacaoID { get; set; }

        [DisplayName("Unidade")]
        [Required(ErrorMessage = "Informe a Unidade")]
        public System.Nullable<int> UnidadeID { get; set; }

        [DisplayName("Condômino")]
        public System.Nullable<int> CondominoID { get; set; }

        [DisplayName("Grupo")]
        public System.Nullable<int> GrupoCondominoID { get; set; }

        [DisplayName("Data")]
        public System.DateTime Data { get; set; }

        [DisplayName("Nome")]
        [Required(ErrorMessage = "Nome do arquivo deve ser informado")]
        [StringLength(60, ErrorMessage = "Nome do arquivo deve ter no máximo 50 caracteres")]
        public string Nome { get; set; }

        [DisplayName("Sempre Visível")]
        public string IndSempreVisivel { get; set; }

        public string EdificacaoDescricao { get; set; }
        public string GrupoCondominoDescricao { get; set; }
        public string CondominoNome { get; set; }

        public string Path()
        {
            if (empresaId == 0)
            {
                EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
                Sessao sessaoCorrente = security.getSessaoCorrente();
                empresaId = sessaoCorrente.empresaId;
            }
            return "~/Users_Data/Empresas/" + empresaId.ToString() + "/download/";
        }

        public string Extension()
        {
            System.IO.FileInfo f = new System.IO.FileInfo(System.IO.Path.Combine(Path(), FileID));
            return f.Extension;
        }

        public string MapFile()
        {
            if (empresaId == 0)
            {
                EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
                Sessao sessaoCorrente = security.getSessaoCorrente();
                empresaId = sessaoCorrente.empresaId;
            }
            return "~/Users_Data/Empresas/" + empresaId.ToString() + "/download/" + FileID;
        }
    }
}