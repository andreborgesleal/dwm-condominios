using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App_Dominio.Component;

namespace DWM.Models.Repositories
{
    public class FilaAtendimentoViewModel : Repository
    {
        [DisplayName("ID")]
        public int FilaAtendimentoID { get; set; }

        [DisplayName("CondominioID")]
        [Required(ErrorMessage = "Código identificador do condomínio deve ser informado")]
        public int CondominioID { get; set; }

        [DisplayName("Descrição")]
        [Required(ErrorMessage = "Descrição da fila deve ser informada")]
        [StringLength(30, ErrorMessage ="Descrição deve ter no máximo 30 caracteres")]
        public string Descricao { get; set; }

        [DisplayName("Visibilidade")]
        public string VisibilidadeCondomino { get; set; }

        public int FilaCondominoID { get; set; }
    }
}