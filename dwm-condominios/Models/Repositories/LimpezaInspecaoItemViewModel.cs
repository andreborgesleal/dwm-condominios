using App_Dominio.Component;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DWM.Models.Repositories
{
    public class LimpezaInspecaoItemViewModel : Repository
    {
        [DisplayName("LimpezaInspecaoID")]
        public int LimpezaInspecaoID { get; set; }

        [DisplayName("LimpezaRequisitoID")]
        public int LimpezaRequisitoID { get; set; }

        [DisplayName("Nota")]
        public int Nota { get; set; }

        [StringLength(500)]
        [DisplayName("Justificativa")]
        public string Justificativa { get; set; }
    }
}