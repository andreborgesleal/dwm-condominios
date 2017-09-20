using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("ProprietarioUnidade")]
    public class ProprietarioUnidade
    {
        [Key, Column(Order = 0)]
        [DisplayName("ID")]
        public int ProprietarioID { get; set; }

        [Key, Column(Order = 1)]
        [DisplayName("CondominioID")]
        public int CondominioID { get; set; }

        [Key, Column(Order = 2)]
        [DisplayName("UnidadeID")]
        public int UnidadeID { get; set; }

        [Key, Column(Order = 3)]
        [DisplayName("EdificaçãoID")]
        public int EdificacaoID { get; set; }

        [DisplayName("DataFim")]
        public System.Nullable<System.DateTime> DataFim { get; set; }
    }
}