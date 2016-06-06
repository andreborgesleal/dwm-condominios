﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("Condominio")]
    public class Condominio
    {
        [Key]
        [DisplayName("ID")]
        public int CondominioID { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Razão Social")]
        public string RazaoSocial { get; set; }

        [DisplayName("CNPJ")]
        public string CNPJ { get; set; }

        [DisplayName("Endereço")]
        public string Endereco { get; set; }

        [DisplayName("Complemento")]
        public string Complemento { get; set; }

        [DisplayName("CEP")]
        public string CEP { get; set; }

        [DisplayName("Logo")]
        public string Logo { get; set; }

        [DisplayName("Path Logo")]
        public string PathLogo { get; set; }

        [DisplayName("Latitude")]
        public string Latitude { get; set; }

        [DisplayName("Longitude")]
        public string Longitudode { get; set; }

        [DisplayName("Indicador Tipo Condomino")]
        public string IndTipoCondomino { get; set; }
    }
}