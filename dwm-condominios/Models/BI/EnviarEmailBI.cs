using System;
using System.Collections.Generic;
using App_Dominio.Contratos;
using App_Dominio.Entidades;
using App_Dominio.Component;
using DWM.Models.Entidades;
using System.Web.Mvc;
using App_Dominio.Enumeracoes;
using App_Dominio.Security;
using App_Dominio.Models;
using System.Net.Mail;

namespace DWM.Models.BI
{
    public class EnviarEmailBI : DWMContext<ApplicationContext>, IProcess<RegisterViewModel, ApplicationContext>
    {
        #region Constructor
        public EnviarEmailBI() { }

        public void Create(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.db = _db;
            this.seguranca_db = _seguranca_db;
        }

        public EnviarEmailBI(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            Create(_db, _seguranca_db);
        }
        #endregion

        public RegisterViewModel Run(Repository value)
        {
            string habilitaEmail = System.Configuration.ConfigurationManager.AppSettings["HabilitaEmail"];
            RegisterViewModel rec = (RegisterViewModel)value;

            if (habilitaEmail == "S")
            {
                int _empresaId = int.Parse(System.Configuration.ConfigurationManager.AppSettings["empresaId"]);
                int _sistemaId = int.Parse(System.Configuration.ConfigurationManager.AppSettings["sistemaId"]);
                string email_admin = System.Configuration.ConfigurationManager.AppSettings["email_admin"];
                rec.empresaId = _empresaId;

                SendEmail sendMail = new SendEmail();

                Empresa empresa = seguranca_db.Empresas.Find(rec.empresaId);
                Sistema sistema = seguranca_db.Sistemas.Find(_sistemaId);

                MailAddress sender = new MailAddress(empresa.nome + " <" + empresa.email + ">");
                List<string> recipients = new List<string>();

                recipients.Add(rec.Nome + "<" + rec.Email + ">");

                string Subject = "Confirmação de cadastro no " + empresa.nome;
                string Text = "<p>Confirmação de cadastro</p>";
                string Html = "<p><span style=\"font-family: Verdana; font-size: larger; color: #656464\">" + sistema.descricao + "</span></p>" +
                              "<p><span style=\"font-family: Verdana; font-size: xx-large; color: #0094ff\">" + rec.Nome.ToUpper() + "</span></p>" +
                              "<p></p>" +
                              "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">Telefone: <b>" + Funcoes.FormataTelefone(rec.Telefone) + "</b></span></p>" +
                              "<p></p>" +
                              "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">CPF: <b>" + Funcoes.FormataCPF(rec.CPF) + "</b></span></p>" +
                              "<p></p>" +
                              "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">Essa é uma mensagem de confirmação de seu cadastro. Seu registro no Sistema Administrativo do " + empresa.nome + " foi realizado com sucesso.</span></p>" +
                              "<p></p>" +
                              "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">Clique no link abaixo para ativar seu cadastro:</span></p>" +
                              "<p><a href=\"http://localhost:64972/Account/Ativar/" + rec.Keyword + "\" target=\"_blank\"><span style=\"font-family: Verdana; font-size: small; color: #0094ff\">Ativação do cadastro no Sopro da Sorte</span></a></p>" +
                              "<p></p>" +
                              "<p></p>";

                string asterisco = "";
                for (int i = 1; i <= rec.senha.Length - 1; i++)
                    asterisco += "*";

                Html += "<p></p>" +
                        "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">Seu Login de acesso é: </span></p>" +
                        "<p></p>" +
                        "<p><span style=\"font-family: Verdana; font-size: xx-large; color: #0094ff\">" + rec.Email + "</span></p>" +
                        "<p></p>" +
                        "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">Sua senha informada no cadastro é: </span></p>" +
                        "<p></p>" +
                        "<p><span style=\"font-family: Verdana; font-size: xx-large; color: #0094ff\">" + rec.senha.Substring(0, 1) + asterisco + "</span></p>" +
                        "<hr />";

                Html += "<p></p>" +
                        "<p></p>" +
                        "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">Através do sistema o apostador poderá:</span></p>" +
                        "<p></p>" +
                        "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">- Consultar sua conta corrente virtual com os débitos e créditos realizados.</span></p>" +
                        "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">- Consultar sua participação percentual em cada concurso nos bolões estruturados pela DWM Sistemas.</span></p>" +
                        "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">- Consultar extrato de mensaldiades.</span></p>" +
                        "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">- Simular e comprar cotas de cada concurso.</span></p>" +
                        "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">- Atualizar seu cadastro (foto, conta corrente, etc).</span></p>" +
                        "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">- Receber mensagens e alertas personalizados.</span></p>" +
                        "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">- Consultar o resultado dos sorteios e conferir os volantes premiados.</span></p>" +
                        "<hr />" +
                        "<p></p>" +
                        "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">Além desses recursos, estaremos implementando outras novidades. Aguarde !</span></p>" +
                        "<p>&nbsp;</p>" +
                        "<p>&nbsp;</p>" +
                        "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">Obrigado,</span></p>" +
                        "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">Administração " + empresa.nome + "</span></p>";

                Validate result = sendMail.Send(sender, recipients, Html, Subject, Text);
                if (result.Code > 0)
                {
                    result.MessageBase = "Seu cadastro foi realizado com sucesso, mas por falhas de comunicação não foi possível enviar seu e-mail de confirmação. Favor entrar em contato com faleconosco@dwmsisteamas.com e solicite seu e-mail de ativação.";
                    throw new App_DominioException(result);
                }

            }
            rec.mensagem = new Validate() { Code = -1, Message = MensagemPadrao.Message(0).ToString(), MessageType = MsgType.SUCCESS };
            return rec; 
        }

        public IEnumerable<RegisterViewModel> List(params object[] param)
        {
            throw new NotImplementedException();
        }

        public IPagedList PagedList(int? index, int pageSize = 50, params object[] param)
        {
            throw new NotImplementedException();
        }

    }
}