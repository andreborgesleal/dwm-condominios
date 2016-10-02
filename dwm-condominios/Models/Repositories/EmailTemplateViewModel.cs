using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App_Dominio.Component;

namespace DWM.Models.Repositories
{
    public class EmailTemplateViewModel : Repository
    {
        [DisplayName("ID")]
        public int EmailTemplateID { get; set; }

        [DisplayName("EmailTipoID")]
        public int EmailTipoID { get; set; }

        /// <summary>
        /// Descrição do tipo de e-mail
        /// </summary>
        public string Descricao { get; set; }

        [DisplayName("CondominioID")]
        public int CondominioID { get; set; }

        [DisplayName("Nome")]
        [Required(ErrorMessage = "Nome do template deve ser informado")]
        public string Nome { get; set; }

        [DisplayName("EmailMensagem")]
        [Required(ErrorMessage = "Corpo da Mensagem deve ser informado")]
        public string EmailMensagem { get; set; }
    }
}