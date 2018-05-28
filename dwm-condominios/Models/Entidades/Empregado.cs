using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DWM.Models.Entidades
{
    [Table("Empregado")]
    public class Empregado
    {

        [Key]
        [DisplayName("EmpregadoID")]
        public int EmpregadoID { get; set; }

        [DisplayName("CondominioID")]
        public int CondominioID { get; set; }

        [DisplayName("CargoID")]
        public int CargoID { get; set; }

        [Required]
        [StringLength(10)]
        [DisplayName("Matricula")]
        public string Matricula { get; set; }

        [Required]
        [StringLength(40)]
        [DisplayName("Nome")]
        public string Nome { get; set; }

        [StringLength(20)]
        [DisplayName("RG")]
        public string RG { get; set; }

        [StringLength(15)]
        [DisplayName("OrgaoEmissor")]
        public string OrgaoEmissor { get; set; }

        [StringLength(11)]
        [DisplayName("CPF")]
        public string CPF { get; set; }

        [StringLength(11)]
        [DisplayName("Telefone")]
        public string Telefone { get; set; }

        [StringLength(100)]
        [DisplayName("Email")]
        public string Email { get; set; }

        [StringLength(60)]
        [DisplayName("Endereco")]
        public string Endereco { get; set; }

        [StringLength(20)]
        [DisplayName("ComplementoEnd")]
        public string ComplementoEnd { get; set; }

        [StringLength(8)]
        [DisplayName("CEP")]
        public string CEP { get; set; }

        [StringLength(25)]
        [DisplayName("Cidade")]
        public string Cidade { get; set; }

        [StringLength(2)]
        [DisplayName("UF")]
        public string UF { get; set; }

        [DisplayName("DataAdmissao")]
        public DateTime? DataAdmissao { get; set; }

        [DisplayName("DataDesativacao")]
        public DateTime? DataDesativacao { get; set; }

        [StringLength(7)]
        [DisplayName("Dia")]
        public string Dia { get; set; }

        [StringLength(5)]
        [DisplayName("HoraInicial")]
        public string HoraInicial { get; set; }

        [StringLength(5)]
        [DisplayName("HoraFinal")]
        public string HoraFinal { get; set; }
    }
}