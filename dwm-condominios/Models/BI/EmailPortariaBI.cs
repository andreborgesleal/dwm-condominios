using System;
using System.Collections.Generic;
using App_Dominio.Contratos;
using App_Dominio.Entidades;
using App_Dominio.Component;
using DWM.Models.Repositories;
using DWM.Models.Entidades;
using System.Web.Mvc;
using DWM.Models.Persistence;
using App_Dominio.Enumeracoes;
using App_Dominio.Security;
using System.Linq;
using System.Data.Entity.Infrastructure;
using App_Dominio.Models;
using App_Dominio.Repositories;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;

namespace DWM.Models.BI
{
    public class EmailPortariaBI : DWMContextLocal, IProcess<VisitanteAcessoViewModel, ApplicationContext>
    {
        #region Constructor
        public EmailPortariaBI() { }

        public void Create(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.db = _db;
            this.seguranca_db = _seguranca_db;
        }

        public EmailPortariaBI(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            Create(_db, _seguranca_db);
        }
        #endregion

        public VisitanteAcessoViewModel Run(Repository value)
        {
            VisitanteAcessoViewModel a = (VisitanteAcessoViewModel)value;
            VisitanteAcessoViewModel r = new VisitanteAcessoViewModel();
            try
            {
                EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
                security.Create(this.seguranca_db);
                sessaoCorrente = security._getSessaoCorrente(seguranca_db, value.sessionId);
                SessaoLocal = DWMSessaoLocal.GetSessaoLocal(sessaoCorrente, this.db);

                DateTime _hoje = Funcoes.Brasilia().Date;

                #region Recupera o AcessoID
                if (a.AcessoID == 0)
                {
                    a.AcessoID = (from vac in db.VisitanteAcessos
                                  where vac.VisitanteID == a.VisitanteID
                                        && System.Data.Entity.DbFunctions.TruncateTime(vac.DataInclusao) == _hoje
                                  select vac.AcessoID).Max();

                    VisitanteAcessoModel AcessoModel = new VisitanteAcessoModel(this.db, this.seguranca_db);
                    a = AcessoModel.getObject(a);
                }
                #endregion

                #region Enviar E-mail de notificação
                int EmailTemplateID = int.Parse(db.Parametros.Find(sessaoCorrente.empresaId, (int)Enumeracoes.Enumeradores.Param.EMAIL_TEMPLATE_PORTARIA).Valor);
                string _Edificacao = " - Administração";
                string _Nome = seguranca_db.Usuarios.Find(sessaoCorrente.usuarioId).nome;
                if (SessaoLocal.Unidades != null)
                {
                    _Edificacao = " - " + db.Edificacaos.Find(a.EdificacaoID).Descricao + " " + a.UnidadeID.ToString();
                    _Nome = a.Visitante.NomeCondomino;
                }

                EmailLogViewModel EmailLogViewModel = new EmailLogViewModel()
                {
                    UsuarioID = sessaoCorrente.usuarioId,
                    uri = value.uri,
                    empresaId = sessaoCorrente.empresaId,
                    EmailTipoID = (int)Enumeracoes.Enumeradores.EmailTipo.PORTARIA,
                    CondominioID = sessaoCorrente.empresaId,
                    EdificacaoID = a.EdificacaoID ?? 0,
                    UnidadeID = a.UnidadeID ?? 0,
                    GrupoCondominoID = null,
                    DataEmail = Funcoes.Brasilia(),
                    Descricao_Edificacao = a.DescricaoEdificacao,
                    Nome = _Nome,
                    Assunto = db.EmailTipos.Find((int)Enumeracoes.Enumeradores.EmailTipo.PORTARIA, sessaoCorrente.empresaId).Assunto + " " + a.AcessoID.ToString(),
                    EmailMensagem = db.EmailTemplates.Find(EmailTemplateID).EmailMensagem.Replace("@ID",a.AcessoID.ToString()).Replace("@NomeVisitante", a.Visitante.Nome).Replace("@data",a.DataAutorizacao.ToString("dd/MM/yyyy")),
                    Repository = a
                };
                EmailNotificacaoBI notificacaoBI = new EmailNotificacaoBI(this.db, this.seguranca_db);
                EmailLogViewModel = notificacaoBI.Run(EmailLogViewModel);
                if (EmailLogViewModel.mensagem.Code > 0)
                    throw new App_DominioException(EmailLogViewModel.mensagem);

                EmailLogViewModel.Repository = a;
                IEnumerable<EmailLogViewModel> EmailLogPessoas = notificacaoBI.List(EmailLogViewModel);
                #endregion

                foreach (EmailLogViewModel log in EmailLogPessoas.Where(info => info.UsuarioID > 0))
                {
                    Alerta alerta = new Alerta()
                    {
                        usuarioId = log.UsuarioID.Value,
                        sistemaId = sessaoCorrente.sistemaId,
                        dt_emissao = Funcoes.Brasilia(),
                        linkText = "Convite - " + a.Visitante.Nome,
                        url = "../Portaria/Edit?id=" + a.AcessoID.ToString(),
                        mensagem = "<b>" + Funcoes.Brasilia().ToString("dd/MM/yyyy HH:mm") + "h</b><p>Convite - " + a.Visitante.Nome + "</p>"
                    };

                    seguranca_db.Alertas.Add(alerta);
                }
                r.mensagem = new Validate() { Code = 0 };
            }
            catch (DbUpdateException ex)
            {
                r.mensagem.MessageBase = ex.InnerException.InnerException.Message ?? ex.Message;
                if (r.mensagem.MessageBase.ToUpper().Contains("REFERENCE"))
                {
                    if (r.mensagem.MessageBase.ToUpper().Contains("DELETE"))
                    {
                        r.mensagem.Code = 16;
                        r.mensagem.Message = MensagemPadrao.Message(16).ToString();
                        r.mensagem.MessageType = MsgType.ERROR;
                    }
                    else
                    {
                        r.mensagem.Code = 28;
                        r.mensagem.Message = MensagemPadrao.Message(28).ToString();
                        r.mensagem.MessageType = MsgType.ERROR;
                    }
                }
                else if (r.mensagem.MessageBase.ToUpper().Contains("PRIMARY"))
                {
                    r.mensagem.Code = 37;
                    r.mensagem.Message = MensagemPadrao.Message(37).ToString();
                    r.mensagem.MessageType = MsgType.WARNING;
                }
                else if (r.mensagem.MessageBase.ToUpper().Contains("UNIQUE KEY"))
                {
                    r.mensagem.Code = 54;
                    r.mensagem.Message = MensagemPadrao.Message(54).ToString();
                    r.mensagem.MessageType = MsgType.WARNING;
                }
                else
                {
                    r.mensagem.Code = 44;
                    r.mensagem.Message = MensagemPadrao.Message(44).ToString();
                    r.mensagem.MessageType = MsgType.ERROR;
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                r.mensagem = new Validate() { Code = 42, Message = MensagemPadrao.Message(42).ToString(), MessageBase = ex.EntityValidationErrors.Select(m => m.ValidationErrors.First().ErrorMessage).First() };
            }
            catch (Exception ex)
            {
                r.mensagem.Code = 17;
                r.mensagem.Message = MensagemPadrao.Message(17).ToString();
                r.mensagem.MessageBase = new App_DominioException(ex.InnerException.InnerException.Message ?? ex.Message, GetType().FullName).Message;
                r.mensagem.MessageType = MsgType.ERROR;
            }
            return r;
        }

        public IEnumerable<VisitanteAcessoViewModel> List(params object[] param)
        {
            throw new NotImplementedException();
        }

        public IPagedList PagedList(int? index, int pageSize = 50, params object[] param)
        {
            throw new NotImplementedException();
        }
    }
}