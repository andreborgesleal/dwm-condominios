using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System;

namespace DWM.Models.Repositories
{
    public class CondominoPFViewModel : CondominoViewModel
    {
        [DisplayName("ProfissaoID")]
        public int? ProfissaoID { get; set; }

        [DisplayName("Data de Nascimento")]
        public DateTime? DataNascimento { get; set; }
        
        [DisplayName("Sexo")]
        public string Sexo { get; set; }

        [DisplayName("Animal de estimação")]
        public string IndAnimal { get; set; }
    }
}