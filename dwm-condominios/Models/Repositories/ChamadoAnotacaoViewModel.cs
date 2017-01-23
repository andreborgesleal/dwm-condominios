using System.ComponentModel;
using App_Dominio.Component;
using System.ComponentModel.DataAnnotations;

namespace DWM.Models.Repositories
{
    public class ChamadoAnotacaoViewModel : Repository
    {
        [DisplayName("ChamadoID")]
        public int ChamadoID { get; set; }

        [DisplayName("Data")]
        public System.DateTime DataAnotacao { get; set; }

        [DisplayName("Mensagem")]
        [Required(ErrorMessage = "Texto da anotação deve ser informado")]
        public string Mensagem { get; set; }

        [DisplayName("UsuarioID")]
        public int UsuarioID { get; set; }

        [DisplayName("Nome")]
        public string Nome { get; set; }

        [DisplayName("Login")]
        public string Login { get; set; }

        public UsuarioViewModel UsuarioViewModel { get; set; }
    }
}