using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("ChamadoFila")]
    public class ChamadoFila
    {
        [Key, Column(Order = 0)]
        [DisplayName("ChamadoID")]
        public int ChamadoID { get; set; }

        [Key, Column(Order = 1)]
        [DisplayName("Data")]
        public System.DateTime Data { get; set; }

        [DisplayName("FilaAtendimentoID")]
        public int FilaAtendimentoID { get; set; }

        [DisplayName("UsuarioID")]
        public System.Nullable<int> UsuarioID { get; set; }

        [DisplayName("Nome")]
        public string Nome { get; set; }

        [DisplayName("Login")]
        public string Login { get; set; }
    }
}