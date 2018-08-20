using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("AluguelEspaco")]
    public class AluguelEspaco
    {
        //public AluguelEspaco()
        //{
        //    VisitanteAcessos = new List<VisitanteAcesso>();
        //}

        [Key]
        [DisplayName("AluguelID")]
        public int AluguelID { get; set; }

        [DisplayName("EspacoID")]
        public int EspacoID { get; set; }

        [DisplayName("CondominioID")]
        public int CondominioID { get; set; }

        [DisplayName("EdificacaoID")]
        public int EdificacaoID { get; set; }

        [DisplayName("UnidadeID")]
        public int UnidadeID { get; set; }

        [DisplayName("CondominoID")]
        public int CondominoID { get; set; }

        [DisplayName("CredenciadoID")]
        public System.Nullable<int> CredenciadoID { get; set; }

        [DisplayName("DataEvento")]
        [DataType(DataType.DateTime)]
        public DateTime DataEvento { get; set; }

        [DisplayName("DataReserva")]
        [DataType(DataType.DateTime)]
        public DateTime DataReserva { get; set; }

        [DisplayName("DataAutorizacao")]
        public System.Nullable<DateTime> DataAutorizacao { get; set; }

        [DisplayName("DataRevogacao")]
        public System.Nullable<DateTime> DataRevogacao { get; set; }

        [DisplayName("DataCancelamento")]
        public System.Nullable<DateTime> DataCancelamento { get; set; }

        [DisplayName("Valor")]
        public decimal Valor { get; set; }

        [DisplayName("Observacao")]
        public string Observacao { get; set; }

        #region Virtuals
        //public virtual Condominio Condominio { get; set; }

        //public virtual CondominoUnidade CondominoUnidade { get; set; }

        //public virtual Credenciado Credenciado { get; set; }

        //public virtual EspacoComum EspacoComum { get; set; }

        //public virtual ICollection<VisitanteAcesso> VisitanteAcessos { get; set; }
        #endregion

    }
}