using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace DWM.Models.Entidades
{
    [Table("PatrimonioClassificacao")]
    public class PatrimonioClassificacao
    {
        //public PatrimonioClassificacao()
        //{
        //    Patrimonio = new List<Patrimonio>();
        //}

        [DisplayName("PatrimonioClassificacaoID")]
        public int PatrimonioClassificacaoID { get; set; }

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