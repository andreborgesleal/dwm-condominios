using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace DWM.Models.Entidades
{
    [Table("LimpezaInspecaoItem")]
    public class LimpezaInspecaoItem
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [DisplayName("LimpezaInspecaoID")]
        public int LimpezaInspecaoID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [DisplayName("LimpezaRequisitoID")]
        public int LimpezaRequisitoID { get; set; }

        [DisplayName("Nota")]
        public int? Nota { get; set; }

        [StringLength(500)]
        [DisplayName("Justificativa")]
        public string Justificativa { get; set; }

        
    }
}