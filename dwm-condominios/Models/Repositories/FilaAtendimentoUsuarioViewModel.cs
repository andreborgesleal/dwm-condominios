using System.ComponentModel;
using App_Dominio.Component;

namespace DWM.Models.Repositories
{
    public class FilaAtendimentoUsuarioViewModel : Repository
    {
        [DisplayName("ID")]
        public int FilaAtendimentoID { get; set; }

        [DisplayName("UsuarioID")]
        public int UsuarioID { get; set; }

        public string nome { get; set; }

        public string login { get; set; }

        [DisplayName("Situacao")]
        public string Situacao { get; set; }

        public string DescricaoFila { get; set; }

        [DisplayName("Nome")]
        public string Nome { get; set; }

        [DisplayName("Login")]
        public string Login { get; set; }

    }
}