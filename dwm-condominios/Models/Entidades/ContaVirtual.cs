using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("ContaVirtual")]
    public class ContaVirtual
    {
        [Key]
        [DisplayName("ID")]
        public int ContaVirtualID { get; set; }

        [DisplayName("MembroID")]
        public int MembroID { get; set; }

        [DisplayName("TipoContaID")]
        public int TipoContaID { get; set; }

        [DisplayName("DataAbertura")]
        public System.DateTime DataAbertura { get; set; }

        [DisplayName("DataEncerramento")]
        public System.Nullable<System.DateTime> DataEncerramento{ get; set; }

        [DisplayName("Saldo")]
        public System.Decimal ValorSaldo { get; set; }

        public virtual Membro Membro { get; set; }
    }
}