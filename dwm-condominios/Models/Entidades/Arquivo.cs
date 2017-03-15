using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("Arquivo")]
    public class Arquivo
    {
        [Key]
        [DisplayName("ID")]
        public string FileID { get; set; }

        [DisplayName("CondominioID")]
        public int CondominioID { get; set; }

        [DisplayName("EdificacaoID")]
        public System.Nullable<int> EdificacaoID { get; set; }

        [DisplayName("UnidadeID")]
        public System.Nullable<int> UnidadeID { get; set; }

        [DisplayName("CondominoID")]
        public System.Nullable<int> CondominoID { get; set; }

        [DisplayName("GrupoCondomino")]
        public System.Nullable<int> GrupoCondominoID { get; set; }

        [DisplayName("Nome")]
        public string Nome { get; set; }

        [DisplayName("Data")]
        public DateTime Data { get; set; }

        [DisplayName("IndSempreVisivel")]
        public string IndSempreVisivel { get; set; }
    }
}