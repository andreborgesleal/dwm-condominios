using System.ComponentModel;
using App_Dominio.Component;

namespace DWM.Models.Repositories
{
    public class ChamadoFilaViewModel : Repository
    {
        [DisplayName("ChamadoID")]
        public int ChamadoID { get; set; }

        [DisplayName("Data")]
        public System.DateTime Data { get; set; }

        [DisplayName("FilaAtendimentoID")]
        public int FilaAtendimentoID { get; set; }

        public string DescricaoFilaAtendimento { get; set; }

        [DisplayName("UsuarioID")]
        public System.Nullable<int> UsuarioID { get; set; }

        [DisplayName("Nome")]
        public string Nome { get; set; }

        [DisplayName("Login")]
        public string Login { get; set; }
    }
}