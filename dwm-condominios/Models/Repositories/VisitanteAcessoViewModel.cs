﻿using App_Dominio.Component;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DWM.Models.Repositories
{
    public class VisitanteAcessoViewModel : Repository
    {
        [DisplayName("AcessoID")]
        public int AcessoID { get; set; }

        [DisplayName("VisitanteID")]
        [Required]
        public int VisitanteID { get; set; }

        [DisplayName("CondominioID")]
        [Required]
        public int CondominioID { get; set; }

        [DisplayName("DataInclusao")]
        [Required]
        public DateTime DataInclusao { get; set; }

        [DisplayName("DataAutorizacao")]
        [Required]
        public DateTime DataAutorizacao { get; set; }

        [DisplayName("HoraInicio")]
        [StringLength(5)]
        public string HoraInicio { get; set; }

        [DisplayName("HoraLimite")]
        [StringLength(5)]
        public string HoraLimite { get; set; }

        [DisplayName("DataAcesso")]
        public DateTime? DataAcesso { get; set; }
    }
}