using App_Dominio.Component;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace DWM.Models.Repositories
{
    public class VisitanteUnidadeViewModel : Repository
    {
        [DisplayName("VisitanteID")]
        public int VisitanteID { get; set; }

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