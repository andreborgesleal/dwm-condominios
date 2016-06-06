using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App_Dominio.Component;

namespace DWM.Models.Repositories
{
    public class VeiculoViewModel : Repository
    {
        [DisplayName("Condomínio ID")]
        public int CondominioID { get; set; }

        [DisplayName("Placa")]
        [Required(ErrorMessage = "Informe a Placa do veículo")]
        [StringLength(7, ErrorMessage = "Placa deve ter no máximo 7 caracteres")]
        public int Placa { get; set; }

        [DisplayName("UnidadeID")]
        [Required(ErrorMessage = "Informe a Unidade ID")]
        public int UnidadeID { get; set; }

        [DisplayName("Condômino ID")]
        [Required(ErrorMessage = "Informe o Condômino ID")]
        public int CondominoID { get; set; }

        [DisplayName("Cor")]
        [StringLength(15, ErrorMessage = "Cor deve ter no máximo 15 caracteres")]
        public string Cor { get; set; }

        [DisplayName("Descrição")]
        [StringLength(20, ErrorMessage = "Descrição deve ter no máximo 20 caracteres")]
        public string Descricao { get; set; }

        [DisplayName("Marca")]
        [StringLength(20, ErrorMessage = "Marca deve ter no máximo 20 caracteres")]
        public string Marca { get; set; }
        
        [DisplayName("Condutor")]
        [StringLength(40, ErrorMessage = "Condutor deve ter no máximo 40 caracteres")]
        public string Condutor { get; set; }

        [DisplayName("Número de Série")]
        [StringLength(10, ErrorMessage = "Número de Série deve ter no máximo 10 caracteres")]
        public string NumeroSerie { get; set; }
    }
}