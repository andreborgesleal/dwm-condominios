using DWM.Models.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dwm_condominios.Models.API
{
    public class UsuarioLogadoToken : Auth
    {
        public int EdificacaoID { get; set; }
        public int UnidadeID { get; set; }
        public string Descricao { get; set; }
    }
}