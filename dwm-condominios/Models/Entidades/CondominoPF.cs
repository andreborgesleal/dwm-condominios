using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System;

namespace DWM.Models.Entidades
{
    public class CondominoPF : Condomino
    {
        [DisplayName("Data de Nascimento")]
        public DateTime? DataNascimento { get; set; }

        [StringLength(1, ErrorMessage = "Este campo só permite até 1 caracteres")]
        [DisplayName("Sexo")]
        public string Sexo { get; set; }

        [StringLength(1, ErrorMessage = "Este campo só permite até 1 caracteres")]
        [DisplayName("Indicador Animal")]
        public string IndAnimal { get; set; }
    }
}