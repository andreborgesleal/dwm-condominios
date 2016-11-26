using App_Dominio.Component;
using DWM.Models.Repositories;
using System.Collections.Generic;

namespace DWM.Models.Repositories
{
    public class FilaAtendimentoUsuarioEditViewModel : Repository
    {
        public FilaAtendimentoUsuarioViewModel FilaAtendimentoUsuarioViewModel { get; set; }
        public IEnumerable<FilaAtendimentoUsuarioViewModel> FilaAtendimentoUsuarios { get; set; }
    }
}