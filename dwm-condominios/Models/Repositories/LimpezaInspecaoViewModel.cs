using App_Dominio.Component;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DWM.Models.Repositories
{
    public class LimpezaInspecaoViewModel : Repository
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
    }
}