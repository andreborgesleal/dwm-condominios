using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App_Dominio.Component;

namespace DWM.Models.Repositories
{
    public class VeiculoViewModel : Repository
    {
        [DisplayName("Condomínio")]
        [Required(ErrorMessage ="Condomínio deve ser informado")]
        public int CondominioID { get; set; }

        [DisplayName("Edificação")]
        [Required(ErrorMessage = "Edificação deve ser informada")]
        public int EdificacaoID { get; set; }

        [DisplayName("Unidade")]
        [Required(ErrorMessage = "Unidade deve ser informada")]
        public int UnidadeID { get; set; }

        [DisplayName("Condôminio")]
        [Required(ErrorMessage = "Condômino deve ser informado")]
        public int CondominoID { get; set; }

        [DisplayName("Placa")]
        [Required(ErrorMessage = "Placa do automóvel deve ser informada")]
        [StringLength(7, MinimumLength = 7, ErrorMessage = "Placa deve ter 7 caracteres")]
        public string Placa { get; set; }

        [DisplayName("Cor")]
        [Required(ErrorMessage = "Cor do automóvel deve ser informada")]
        [StringLength(15, ErrorMessage = "Cor deve ter no máximo 15 caracteres")]
        public string Cor { get; set; }

        [DisplayName("Descrição")]
        [Required(ErrorMessage = "Descrição do automóvel deve ser informada")]
        [StringLength(20, ErrorMessage = "Descrição deve ter no máximo 20 caracteres")]
        public string Descricao { get; set; }

        [DisplayName("Marca")]
        [Required(ErrorMessage = "Marca do automóvel deve ser informada")]
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