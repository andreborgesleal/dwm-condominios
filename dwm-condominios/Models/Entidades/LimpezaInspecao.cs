using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace DWM.Models.Entidades
{
    [Table("LimpezaInspecao")]
    public class LimpezaInspecao
    {
        [DisplayName("LimpezaInspecaoID")]
        public int LimpezaInspecaoID { get; set; }

        [DisplayName("credorId")]
        public int credorId { get; set; }

        [DisplayName("EspacoID")]
        public int EspacoID { get; set; }

        [DisplayName("DataInspecao")]
        public DateTime DataInspecao { get; set; }

        [DisplayName("NotaFinal")]
        public int NotaFinal { get; set; }

        [StringLength(4000)]
        [DisplayName("Laudo")]
        public string Laudo { get; set; }

        //public virtual Credor Credor { get; set; }

        //public virtual EspacoComum EspacoComum { get; set; }

        //public virtual LimpezaInspecaoItem LimpezaInspecaoItem { get; set; }
    }
}