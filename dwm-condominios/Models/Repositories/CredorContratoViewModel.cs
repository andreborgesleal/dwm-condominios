using App_Dominio.Component;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DWM.Models.Repositories
{
    public class CredorContratoViewModel : Repository
    {
        [DisplayName("ID")]
        public int ContratoID { get; set; }

        [DisplayName("CondominioID")]
        [Required]
        public int CondominioID { get; set; }

        [DisplayName("TipoServicoID")]
        [Required]
        public int TipoServicoID { get; set; }

        [DisplayName("LicitacaoID")]
        [Required]
        public int? LicitacaoID { get; set; }

        [DisplayName("credorId")]
        [Required]
        public int credorId { get; set; }

        [DisplayName("DataContrato")]
        [Required]
        public DateTime DataContrato { get; set; }
        
        [DisplayName("DataVencimento")]
        public DateTime? DataVencimento { get; set; }

        [DisplayName("DataRenovacao")]
        public DateTime? DataRenovacao { get; set; }

        [DisplayName("Historico")]
        public string Historico { get; set; }

        [DisplayName("ArquivoContrato")]
        public string ArquivoContrato { get; set; }

        [DisplayName("Valor")]
        [Required]
        public decimal Valor { get; set; }
    }
}