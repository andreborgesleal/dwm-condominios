using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App_Dominio.Component;

namespace DWM.Models.Repositories
{
    public class EmailTipoViewModel : Repository
    {
        [DisplayName("ID")]
        public int EmailTipoID { get; set; }

        [DisplayName("CondominioID")]
        public int CondominioID { get; set; }

        [DisplayName("Descricao")]
        [Required(ErrorMessage = "Descrição deve ser informada")]
        public string Descricao { get; set; }

        [DisplayName("Assunto")]
        public string Assunto { get; set; }

        [DisplayName("IndTipoFixo")]
        public string IndTipoFixo { get; set; }
    }
}