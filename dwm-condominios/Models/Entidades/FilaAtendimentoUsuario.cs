using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("FilaAtendimentoUsuario")]
    public class FilaAtendimentoUsuario
    {
        [Key, Column(Order = 0)]
        [DisplayName("ID")]
        public int FilaAtendimentoID { get; set; }

        [Key, Column(Order = 1)]
        [DisplayName("UsuarioID")]
        public int UsuarioID { get; set; }

        [DisplayName("Situacao")]
        public string Situacao { get; set; }

        [DisplayName("Nome")]
        public string Nome { get; set; }

        [DisplayName("Login")]
        public string Login { get; set; }
    }
}