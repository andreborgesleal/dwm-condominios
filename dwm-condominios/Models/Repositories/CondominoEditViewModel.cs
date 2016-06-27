using App_Dominio.Component;
using System.Collections.Generic;

namespace DWM.Models.Repositories
{
    public class CondominoEditViewModel : Repository
    {
        public CondominoPFViewModel CondominoPFViewModel { get; set; }
        public UnidadeViewModel UnidadeViewModel { get; set; }
        public IEnumerable<CredenciadoViewModel> Credenciados{ get; set; }
    }
}