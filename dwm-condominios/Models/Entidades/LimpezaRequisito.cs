using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace DWM.Models.Entidades
{
    [Table("LimpezaRequisito")]
    public class LimpezaRequisito
    {
        //public LimpezaRequisito()
        //{
        //    LimpezaInspecaoItem = new HashSet<LimpezaInspecaoItem>();
        //}

        [DisplayName("LimpezaRequisitoID")]
        public int LimpezaRequisitoID { get; set; }

        [DisplayName("EspacoID")]
        public int EspacoID { get; set; }

        [Required]
        [StringLength(80)]
        [DisplayName("Descricao")]
        public string Descricao { get; set; }

        public virtual EspacoComum EspacoComum { get; set; }

        //public virtual ICollection<LimpezaInspecaoItem> LimpezaInspecaoItem { get; set; }
    }
}