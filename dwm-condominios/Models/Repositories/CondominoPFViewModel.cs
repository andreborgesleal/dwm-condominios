using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System;

namespace DWM.Models.Repositories
{
    public class CondominoPFViewModel : CondominoViewModel
    {
        [DisplayName("Data de Nascimento")]
        public DateTime? DataNascimento { get; set; }
        
        [DisplayName("Sexo")]
        [Required(ErrorMessage = "Informe o Sexo")]
        public string Sexo { get; set; }

        [DisplayName("Indicador Animal")]
        [Required(ErrorMessage = "Informe se o Condômino possui animal de estimação")]
        public string IndAnimal { get; set; }
    }
}