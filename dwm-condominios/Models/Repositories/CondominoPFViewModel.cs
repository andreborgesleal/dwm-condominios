using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App_Dominio.Component;
using DWM.Models.Repositories;
using System;

namespace DWM.Models.Repositories
{
    public class CondominoPFViewModel : CondominoViewModel
    {
        [DisplayName("Data de Nascimento")]
        public DateTime? DataNascimento { get; set; }
        
        [DisplayName("Sexo")]
        [StringLength(1, ErrorMessage = "Este campo só permite até 1 caracteres")]
        public string Sexo { get; set; }

        [DisplayName("Indicador Animal")]
        [StringLength(1, ErrorMessage = "Este campo só permite até 1 caracteres")]
        public string IndAnimal { get; set; }
    }
}