using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("ChamadoAnotacao")]
    public class ChamadoAnotacao
    {
        [Key, Column(Order = 0)]
        [DisplayName("ChamadoID")]
        public int ChamadoID { get; set; }

        [Key, Column(Order = 1)]
        [DisplayName("DataAnotacao")]
        public System.DateTime DataAnotacao { get; set; }

        [DisplayName("Mensagem")]
        public string Mensagem { get; set; }

        [DisplayName("UsuarioID")]
        public int UsuarioID { get; set; }

        [DisplayName("Nome")]
        public string Nome { get; set; }

        [DisplayName("Login")]
        public string Login { get; set; }

    }
}