using App_Dominio.Component;
using DWM.Models.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DWM.Models.Repositories
{
    public class VisitanteViewModel : Repository
    {
        public VisitanteViewModel()
        {
            VisitanteUnidadeViewModel = new HashSet<VisitanteUnidadeViewModel>();
        }

        [DisplayName("VisitanteID")]
        public int VisitanteID { get; set; }

        [DisplayName("CondominioID")]
        [Required]
        public int CondominioID { get; set; }

        [DisplayName("PrestadorTipoID")]
        public int? PrestadorTipoID { get; set; }

        [DisplayName("Nome")]
        [Required]
        [StringLength(40, ErrorMessage = "Nome do visitante/prestador deve possuir no máximo 40 caracteres")]
        public string Nome { get; set; }

        [DisplayName("RG")]
        [StringLength(20, ErrorMessage = "RG deve possuir no máximo 20 caracteres")]
        public string RG { get; set; }

        [DisplayName("OrgaoEmissor")]
        [StringLength(15, ErrorMessage = "Órgão Emissor deve possuir no máximo 15 caracteres")]
        public string OrgaoEmissor { get; set; }

        [DisplayName("CPF")]
        //[StringLength(11)]
        public string CPF { get; set; }

        [DisplayName("Sexo")]
        [StringLength(1)]
        [Required]
        public string Sexo { get; set; }

        [DisplayName("DataInclusao")]
        [Required]
        public DateTime DataInclusao { get; set; }

        [DisplayName("Telefone")]
        //[StringLength(11)]
        public string Telefone { get; set; }

        [DisplayName("Fotografia")]
        [StringLength(100)]
        public string Fotografia { get; set; }

        [DisplayName("Prestador Condomínio")]
        [StringLength(1)]
        [Required]
        public string PrestadorCondominio { get; set; }

        [DisplayName("Situação")]
        [StringLength(1)]
        public string Situacao { get; set; }

        [DisplayName("Placa")]
        [StringLength(7, ErrorMessage = "Placa do veículo deve possuir no máximo 7 caracteres")]
        public string Placa { get; set; }

        [DisplayName("Cor")]
        [StringLength(15, ErrorMessage = "Cor do veículo deve possuir no máximo 15 caracteres")]
        public string Cor { get; set; }

        [DisplayName("Descrição")]
        [StringLength(20, ErrorMessage = "Descrição do veículo deve possuir no máximo 20 caracteres")]
        public string Descricao { get; set; }

        [DisplayName("Marca")]
        [StringLength(20, ErrorMessage = "Marca do veículo deve possuir no máximo 20 caracteres")]
        public string Marca { get; set; }

        #region Atributos de Outras Tabelas
        public string DescricaoEdificacao { get; set; }

        public string DescricaoTipoPrestador { get; set; }

        public int? EdificacaoID { get; set; }

        public int? UnidadeID { get; set; }

        public string NomeCondomino { get; set; }

        //public virtual VisitanteAcessoViewModel VisitanteAcessoViewModel { get; set; }

        public virtual IEnumerable<VisitanteUnidadeViewModel> VisitanteUnidadeViewModel { get; set; }
        #endregion
    }
}