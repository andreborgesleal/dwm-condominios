using App_Dominio.Component;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DWM.Models.Repositories
{
    public class EspacoComumViewModel : Repository
    {
        [DisplayName("EspacoID")]
        public int EspacoID { get; set; }

        [DisplayName("CondominioID")]
        public int CondominioID { get; set; }

        [DisplayName("Descricao")]
        [StringLength(40, ErrorMessage = "Descrição do espaço deve possuir no máximo 40 caracteres")]
        public string Descricao { get; set; }

        [DisplayName("LimitePessoas")]
        public int LimitePessoas { get; set; }

        [DisplayName("Valor")]
        public decimal Valor { get; set; }

    }
}