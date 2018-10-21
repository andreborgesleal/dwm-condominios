using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DWM.Models.Entidades
{
    [Table("CredorTipoServico")]
    public class CredorTipoServico
    {
        [Key]
        [DisplayName("ID")]
        public int TipoServicoID { get; set; }

        [DisplayName("CondominioID")]
        public int CondominioID { get; set; }

        [DisplayName("Descricao")]
        public string Descricao { get; set; }

        [DisplayName("Situacao")]
        public string Situacao { get; set; }
    }
}