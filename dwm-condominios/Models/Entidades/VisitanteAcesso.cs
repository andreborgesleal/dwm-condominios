using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DWM.Models.Entidades
{
    [Table("VisitanteAcesso")]
    public class VisitanteAcesso
    {
        [Key]
        [DisplayName("AcessoID")]
        public int AcessoID { get; set; }

        [DisplayName("VisitanteID")]
        public int VisitanteID { get; set; }

        [DisplayName("CondominioID")]
        public int CondominioID { get; set; }

        [DisplayName("DataInclusao")]
        public DateTime DataInclusao { get; set; }

        [DisplayName("DataAutorizacao")]
        public DateTime DataAutorizacao { get; set; }

        [DisplayName("HoraInicio")]
        public string HoraInicio { get; set; }

        [DisplayName("HoraLimite")]
        public string HoraLimite { get; set; }

        [DisplayName("DataAcesso")]
        public DateTime? DataAcesso { get; set; }
    }
}