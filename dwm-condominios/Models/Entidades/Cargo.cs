using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DWM.Models.Entidades
{
    [Table("Cargo")]
    public class Cargo
    {
        //public Cargo()
        //{
        //    Empregado = new List<Empregado>();
        //}

        [Key]
        [DisplayName("CargoID")]
        public int CargoID { get; set; }

        [Required]
        [StringLength(30)]
        [DisplayName("Descricao")]
        public string Descricao { get; set; }

        [Required]
        [StringLength(1)]
        [DisplayName("Situacao")]
        public string Situacao { get; set; }

        [DisplayName("CondominioID")]
        public int CondominioID { get; set; }

        //[DisplayName("Empregado")]
        //public virtual ICollection<Empregado> Empregado { get; set; }

    }
}