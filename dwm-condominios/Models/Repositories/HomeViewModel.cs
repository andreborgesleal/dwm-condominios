using App_Dominio.Component;
using System;
using System.Collections.Generic;


namespace DWM.Models.Repositories
{
    public class HomeViewModel : Repository
    {
        public int? concursoId { get; set; }
        public DateTime? data_ini { get; set; }
        public int membroId { get; set; }
        public string isAdmin { get; set; }
        public string nome { get; set; }
        public IEnumerable<InformativoViewModel> Informativos { get; set; }

        //public IEnumerable<PanoramaViewModel> Panorama { get; set; }
        //public IEnumerable<MovimentoViewModel> Movimento { get; set; }
        //public IEnumerable<ConcursoViewModel> Concurso { get; set; }
        //public IEnumerable<VolanteViewModel> Volante { get; set; }
        //public ApostaViewModel apostaViewModel { get; set; }
        //public IEnumerable<MovimentoViewModel> Extrato { get; set; }
        //public string NumerosApostados { get; set; }
        //public IEnumerable<MensalidadeViewModel> Mensalidades { get; set; }
    }
}