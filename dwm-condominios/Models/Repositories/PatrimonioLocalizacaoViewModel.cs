using App_Dominio.Component;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DWM.Models.Repositories
{
    public class PatrimonioLocalizacaoViewModel : Repository
    {
        [DisplayName("PatrimonioLocalizacaoID")]
        public int PatrimonioLocalizacaoID { get; set; }

        [DisplayName("CondominioID")]
        public int CondominioID { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Descrição")]
        public string Descricao { get; set; }
    }
}