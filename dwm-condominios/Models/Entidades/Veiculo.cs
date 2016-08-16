using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("Veiculo")]
    public class Veiculo
    {
        [Key, Column(Order = 0)]
        [DisplayName("CondominioID")]
        public int CondominioID { get; set; }

        [Key, Column(Order = 1)]
        [DisplayName("EdificacaoID")]
        public int EdificacaoID { get; set; }

        [Key, Column(Order = 2)]
        [DisplayName("UnidadeID")]
        public int UnidadeID { get; set; }

        [Key, Column(Order = 3)]
        [DisplayName("Condomino ID")]
        public int CondominoID { get; set; }

        [Key, Column(Order = 4)]
        [DisplayName("Placa")]
        public string Placa { get; set; }

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