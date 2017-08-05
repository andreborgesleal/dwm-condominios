using App_Dominio.Component;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DWM.Models.Repositories
{
    public class VisitanteAcessoUnidadeViewModel : Repository
    {
        [DisplayName("AcessoID")]
        public int AcessoID { get; set; }

        [DisplayName("CondominioID")]
        [Required]
        public int CondominioID { get; set; }

        [DisplayName("EdificacaoID")]
        [Required]
        public int EdificacaoID { get; set; }

        [DisplayName("UnidadeID")]
        [Required]
        public int UnidadeID { get; set; }

        [DisplayName("CondominoID")]
        [Required]
        public int CondominoID { get; set; }

        [DisplayName("CredenciadoID")]
        public System.Nullable<int> CredenciadoID { get; set; }

        public virtual VisitanteAcessoViewModel VisitanteAcessoViewModel { get; set; }

    }
}