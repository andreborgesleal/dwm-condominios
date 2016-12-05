using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App_Dominio.Component;

namespace DWM.Models.Repositories
{
    public class ChamadoMotivoViewModel : Repository
    {
        [DisplayName("ID")]
        public int ChamadoMotivoID { get; set; }

        [DisplayName("CondominioID")]
        public int CondominioID { get; set; }

        [DisplayName("FilaAtendimentoID")]
        [Required(ErrorMessage = "Fila de atendimento deve ser informada")]
        public int FilaAtendimentoID { get; set; }

        public string DescricaoFilaAtendimento { get; set; }

        [DisplayName("Descricao")]
        [Required(ErrorMessage = "Descrição do motivo deve ser informada")]
        [StringLength(30, ErrorMessage = "Descrição do motivo deve ter no máximo 30 caracteres")]
        public string Descricao { get; set; }

        [DisplayName("IndFixo")]
        public string IndFixo { get; set; }
    }
}