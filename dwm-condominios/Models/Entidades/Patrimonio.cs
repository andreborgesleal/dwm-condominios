using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DWM.Models.Entidades
{
    [Table("Patrimonio")]
    public class Patrimonio
    {
        [DisplayName("PatrimonioID")]
        public int PatrimonioID { get; set; }

        [DisplayName("PatrimonioClassificacaoID")]
        public int PatrimonioClassificacaoID { get; set; }

        [DisplayName("PatrimonioLocalizacaoID")]
        public int PatrimonioLocalizacaoID { get; set; }

        [DisplayName("credorId")]
        public int? credorId { get; set; }

        [DisplayName("CondominioID")]
        public int CondominioID { get; set; }

        [Required]
        [StringLength(25)]
        [DisplayName("TombamentoID")]
        public string TombamentoID { get; set; }

        [DisplayName("DataTombamento")]
        public DateTime DataTombamento { get; set; }

        [StringLength(200)]
        [DisplayName("Descricao")]
        public string Descricao { get; set; }

        [StringLength(4000)]
        [DisplayName("Observacao")]
        public string Observacao { get; set; }

        [DisplayName("ValorCompra")]
        public decimal? ValorCompra { get; set; }

        [DisplayName("ValorAtual")]
        public decimal? ValorAtual { get; set; }

        [DisplayName("DataBaixa")]
        public DateTime? DataBaixa { get; set; }

        //public virtual Condominio Condominio { get; set; }

        //public virtual Credor Credor { get; set; }

        //public virtual PatrimonioClassificacao PatrimonioClassificacao { get; set; }

        //public virtual PatrimonioLocalizacao PatrimonioLocalizacao { get; set; }
    }
}