using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DWM.Models.Entidades
{
    [Table("PatrimonioLocalizacao")]
    public class PatrimonioLocalizacao
    {
        //public PatrimonioLocalizacao()
        //{
        //    Patrimonio = new List<Patrimonio>();
        //}

        [DisplayName("PatrimonioLocalizacaoID")]
        public int PatrimonioLocalizacaoID { get; set; }

        [DisplayName("CondominioID")]
        public int CondominioID { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Descricao")]
        public string Descricao { get; set; }

        //public virtual Condominio Condominio { get; set; }

        //public virtual ICollection<Patrimonio> Patrimonio { get; set; }
    }
}