using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DWM.Models.Entidades
{
    [Table("VisitanteAcessoUnidade")]
    public class VisitanteAcessoUnidade
    {
        [Key]
        [DisplayName("AcessoID")]
        public int AcessoID { get; set; }

        [DisplayName("CondominioID")]
        public int CondominioID { get; set; }

        [DisplayName("EdificacaoID")]
        public int EdificacaoID { get; set; }

        [DisplayName("UnidadeID")]
        public int UnidadeID { get; set; }

        [DisplayName("CondominoID")]
        public int CondominoID { get; set; }
    }
}