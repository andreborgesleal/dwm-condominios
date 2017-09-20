using App_Dominio.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DWM.Models.Enumeracoes
{
    public static class Enumeradores
    {
        public enum Param
        {
            GRUPO_USUARIO = 1,
            AREA_ATENDIMENTO = 2,
            SISTEMA = 3,
            EMPRESA = 4,
            HABILITA_EMAIL = 5,
            GRUPO_CREDENCIADO = 6,
            URL_CONDOMINIO = 7,
            EMAIL_TEMPLATE_CHAMADO = 8,
            ESQUECI_MINHA_SENHA = 9,
            GRUPO_PORTARIA = 10,
            EMAIL_TEMPLATE_PORTARIA = 11,
            HABILITA_SMS = 12,
            CHAVE_SMS = 13
        }

        public enum EmailTipo
        {
            [StringDescription("Informativo")]
            [StringValue("1")]
            INFORMATIVO = 1,

            [StringDescription("Cadastro Condômino")]
            [StringValue("2")]
            CADASTRO_CONDOMINO = 2,

            [StringDescription("Cadastro Convite (token)")]
            [StringValue("3")]
            CADASTRO_CONVITE = 3,

            [StringDescription("Cadastro Proprietário")]
            [StringValue("4")]
            CADASTRO_PROPRIETARIO = 4,

            [StringDescription("Cadastro Credenciado")]
            [StringValue("5")]
            CADASTRO_CREDENCIADO = 5,

            [StringDescription("Chamado")]
            [StringValue("6")]
            CHAMADO = 6,

            [StringDescription("Esqueci minha senha")]
            [StringValue("7")]
            FORGOT = 7,

            [StringDescription("Acesso Portaria")]
            [StringValue("8")]
            PORTARIA = 8,

            [StringDescription("Outros")]
            [StringValue("9")]
            OUTROS = 9,

        }

    }
}

