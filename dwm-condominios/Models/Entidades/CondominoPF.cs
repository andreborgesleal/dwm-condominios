using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System;

namespace DWM.Models.Entidades
{
    public class CondominoPF : Condomino
    {
        [DisplayName("ProfissaoID")]
        public int ProfissaoID { get; set; }

        [DisplayName("DataNascimento")]
        public DateTime? DataNascimento { get; set; }

        [DisplayName("Sexo")]
        public string Sexo { get; set; }

        [DisplayName("IndicadorAnimal")]
        public string IndAnimal { get; set; }
    }
}