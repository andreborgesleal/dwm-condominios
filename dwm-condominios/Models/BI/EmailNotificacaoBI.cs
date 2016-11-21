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
using DWM.Models.Persistence;
using System.Data.Entity.Infrastructure;

namespace DWM.Models.BI
{
    public class EmailNotificacaoBI : DWMContextLocal, IProcess<EmailLogViewModel, ApplicationContext>
    {
        #region Constructor
        public EmailNotificacaoBI() { }

        public EmailNotificacaoBI(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            Create(_db, _seguranca_db);
        }
        #endregion

        public EmailLogViewModel Run(Repository value)
        {
            EmailLogViewModel log = (EmailLogViewModel)value;
            log.mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso" };
            string habilitaEmail = db.Parametros.Find(log.CondominioID, (int)Enumeracoes.Enumeradores.Param.HABILITA_EMAIL).Valor;
            if (habilitaEmail == "S")
            {
                try
                {
                    int _empresaId = SessaoLocal.empresaId;
                    int _sistemaId = int.Parse(db.Parametros.Find(log.CondominioID, (int)Enumeracoes.Enumeradores.Param.SISTEMA).Valor);

                    Condominio condominio = db.Condominios.Find(log.CondominioID);
                    Sistema sistema = seguranca_db.Sistemas.Find(_sistemaId);

                    log.empresaId = _empresaId;

                    IEnumerable<EmailLogViewModel> EmailLogPessoas = List(log);

                    SendEmail sendMail = new SendEmail();

                    MailAddress sender = new MailAddress(condominio.RazaoSocial + " <" + condominio.Email + ">");
                    List<string> recipients = new List<string>();

                    foreach (EmailLogViewModel item in EmailLogPessoas)
                        recipients.Add(item.Nome + "<" + item.Email + ">");

                    string Subject = log.Assunto.Replace("@condominio", condominio.RazaoSocial);
                    string Html = log.EmailMensagem.Replace("@condominio", condominio.RazaoSocial).Replace("@nome", log.Nome).Replace("@unidade", log.UnidadeID.ToString()).Replace("@edificacao", log.Descricao_Edificacao).Replace("@grupo", log.Descricao_GrupoCondomino).Replace("@sistema", sistema.descricao).Replace("@email", log.Email);

                    Validate result = sendMail.Send(sender, null, Html, Subject, "", recipients);
                    if (result.Code > 0)
                    {
                        result.MessageBase = "Não foi possível enviar o e-mail. Tente novamente mais tarde e se o erro persistir, favor entrar em contato com faleconosco@dwmsisteamas.com e reporte o código de erro " + result.Code.ToString() + ". ";
                        throw new App_DominioException(result);
                    }

                    #region Incluir o Log do e-mail enviado
                    EmailLogModel Model = new EmailLogModel(this.db, this.seguranca_db);
                    log = Model.Insert(log);
                    if (log.mensagem.Code > 0)
                        throw new App_DominioException(log.mensagem);
                    #endregion

                    log.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString(), MessageType = MsgType.SUCCESS };
                }
                catch (ArgumentException ex)
                {
                    log.mensagem = new Validate() { Code = 997, Message = MensagemPadrao.Message(997).ToString(), MessageBase = ex.Message };
                }
                catch (App_DominioException ex)
                {
                    log.mensagem = ex.Result;

                    if (ex.InnerException != null)
                        log.mensagem.MessageBase = new App_DominioException(ex.InnerException.Message ?? ex.Message, GetType().FullName).Message;
                    else
                        log.mensagem.MessageBase = new App_DominioException(ex.Result.Message, GetType().FullName).Message;
                }
                catch (DbUpdateException ex)
                {
                    log.mensagem.MessageBase = ex.InnerException.InnerException.Message ?? ex.Message;
                    if (log.mensagem.MessageBase.ToUpper().Contains("REFERENCE") || log.mensagem.MessageBase.ToUpper().Contains("FOREIGN"))
                    {
                        if (log.mensagem.MessageBase.ToUpper().Contains("DELETE"))
                        {
                            log.mensagem.Code = 16;
                            log.mensagem.Message = MensagemPadrao.Message(16).ToString();
                            log.mensagem.MessageType = MsgType.ERROR;
                        }
                        else
                        {
                            log.mensagem.Code = 28;
                            log.mensagem.Message = MensagemPadrao.Message(28).ToString();
                            log.mensagem.MessageType = MsgType.ERROR;
                        }
                    }
                    else if (log.mensagem.MessageBase.ToUpper().Contains("PRIMARY"))
                    {
                        log.mensagem.Code = 37;
                        log.mensagem.Message = MensagemPadrao.Message(37).ToString();
                        log.mensagem.MessageType = MsgType.WARNING;
                    }
                    else if (log.mensagem.MessageBase.ToUpper().Contains("UNIQUE KEY"))
                    {
                        log.mensagem.Code = 54;
                        log.mensagem.Message = MensagemPadrao.Message(54).ToString();
                        log.mensagem.MessageType = MsgType.WARNING;
                    }
                    else
                    {
                        log.mensagem.Code = 44;
                        log.mensagem.Message = MensagemPadrao.Message(44).ToString();
                        log.mensagem.MessageType = MsgType.ERROR;
                    }
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {
                    log.mensagem = new Validate() { Code = 42, Message = MensagemPadrao.Message(42).ToString(), MessageBase = ex.EntityValidationErrors.Select(m => m.ValidationErrors.First().ErrorMessage).First() };
                }
                catch (Exception ex)
                {
                    log.mensagem.Code = 17;
                    log.mensagem.Message = MensagemPadrao.Message(17).ToString();
                    log.mensagem.MessageBase = new App_DominioException(ex.InnerException.InnerException.Message ?? ex.Message, GetType().FullName).Message;
                    log.mensagem.MessageType = MsgType.ERROR;
                }
            }
            
            return log;
        }

        public IEnumerable<EmailLogViewModel> List(params object[] param)
        {
            EmailLogViewModel log = (EmailLogViewModel)param [0];
            List<EmailLogViewModel> result = new List<EmailLogViewModel>();

            if (log.EmailTipoID == (int)Enumeracoes.Enumeradores.EmailTipo.INFORMATIVO)
            {
                result = (from con in db.Condominos
                          join und in db.CondominoUnidades on con.CondominoID equals und.CondominoID
                          join gru in db.GrupoCondominoUsuarios on con.CondominoID equals gru.CondominoID into GRU
                          from gru in GRU.DefaultIfEmpty()
                          where con.CondominioID == sessaoCorrente.empresaId
                                && und.CondominioID == sessaoCorrente.empresaId
                                && und.DataFim == null && (!log.EdificacaoID.HasValue || (und.EdificacaoID == log.EdificacaoID))
                                && (!log.GrupoCondominoID.HasValue || gru.GrupoCondominoID == log.GrupoCondominoID)
                          select new EmailLogViewModel()
                          {
                              Nome = con.Nome,
                              Email = con.Email
                          }).Union(from con in db.Condominos
                                   join cre in db.Credenciados on con.CondominoID equals cre.CondominoID
                                   join und in db.CondominoUnidades on con.CondominoID equals und.CondominoID
                                   join gru in db.GrupoCondominoUsuarios on con.CondominoID equals gru.CondominoID into GRU
                                   from gru in GRU.DefaultIfEmpty()
                                   where con.CondominioID == sessaoCorrente.empresaId
                                         && und.CondominioID == sessaoCorrente.empresaId
                                         && und.DataFim == null && (!log.EdificacaoID.HasValue || (und.EdificacaoID == log.EdificacaoID))
                                         && (!log.GrupoCondominoID.HasValue || gru.GrupoCondominoID == log.GrupoCondominoID)
                                   select new EmailLogViewModel()
                                   {
                                       Nome = cre.Nome,
                                       Email = cre.Email
                                   }).ToList();
            }
            else if(log.EmailTipoID == (int)Enumeracoes.Enumeradores.EmailTipo.CADASTRO_CREDENCIADO)
            {
                result.Add(log);
            }

            return result;
        }

        public IPagedList PagedList(int? index, int pageSize = 50, params object[] param)
        {
            throw new NotImplementedException();
        }

    }
}