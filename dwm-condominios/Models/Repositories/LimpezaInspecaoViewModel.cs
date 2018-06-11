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
    public class LimpezaInspecaoViewModel : Repository
    {
        public LimpezaInspecaoViewModel()
        {
            this.LimpezaInspecaoItem = new List<LimpezaInspecaoItemViewModel>();
        }

        [DisplayName("LimpezaInspecaoID")]
        public int LimpezaInspecaoID { get; set; }

        [DisplayName("credorId")]
        public int credorId { get; set; }


        [DisplayName("Fornecedor")]
        public string FornecedorDescricao { get; set; }

        [DisplayName("EspacoID")]
        public int EspacoID { get; set; }

        [DisplayName("Espaço")]
        public string EspacoDescricao { get; set; }

        [DisplayName("DataInspecao")]
        public DateTime DataInspecao { get; set; }

        [DisplayName("NotaFinal")]
        public int? NotaFinal { get; set; }

        [StringLength(4000)]
        [DisplayName("Laudo")]
        public string Laudo { get; set; }

        [Required]
        [DisplayName("CondominioID")]
        public int CondominioID { get; set; }

        public IEnumerable<LimpezaInspecaoItemViewModel> LimpezaInspecaoItem { get; set; }

    }
}