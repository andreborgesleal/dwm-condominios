using App_Dominio.Component;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DWM.Models.Repositories
{

    public class CargoViewModel : Repository
    {
        //public CargoViewModel()
        //{
        //    EmpregadoViewModel = new List<EmpregadoViewModel>();
        //}

        [DisplayName("CargoID")]
        public int CargoID { get; set; }

        [Required]
        [StringLength(30)]
        [DisplayName("Descrição")]
        public string Descricao { get; set; }

        [Required]
        [StringLength(1)]
        [DisplayName("Situação")]
        public string Situacao { get; set; }

        [DisplayName("CondominioID")]
        public int CondominioID { get; set; }

        //[DisplayName("EmpregadoViewModel")]
        //public virtual ICollection<EmpregadoViewModel> EmpregadoViewModel { get; set; }

    }
}