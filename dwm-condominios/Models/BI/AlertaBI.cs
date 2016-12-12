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

namespace DWM.Models.BI
{
    public class AlertaBI : DWMContextLocal, IProcess<AlertaRepository, ApplicationContext>
    {
        #region Constructor
        public AlertaBI() { }

        public void Create(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.db = _db;
            this.seguranca_db = _seguranca_db;
        }

        public AlertaBI(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            Create(_db, _seguranca_db);
        }
        #endregion

        public AlertaRepository Run(Repository value)
        {
            ChamadoViewModel c = (ChamadoViewModel)value;
            AlertaRepository r = new AlertaRepository();
            try
            {
                EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
                security.Create(this.seguranca_db);
                sessaoCorrente = security._getSessaoCorrente(seguranca_db, value.sessionId);

                #region Recupera o número do chamado
                c.ChamadoID = (from cham in db.Chamados
                               where cham.CondominioID == sessaoCorrente.empresaId &&
                                     cham.UsuarioID == sessaoCorrente.usuarioId
                               select cham.ChamadoID).Max();
                #endregion

                #region Enviar E-mail de notificação
                int EmailTemplateID = int.Parse(db.Parametros.Find(sessaoCorrente.empresaId, (int)Enumeracoes.Enumeradores.Param.EMAIL_TEMPLATE_CHAMADO).Valor);

                string _Edificacao = c.EdificacaoID > 0 && c.FilaSolicitanteID == DWMSessaoLocal.FilaCondominoID(sessaoCorrente,db) ? " - " + db.Edificacaos.Find(c.EdificacaoID).Descricao + " " + c.UnidadeID.ToString() : "";

                EmailLogViewModel EmailLogViewModel = new EmailLogViewModel()
                {
                    uri = value.uri,
                    empresaId = sessaoCorrente.empresaId,
                    EmailTipoID = (int)Enumeracoes.Enumeradores.EmailTipo.CHAMADO,
                    CondominioID = sessaoCorrente.empresaId,
                    EdificacaoID = c.EdificacaoID,
                    UnidadeID = c.UnidadeID,
                    GrupoCondominoID = null,
                    DataEmail = Funcoes.Brasilia(),
                    Nome = c.NomeUsuario + _Edificacao,
                    Assunto = db.EmailTipos.Find((int)Enumeracoes.Enumeradores.EmailTipo.CHAMADO, sessaoCorrente.empresaId).Assunto + " " + c.ChamadoID.ToString() + " - " + c.Assunto + " - " + db.Condominios.Find(c.CondominioID).RazaoSocial,
                    EmailMensagem = db.EmailTemplates.Find(EmailTemplateID).EmailMensagem,
                    Repository = value
                };
                EmailNotificacaoBI notificacaoBI = new EmailNotificacaoBI(this.db, this.seguranca_db);
                EmailLogViewModel = notificacaoBI.Run(EmailLogViewModel);
                if (EmailLogViewModel.mensagem.Code > 0)
                    throw new App_DominioException(EmailLogViewModel.mensagem);
                #endregion

                Alerta alerta = new Alerta()
                {
                    usuarioId = c.UsuarioID,
                    sistemaId = sessaoCorrente.sistemaId,
                    dt_emissao = Funcoes.Brasilia(),
                    linkText = c.Assunto,
                    url = "../Chamado/Edit?chamadoId=" + c.ChamadoID.ToString(),
                    mensagem = "<b>" + Funcoes.Brasilia().ToString("dd/MM/yyyy HH:mm") + "h</b><p>" + c.Assunto + "</p>"
                };

                seguranca_db.Alertas.Add(alerta);
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

        public IEnumerable<AlertaRepository> List(params object[] param)
        {
            throw new NotImplementedException();
        }

        public IPagedList PagedList(int? index, int pageSize = 50, params object[] param)
        {
            throw new NotImplementedException();
        }

    }
}