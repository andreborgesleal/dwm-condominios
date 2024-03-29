﻿using App_Dominio.Component;
using App_Dominio.Entidades;
using App_Dominio.Security;
using DWM.Models.Entidades;
using DWM.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace DWM.Models.Repositories
{
    public class VisitanteViewModel : Repository, IPathArquivos
    {
        public VisitanteViewModel()
        {
            VisitanteUnidadeViewModel = new List<VisitanteUnidadeViewModel>();
        }

        #region IPathArquivos
        public string getAvatar
        {
            get
            {
                return Avatar();
            }
        }

        private static string URL;

        public string GetURL()
        {
            if (URL == null || URL.Trim() == "")
            {
                if (empresaId == 0)
                {
                    EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
                    Sessao sessaoCorrente = security.getSessaoCorrente(sessionId);
                    if (sessaoCorrente == null)
                        return "";
                    empresaId = sessaoCorrente.empresaId;
                }

                using (var db = new ApplicationContext())
                    return db.Parametros.Find(empresaId, (int)Models.Enumeracoes.Enumeradores.Param.URL_CONDOMINIO).Valor;
            }

            return URL;
        }

        public void SetURL(string _URL)
        {
            URL = _URL;
        }

        public string Path()
        {
            if (empresaId == 0)
            {
                EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
                Sessao sessaoCorrente = security.getSessaoCorrente(sessionId);
                if (sessaoCorrente == null)
                    return "";
                empresaId = sessaoCorrente.empresaId;
            }

            return URL + "/Users_Data/Empresas/" + empresaId.ToString() + "/Visitante/";
        }

        public string Extension(string FileID)
        {
            System.IO.FileInfo f = new System.IO.FileInfo(System.IO.Path.Combine(Path(), FileID));
            return f.Extension;
        }

        public string Avatar(string size = "30")
        {
            this.SetURL(this.GetURL());

            if (VisitanteID == 0 || Fotografia == null || Fotografia.Trim() == "")
                return "http://api.ning.com/files/XDvieCk-6Hj1PFXyHT13r7Et-ybLOKWFR9fYd15dBrqFQHv6gCVuGdr4GYjaO0u*h2E0p*c5ZVHE-H41wNz4uAGNfcH8LLZS/top_8_silhouette_male_120.jpg?width=" + size;

            if (empresaId == 0)
            {
                EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
                Sessao sessaoCorrente = security.getSessaoCorrente(sessionId);
                if (sessaoCorrente == null)
                    return "http://api.ning.com/files/XDvieCk-6Hj1PFXyHT13r7Et-ybLOKWFR9fYd15dBrqFQHv6gCVuGdr4GYjaO0u*h2E0p*c5ZVHE-H41wNz4uAGNfcH8LLZS/top_8_silhouette_male_120.jpg?width=" + size;
                empresaId = sessaoCorrente.empresaId;
            }

            FileInfo f = new FileInfo(HttpContext.Current.Server.MapPath("~/Users_Data/Empresas/" + empresaId.ToString() + "/Visitante/" + Fotografia));
            if (f.Exists)
                return URL + "/Users_Data/Empresas/" + empresaId.ToString() + "/Visitante/" + Fotografia;
            else
                return "http://api.ning.com/files/XDvieCk-6Hj1PFXyHT13r7Et-ybLOKWFR9fYd15dBrqFQHv6gCVuGdr4GYjaO0u*h2E0p*c5ZVHE-H41wNz4uAGNfcH8LLZS/top_8_silhouette_male_120.jpg?width=" + size;
        }
        #endregion

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

        [DisplayName("E-mail")]
        [StringLength(100, ErrorMessage = "E-mail do visitante/prestador deve possuir no máximo 100 caracteres")]
        public string Email { get; set; }

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
        //[StringLength(7, ErrorMessage = "Placa do veículo deve possuir no máximo 7 caracteres")]
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

        public VisitanteAcessoViewModel VisitanteAcessoViewModel { get; set; }

        public virtual IEnumerable<VisitanteUnidadeViewModel> VisitanteUnidadeViewModel { get; set; }

        //public virtual IEnumerable<VisitanteAcessoViewModel> VisitanteAcessosViewModel { get; set; }

        #endregion
    }
}