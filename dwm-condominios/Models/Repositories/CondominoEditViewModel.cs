using App_Dominio.Component;
using System.Collections.Generic;

namespace DWM.Models.Repositories
{
    public class CondominoEditViewModel : Repository
    {
        public CondominoPFViewModel CondominoPFViewModel { get; set; }
        public UnidadeViewModel UnidadeViewModel { get; set; }
        public IEnumerable<CredenciadoViewModel> Credenciados{ get; set; }
        public IEnumerable<VeiculoViewModel> Veiculos { get; set; }
        public IEnumerable<FuncionarioViewModel> Funcionarios { get; set; }
        public CredenciadoViewModel CredenciadoViewModel { get; set; }
        public VeiculoViewModel VeiculoViewModel { get; set; }
        public FuncionarioViewModel FuncionarioViewModel { get; set; }
    }
}