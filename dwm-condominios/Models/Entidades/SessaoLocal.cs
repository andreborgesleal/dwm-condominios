using App_Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DWM.Models.Entidades
{
    public class SessaoLocal : Sessao
    {
        public int CondominoID { get; set; }
        public int? CredenciadoID { get; set; }
        public int[] GrupoCondominoID { get; set; }
        public IEnumerable<Unidade> Unidades { get; set; }
        public int? FilaFornecedorID { get; set; }
    }
}