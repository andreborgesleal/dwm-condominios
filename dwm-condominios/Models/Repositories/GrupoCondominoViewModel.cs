using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App_Dominio.Component;

namespace DWM.Models.Repositories
{
    public class GrupoCondominoViewModel : Repository
    {
        [DisplayName("ID")]
        public int GrupoCondominoID { get; set; }

        [DisplayName("Condomínio")]
        [Required(ErrorMessage = "Código identificador do condomínio deve ser informado")]
        public int CondominioID { get; set; }

        public string nome_condominio { get; set; }

        [DisplayName("Descrição")]
        [Required(ErrorMessage = "Descrição do grupo deve ser informada")]
        public string Descricao { get; set; }

        [DisplayName("Objetivo")]
        public string Objetivo { get; set; }

        [DisplayName("Privativo Adimnistração")]
        public string PrivativoAdmin { get; set; }
    }
}