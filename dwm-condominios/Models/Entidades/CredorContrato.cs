using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DWM.Models.Entidades
{
    [Table("CredorContrato")]
    public class CredorContrato
    {
        [Key]
        [DisplayName("ID")]
        public int ContratoID { get; set; }

        [DisplayName("CondominioID")]
        public int CondominioID { get; set; }

        [DisplayName("TipoServicoID")]
        public int TipoServicoID { get; set; }

        [DisplayName("LicitacaoID")]
        public int? LicitacaoID { get; set; }

        [DisplayName("credorId")]
        public int credorId { get; set; }

        [DisplayName("DataContrato")]
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
        public decimal Valor { get; set; }
    }
}