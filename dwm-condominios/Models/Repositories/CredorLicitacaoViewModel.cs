using App_Dominio.Component;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Repositories
{
    public class CredorLicitacaoViewModel : Repository
    {
        [Key]
        [DisplayName("ID")]
        public int LicitacaoID { get; set; }

        [DisplayName("CondominioID")]
        public int CondominioID { get; set; }

        [DisplayName("TipoServicoID")]
        public int TipoServicoID { get; set; }

        [DisplayName("DescricaoTipoServico")]
        public string DescricaoTipoServico { get; set; }

        [DisplayName("credorId")]
        public int? credorId { get; set; }

        [DisplayName("Historico")]
        public string Historico { get; set; }

        [Required]
        [DisplayName("DataEdital")]
        public DateTime DataEdital { get; set; }

        [DisplayName("DataEncerramento")]
        public DateTime? DataEncerramento { get; set; }

        [DisplayName("DataResultado")]
        public DateTime? DataResultado { get; set; }

        [DisplayName("Valor")]
        public decimal Valor { get; set; }

        [DisplayName("Justificativa")]
        public string Justificativa { get; set; }
    }
}