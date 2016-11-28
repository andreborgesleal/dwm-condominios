using System.ComponentModel;
using App_Dominio.Component;
using System.ComponentModel.DataAnnotations;

namespace DWM.Models.Repositories
{
    public class InformativoComentarioViewModel : Repository
    {
        [DisplayName("ID")]
        public int InformativoID { get; set; }

        [DisplayName("Data")]
        public System.DateTime DataComentario { get; set; }

        [DisplayName("Condômino")]
        public int CondominoID { get; set; }

        /// <summary>
        /// Nome do condômino
        /// </summary>
        public string Nome { get; set; }

        public int EdificacaoID { get; set; }

        public string DescricaoEdificacao { get; set; }

        public int UnidadeID { get; set; }

        [DisplayName("Comentário")]
        [Required(ErrorMessage = "Comentário deve ser preenchido")]
        public string Descricao { get; set; }

        [DisplayName("Desativação")]
        public System.DateTime? DataDesativacao { get; set; }

        [DisplayName("Motivo")]
        public string Motivo { get; set; }
    }
}