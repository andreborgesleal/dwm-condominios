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
    public class LimpezaRequisitoViewModel : Repository
    {
        [DisplayName("LimpezaRequisitoID")]
        public int LimpezaRequisitoID { get; set; }

        [DisplayName("EspacoID")]
        public int EspacoID { get; set; }

        [Required]
        [StringLength(80)]
        [DisplayName("Descricao")]
        public string Descricao { get; set; }

        public virtual EspacoComum EspacoComum { get; set; }
    }
}