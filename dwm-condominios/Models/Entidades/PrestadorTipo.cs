using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DWM.Models.Entidades
{
    [Table("PrestadorTipo")]
    public class PrestadorTipo
    {
        [Key]
        [DisplayName("PrestadorTipoID")]
        public int PrestadorTipoID { get; set; }

        [DisplayName("Descricao")]
        public string Descricao { get; set; }
    }
}