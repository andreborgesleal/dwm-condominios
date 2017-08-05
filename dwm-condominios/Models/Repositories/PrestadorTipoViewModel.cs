using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace dwm_condominios.Models.Repositories
{
    public class PrestadorTipoViewModel
    {
        [DisplayName("PrestadorTipoID")]
        public int PrestadorTipoID { get; set; }

        [DisplayName("Descrição")]
        public string Descricao { get; set; }
    }
}