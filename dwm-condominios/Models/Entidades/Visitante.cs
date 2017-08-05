using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace DWM.Models.Entidades
{
    [Table("Visitante")]
    public class Visitante
    {
        public Visitante()
        {
            VisitanteUnidade = new HashSet<VisitanteUnidade>();
        }

        [Key]
        [DisplayName("VisitanteID")]
        public int VisitanteID { get; set; }

        [DisplayName("CondominioID")]
        public int CondominioID { get; set; }

        [DisplayName("PrestadorTipoID")]
        public int? PrestadorTipoID { get; set; }

        [DisplayName("Nome")]
        public string Nome { get; set; }

        [DisplayName("RG")]
        public string RG { get; set; }

        [DisplayName("OrgaoEmissor")]
        public string OrgaoEmissor { get; set; }

        [DisplayName("CPF")]
        public string CPF { get; set; }

        [DisplayName("Sexo")]
        public string Sexo { get; set; }

        [DisplayName("DataInclusao")]
        public DateTime DataInclusao { get; set; }

        [DisplayName("Telefone")]
        public string Telefone { get; set; }

        [DisplayName("Fotografia")]
        public string Fotografia { get; set; }

        [DisplayName("PrestadorCondominio")]
        public string PrestadorCondominio { get; set; }

        [DisplayName("Situacao")]
        public string Situacao { get; set; }

        [DisplayName("Placa")]
        public string Placa { get; set; }

        [DisplayName("Cor")]
        public string Cor { get; set; }

        [DisplayName("Descricao")]
        public string Descricao { get; set; }

        [DisplayName("Marca")]
        public string Marca { get; set; }

        public virtual ICollection<VisitanteUnidade> VisitanteUnidade { get; set; }
    }
}