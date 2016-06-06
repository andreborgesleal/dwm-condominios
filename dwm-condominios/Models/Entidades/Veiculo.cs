using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("Veiculo")]
    public class Veiculo
    {
        [DisplayName("Condomínio ID")]
        public int CondominioID { get; set; }

        [Key]
        [DisplayName("Placa")]
        public int Placa { get; set; }

        [DisplayName("UnidadeID")]
        public int UnidadeID { get; set; }

        [DisplayName("Condômino ID")]
        public int CondominoID { get; set; }

        [DisplayName("Cor")]
        public string Cor { get; set; }

        [DisplayName("Descrição")]
        public string Descricao { get; set; }

        [DisplayName("Marca")]
        public string Marca { get; set; }

        [DisplayName("Condutor")]
        public string Condutor { get; set; }

        [DisplayName("Número de Série")]
        public string NumeroSerie { get; set; }
    }
}