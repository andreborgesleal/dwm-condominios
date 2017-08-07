using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("EspacoComum")]
    public class EspacoComum
    {
        [Key]
        [DisplayName("EspacoID")]
        public int EspacoID { get; set; }

        [DisplayName("CondominioID")]
        public int CondominioID { get; set; }

        [DisplayName("Descricao")]
        public string Descricao { get; set; }

        [DisplayName("LimitePessoas")]
        public int LimitePessoas { get; set; }

        [DisplayName("Valor")]
        public decimal Valor { get; set; }
    }
}