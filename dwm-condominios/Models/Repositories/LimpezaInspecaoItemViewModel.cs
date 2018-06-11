using App_Dominio.Component;
using DWM.Models.Entidades;
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
        public int? Nota { get; set; }

        [StringLength(500)]
        [DisplayName("Justificativa")]
        public string Justificativa { get; set; }

        [DisplayName("Item")]
        public string ItemDescricao { get; set; }

        [DisplayName("EspacoDescricao")]
        public string EspacoDescricao { get; set; }

        [DisplayName("FornecedorDescricao")]
        public string FornecedorDescricao { get; set; }

        [DisplayName("DataInspecao")]
        public DateTime DataInspecao { get; set; }

    }
}