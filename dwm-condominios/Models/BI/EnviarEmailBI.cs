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
using DWM.Models.Repositories;
using System.Linq;

namespace DWM.Models.BI
{
    public class EnviarEmailBI : DWMContextLocal, IProcess<RegisterViewModel, ApplicationContext>
    {
        #region Constructor
        public EnviarEmailBI() { }

        public EnviarEmailBI(ApplicationContext _db, SecurityContext _segurancaDb)
        {
            this.Create(_db, _segurancaDb);
        }

        #endregion

        public RegisterViewModel Run(Repository value)
        {
            RegisterViewModel rec = (RegisterViewModel)value;
            string habilitaEmail = db.Parametros.Find(rec.CondominioID, (int)Enumeracoes.Enumeradores.Param.HABILITA_EMAIL).Valor;
            if (habilitaEmail == "S")
            {
                int _empresaId = int.Parse(db.Parametros.Find(rec.CondominioID, (int)Enumeracoes.Enumeradores.Param.EMPRESA).Valor);
                int _sistemaId = int.Parse(db.Parametros.Find(rec.CondominioID, (int)Enumeracoes.Enumeradores.Param.SISTEMA).Valor);

                Condominio condominio = db.Condominios.Find(rec.CondominioID);
                Sistema sistema = seguranca_db.Sistemas.Find(_sistemaId);

                rec.empresaId = _empresaId;

                SendEmail sendMail = new SendEmail();

                MailAddress sender = new MailAddress(condominio.RazaoSocial + " <" + condominio.Email + ">");
                List<string> recipients = new List<string>();

                recipients.Add(rec.Nome + "<" + rec.Email + ">");

                string Subject = "Confirmação de cadastro no " + condominio.RazaoSocial;
                string Text = "<p>Confirmação de cadastro</p>";
                string Html = "<p><span style=\"font-family: Verdana; font-size: larger; color: #656464\">" + sistema.descricao + "</span></p>" +
                              "<p><span style=\"font-family: Verdana; font-size: xx-large; color: #0094ff\">" + rec.Nome.ToUpper() + "</span></p>" +
                              "<p></p>" +
                              "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">Torre: <b>" + rec.UnidadeViewModel.EdificacaoDescricao + "</b></span></p>" +
                              "<p></p>" +
                              "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">Unidade: <b>" + rec.UnidadeViewModel.UnidadeID.ToString() + "</b></span></p>" +
                              "<p></p>" +
                              "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">Essa é uma mensagem de confirmação de seu cadastro. Seu registro no Sistema Administrativo do " + condominio.RazaoSocial + " foi realizado com sucesso.</span></p>" +
                              "<p></p>" +
                              "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">Clique no link abaixo para acessar o sistema:</span></p>" +
                              "<p><a href=\"http://www.parcparadiso.com.br/Account/Login/id=3\" target=\"_blank\"><span style=\"font-family: Verdana; font-size: small; color: #0094ff\">Acesso ao " + sistema.descricao + "</span></a></p>" +
                              "<p></p>" +
                              "<p></p>";

                if (rec.UnidadeViewModel.Validador == null)
                    Html += "<p></p>" +
                            "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">Solicitamos entrar em contato com a administração do condomínio para ativar o seu cadastro.</span></p>" +
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
                        "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">Através do sistema o condômino poderá:</span></p>" +
                        "<p></p>" +
                        "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">- Realizar a inclusão dos moradores de sua unidade com credenciamento de acesso.</span></p>" +
                        "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">- Consultar os documentos e comunicados oficiais do condomínio postados pelo síndico.</span></p>" +
                        "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">- Consultar os comunicados específicos destinados a sua torre.</span></p>" +
                        "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">- Abrir chamados à administração como por exemplo fazer o registro de uma ocorrência ou uma solicitação.</ span></p>" +
                        "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">- Atualizar seu cadastro (foto, dependentes, veículos, funcionários, etc).</ span></p>" +
                        "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">- Receber mensagens e alertas personalizados.</ span></p>" +
                        "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">- Consultar seu histórico de notificações.</ span></p>" +
                        "<hr />" +
                        "<p></p>" +
                        "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">Além desses recursos, estaremos implementando outras novidades. Aguarde !</span></p>" +
                        "<p>&nbsp;</p>" +
                        "<p>&nbsp;</p>" +
                        "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">Obrigado,</span></p>" +
                        "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">Administração " + condominio.RazaoSocial + "</span></p>";

                Validate result = sendMail.Send(sender, recipients, Html, Subject, Text);
                if (result.Code > 0)
                {
                    result.MessageBase = "Seu cadastro foi realizado com sucesso, mas por falhas de comunicação não foi possível enviar seu e-mail de confirmação. Favor entrar em contato com faleconosco@dwmsisteamas.com e solicite seu e-mail de ativação.";
                    throw new App_DominioException(result);
                }

            }
            rec.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString(), MessageType = MsgType.SUCCESS };
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

    public class EnviarEmailCredenciadoBI : DWMContext<ApplicationContext>, IProcess<CredenciadoViewModel, ApplicationContext>
    {
        #region Constructor
        public EnviarEmailCredenciadoBI() { }

        public EnviarEmailCredenciadoBI(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            Create(_db, _seguranca_db);
        }
        #endregion

        public CredenciadoViewModel Run(Repository value)
        {
            CredenciadoViewModel rec = (CredenciadoViewModel)value;
            string habilitaEmail = db.Parametros.Find(sessaoCorrente.empresaId, (int)Enumeracoes.Enumeradores.Param.HABILITA_EMAIL).Valor;
            if (habilitaEmail == "S")
            {
                int _empresaId = sessaoCorrente.empresaId;
                int _sistemaId = int.Parse(db.Parametros.Find(_empresaId, (int)Enumeracoes.Enumeradores.Param.SISTEMA).Valor);

                Sistema sistema = seguranca_db.Sistemas.Find(_sistemaId);
                Condominio condominio = db.Condominios.Find(sessaoCorrente.empresaId);
                CondominoUnidade CondominoUnidade = db.CondominoUnidades.Where(info => info.CondominoID == rec.CondominoID && !info.DataFim.HasValue).FirstOrDefault();
                Edificacao Edificacao = db.Edificacaos.Find(CondominoUnidade.EdificacaoID);
                Credenciado Credenciado = db.Credenciados.Where(info => info.Email == rec.Email).FirstOrDefault();

                rec.empresaId = _empresaId;

                SendEmail sendMail = new SendEmail();

                MailAddress sender = new MailAddress(condominio.RazaoSocial + " <" + condominio.Email + ">");
                List<string> recipients = new List<string>();

                recipients.Add(rec.Nome + "<" + rec.Email + ">");

                string Subject = "Confirmação de cadastro no " + condominio.RazaoSocial;
                string Text = "<p>Confirmação de cadastro</p>";
                string Html = "<p><span style=\"font-family: Verdana; font-size: larger; color: #656464\">" + sistema.descricao + "</span></p>" +
                              "<p><span style=\"font-family: Verdana; font-size: xx-large; color: #0094ff\">" + rec.Nome.ToUpper() + "</span></p>" +
                              "<p></p>" +
                              "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">Torre: <b>" + Edificacao.Descricao + "</b></span></p>" +
                              "<p></p>" +
                              "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">Unidade: <b>" + CondominoUnidade.UnidadeID.ToString() + "</b></span></p>" +
                              "<p></p>" +
                              "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">Essa é uma mensagem de confirmação de seu cadastro de credenciado. Seu registro no Sistema Administrativo do " + condominio.RazaoSocial + " foi realizado com sucesso.</span></p>" +
                              "<p></p>";

                Html += "<p></p>" +
                        "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">Seu Login de acesso é: </span></p>" +
                        "<p></p>" +
                        "<p><span style=\"font-family: Verdana; font-size: xx-large; color: #0094ff\">" + rec.Email + "</span></p>" +
                        "<p></p>" +
                        "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">Clique no link abaixo para ativar o seu cadastro e acessar o sistema:</span></p>" +
                        "<p><a href=\"http://www.parcparadiso.com.br/Account/AtivarCredenciado?id=" + Credenciado.UsuarioID.ToString() + "&key=" + rec.mensagem.Field +"\" target=\"_blank\"><span style=\"font-family: Verdana; font-size: small; color: #0094ff\">Acesso ao " + sistema.descricao + "</span></a></p>" +
                        "<p></p>" +
                        "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">Observação: este link estará disponível para ativação por 24 h</span></p>" +
                        "<hr />";

                Html += "<p></p>" +
                        "<p></p>" +
                        "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">Através do sistema o credenciado poderá:</span></p>" +
                        "<p></p>" +
                        "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">- Consultar os documentos e comunicados oficiais do condomínio postados pelo síndico.</span></p>" +
                        "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">- Consultar os comunicados específicos destinados a sua torre.</span></p>" +
                        "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">- Abrir chamados à administração como por exemplo fazer o registro de uma ocorrência ou uma solicitação.</ span></p>" +
                        "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">- Receber mensagens e alertas personalizados.</ span></p>" +
                        "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">- Consultar o histórico de notificações.</ span></p>" +
                        "<hr />" +
                        "<p></p>" +
                        "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">Além desses recursos, estaremos implementando outras novidades. Aguarde !</span></p>" +
                        "<p>&nbsp;</p>" +
                        "<p>&nbsp;</p>" +
                        "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">Obrigado,</span></p>" +
                        "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">Administração " + condominio.RazaoSocial + "</span></p>";

                Validate result = sendMail.Send(sender, recipients, Html, Subject, Text);
                if (result.Code > 0)
                {
                    result.MessageBase = "Seu cadastro foi realizado com sucesso, mas por falhas de comunicação não foi possível enviar seu e-mail de confirmação. Favor entrar em contato com faleconosco@dwmsisteamas.com e solicite seu e-mail de ativação.";
                    throw new App_DominioException(result);
                }

            }
            rec.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString(), MessageType = MsgType.SUCCESS };
            return rec;
        }

        public IEnumerable<CredenciadoViewModel> List(params object[] param)
        {
            throw new NotImplementedException();
        }

        public IPagedList PagedList(int? index, int pageSize = 50, params object[] param)
        {
            throw new NotImplementedException();
        }

    }

    public class EnviarEmailTokenBI : DWMContextLocal, IProcess<UnidadeViewModel, ApplicationContext>
    {
        #region Constructor
        public EnviarEmailTokenBI() { }

        public EnviarEmailTokenBI(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            Create(_db, _seguranca_db);
        }
        #endregion

        public UnidadeViewModel Run(Repository value)
        {
            UnidadeViewModel r = (UnidadeViewModel)value;
            string habilitaEmail = db.Parametros.Find(sessaoCorrente.empresaId, (int)Enumeracoes.Enumeradores.Param.HABILITA_EMAIL).Valor;
            if (habilitaEmail == "S")
            {
                int _empresaId = sessaoCorrente.empresaId;
                int _sistemaId = int.Parse(db.Parametros.Find(_empresaId, (int)Enumeracoes.Enumeradores.Param.SISTEMA).Valor);

                Sistema sistema = seguranca_db.Sistemas.Find(_sistemaId);
                Condominio condominio = db.Condominios.Find(sessaoCorrente.empresaId);

                r.empresaId = _empresaId;

                SendEmail sendMail = new SendEmail();

                MailAddress sender = new MailAddress(condominio.RazaoSocial + " <" + condominio.Email + ">");
                List<string> recipients = new List<string>();

                recipients.Add(r.NomeCondomino + "<" + r.Email + ">");

                string Subject = "Autorização de cadastro no " + condominio.RazaoSocial;
                string Text = "<p>Autorizção de cadastro</p>";
                string Html = "<p><span style=\"font-family: Verdana; font-size: larger; color: #656464\">" + sistema.descricao + "</span></p>" +
                              "<p><span style=\"font-family: Verdana; font-size: xx-large; color: #0094ff\">" + r.NomeCondomino + "</span></p>" +
                              "<p></p>" +
                              "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">Torre: <b>" + r.EdificacaoDescricao + "</b></span></p>" +
                              "<p></p>" +
                              "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">Unidade: <b>" + r.UnidadeID.ToString() + "</b></span></p>" +
                              "<p></p>" +
                              "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">Essa é uma mensagem de autorização de cadastro no Sistema Administrativo do " + condominio.RazaoSocial + ".</span></p>" +
                              "<p></p>";

                Html += "<p></p>" +
                        "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">Seu Login de acesso é: </span></p>" +
                        "<p></p>" +
                        "<p><span style=\"font-family: Verdana; font-size: xx-large; color: #0094ff\">" + r.Email + "</span></p>" +
                        "<p></p>" +
                        "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">Clique no link abaixo para acessar o sistema e realizar o seu cadastro:</span></p>" +
                        "<p><a href=\"http://www.parcparadiso.com.br/Account/Register/" + r.Validador + "\" target=\"_blank\"><span style=\"font-family: Verdana; font-size: small; color: #0094ff\">Registre-se no " + sistema.descricao + "</span></a></p>" +
                        "<p></p>" +
                        "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">Observação: este link estará disponível para ativação por 48 h</span></p>" +
                        "<hr />";

                Html += "<p></p>" +
                        "<p></p>" +
                        "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">Através do sistema o credenciado poderá:</span></p>" +
                        "<p></p>" +
                        "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">- Consultar os documentos e comunicados oficiais do condomínio postados pelo síndico.</span></p>" +
                        "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">- Consultar os comunicados específicos destinados a sua torre.</span></p>" +
                        "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">- Abrir chamados à administração como por exemplo fazer o registro de uma ocorrência ou uma solicitação.</ span></p>" +
                        "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">- Receber mensagens e alertas personalizados.</ span></p>" +
                        "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">- Consultar o histórico de notificações.</ span></p>" +
                        "<hr />" +
                        "<p></p>" +
                        "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">Além desses recursos, estaremos implementando outras novidades. Aguarde !</span></p>" +
                        "<p>&nbsp;</p>" +
                        "<p>&nbsp;</p>" +
                        "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">Obrigado,</span></p>" +
                        "<p><span style=\"font-family: Verdana; font-size: small; color: #000\">Administração " + condominio.RazaoSocial + "</span></p>";

                Validate result = sendMail.Send(sender, recipients, Html, Subject, Text);
                if (result.Code > 0)
                {
                    result.MessageBase = "Hocorreram falhas de comunicação e não foi possível enviar o e-mail de autorização para o condômino. ";
                    throw new App_DominioException(result);
                }

            }
            r.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString(), MessageType = MsgType.SUCCESS };
            return r;
        }

        public IEnumerable<UnidadeViewModel> List(params object[] param)
        {
            throw new NotImplementedException();
        }

        public IPagedList PagedList(int? index, int pageSize = 50, params object[] param)
        {
            throw new NotImplementedException();
        }

    }
}