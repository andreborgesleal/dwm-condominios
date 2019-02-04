using App_Dominio.Component;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DWM.Models.Repositories
{
    public class CredorPropostaViewModel : Repository
    {
        [DisplayName("ID")]
        public int CredorPropostaID { get; set; }

        [DisplayName("LicitacaoID")]
        public int LicitacaoID { get; set; }

        [DisplayName("credorId")]
        public int credorId { get; set; }

        [DisplayName("DataProposta")]
        [Required]
        public DateTime DataProposta { get; set; }

        [DisplayName("Valor")]
        [Required]
        public decimal Valor { get; set; }

        [DisplayName("ArquivoProposta")]
        [StringLength(100)]
        public string ArquivoProposta { get; set; }
    }
}