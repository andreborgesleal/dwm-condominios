using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfCondominiosService.Models
{
    public class Auth
    {
        public int Code { get; set; }
        public string Mensagem { get; set; }
        public string Token { get; set; }
    }
}