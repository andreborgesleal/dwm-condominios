using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DWM.Models.Entidades
{
    [Table("CredorProposta")]
    public class CredorProposta
    {
        [Key]
        [DisplayName("ID")]
        public int CredorPropostaID { get; set; }

        [DisplayName("LicitacaoID")]
        public int LicitacaoID { get; set; }

        [DisplayName("credorId")]
        public int credorId { get; set; }

        [DisplayName("DataProposta")]
        public DateTime DataProposta { get; set; }

        [DisplayName("Valor")]
        public decimal Valor { get; set; }

        [DisplayName("ArquivoProposta")]
        public string ArquivoProposta { get; set; }
    }
}