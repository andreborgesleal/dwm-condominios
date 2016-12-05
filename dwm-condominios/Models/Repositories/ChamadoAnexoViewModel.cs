using System.ComponentModel;
using App_Dominio.Component;
using System.ComponentModel.DataAnnotations;

namespace DWM.Models.Repositories
{
    public class ChamadoAnexoViewModel : Repository
    {
        [DisplayName("ChamadoID")]
        public int ChamadoID { get; set; }

        [DisplayName("Arquivo")]
        public string FileID { get; set; }

        [DisplayName("Data do Anexo")]
        public System.DateTime DataAnexo { get; set; }

        [DisplayName("Nome Original")]
        public string NomeOriginal { get; set; }

        [DisplayName("UsuarioID")]
        public int UsuarioID { get; set; }

        [DisplayName("Nome")]
        public string Nome { get; set; }

        [DisplayName("Login")]
        public string Login { get; set; }
    }
}