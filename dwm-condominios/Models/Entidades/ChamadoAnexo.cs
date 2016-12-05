using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("ChamadoAnexo")]
    public class ChamadoAnexo
    {
        [Key, Column(Order = 0)]
        [DisplayName("ChamadoID")]
        public int ChamadoID { get; set; }

        [Key, Column(Order = 1)]
        [DisplayName("FileID")]
        public string FileID { get; set; }

        [DisplayName("DataAnexo")]
        public System.DateTime DataAnexo { get; set; }

        [DisplayName("NomeOriginal")]
        public string NomeOriginal { get; set; }

        [DisplayName("UsuarioID")]
        public int UsuarioID { get; set; }

        [DisplayName("Nome")]
        public string Nome { get; set; }

        [DisplayName("Login")]
        public string Login { get; set; }
    }
}