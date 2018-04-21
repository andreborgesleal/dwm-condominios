using System;
using System.Collections.Generic;
using App_Dominio.Contratos;
using App_Dominio.Entidades;
using App_Dominio.Component;
using DWM.Models.Entidades;
using System.Web.Mvc;
using App_Dominio.Enumeracoes;
using App_Dominio.Security;
using DWM.Models.Repositories;
using System.Linq;
using DWM.Models.Persistence;
using System.Data.Entity.Infrastructure;
using DWM.Models.Enumeracoes;
using App_Dominio.Models;

namespace DWM.Models.BI
{
    public class CondominoAtivarBI : DWMContextLocal, IProcess<CondominoUnidadeViewModel, ApplicationContext>
    {
        #region Constructor
        public CondominoAtivarBI() { }

        public CondominoAtivarBI(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.db = _db;
            this.seguranca_db = _seguranca_db;
        }
        #endregion

        public CondominoUnidadeViewModel Run(Repository value)
        {
            CondominoUnidadeViewModel cu = (CondominoUnidadeViewModel)value;
            cu.mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso" };

            CondominoPFViewModel Cond = new CondominoPFViewModel()
            {
                CondominioID = cu.CondominioID,
                CondominoID = cu.CondominoID,
            };

            CondominoPFModel model = new CondominoPFModel(this.db, this.seguranca_db);
            CondominoPFViewModel r = model.getObject(Cond);
            try
            {
                r.IndSituacao = "A";
                r.uri = value.uri;
                r = model.Update(r);
                db.SaveChanges();

                #region envio de e-mail ao condômino para notificação de ativação
                int EmailTemplateID = int.Parse(db.Parametros.Find(sessaoCorrente.empresaId, (int)Enumeracoes.Enumeradores.Param.EMAIL_TEMPLATE_ATIVACAO_CONDOMINO).Valor);
                EmailLogViewModel EmailLogViewModel = new EmailLogViewModel() 
                {
                    UsuarioID = sessaoCorrente.usuarioId,
                    uri = value.uri,
                    empresaId = sessaoCorrente.empresaId,
                    EmailTipoID = (int)DWM.Models.Enumeracoes.Enumeradores.EmailTipo.ATIVACAO_CONDOMINO,
                    CondominioID = sessaoCorrente.empresaId,
                    EdificacaoID = cu.EdificacaoID,
                    Descricao_Edificacao = db.Edificacaos.Find(cu.EdificacaoID).Descricao,
                    UnidadeID = cu.UnidadeID,
                    Email = r.Email,
                    DataEmail = Funcoes.Brasilia(),
                    Nome = r.Nome,
                    Assunto = db.EmailTipos.Find((int)Enumeracoes.Enumeradores.EmailTipo.ATIVACAO_CONDOMINO, sessaoCorrente.empresaId).Assunto,
                    EmailMensagem = db.EmailTemplates.Find(EmailTemplateID).EmailMensagem,
                    Repository = r
                };

                EmailNotificacaoBI notificacaoBI = new EmailNotificacaoBI(this.db, this.seguranca_db);
                EmailLogViewModel = notificacaoBI.Run(EmailLogViewModel);
                if (EmailLogViewModel.mensagem.Code > 0)
                    throw new App_DominioException(EmailLogViewModel.mensagem);

                EmailLogViewModel.Repository = r;
                IEnumerable<EmailLogViewModel> EmailLogPessoas = notificacaoBI.List(EmailLogViewModel);

                foreach (EmailLogViewModel log in EmailLogPessoas)
                {
                    Alerta alerta = new Alerta()
                    {
                        usuarioId = r.UsuarioID.Value, // log.UsuarioID.Value,
                        sistemaId = sessaoCorrente.sistemaId,
                        dt_emissao = Funcoes.Brasilia(),
                        linkText = "Ativação - " + r.Nome.Split(' ')[0] + " " + r.Nome.Split(' ')[1],
                        url = "../Condomino/Index?id=" + r.CondominoID.ToString() + "&EdificacaoID=" + cu.EdificacaoID.ToString() + "&UnidadeID=" + cu.UnidadeID.ToString() + "&TipoPessoa=PF",
                        mensagem = "<b>" + Funcoes.Brasilia().ToString("dd/MM/yyyy HH:mm") + "h</b><p>Ativação - " + r.Nome.Split(' ')[0] + " " + r.Nome.Split(' ')[1] + "</p>"
                    };

                    seguranca_db.Alertas.Add(alerta);
                }
                #endregion
            }
            catch (ArgumentException ex)
            {
                cu.mensagem = new Validate() { Code = 997, Message = MensagemPadrao.Message(997).ToString(), MessageBase = ex.Message };
            }
            catch (App_DominioException ex)
            {
                cu.mensagem = ex.Result;

                if (ex.InnerException != null)
                    cu.mensagem.MessageBase = new App_DominioException(ex.InnerException.Message ?? ex.Message, GetType().FullName).Message;
                else
                    cu.mensagem.MessageBase = new App_DominioException(ex.Result.Message, GetType().FullName).Message;
            }
            catch (DbUpdateException ex)
            {
                cu.mensagem.MessageBase = ex.InnerException.InnerException.Message ?? ex.Message;
                if (cu.mensagem.MessageBase.ToUpper().Contains("REFERENCE") || cu.mensagem.MessageBase.ToUpper().Contains("FOREIGN"))
                {
                    if (cu.mensagem.MessageBase.ToUpper().Contains("DELETE"))
                    {
                        cu.mensagem.Code = 16;
                        cu.mensagem.Message = MensagemPadrao.Message(16).ToString();
                        cu.mensagem.MessageType = MsgType.ERROR;
                    }
                    else
                    {
                        cu.mensagem.Code = 28;
                        cu.mensagem.Message = MensagemPadrao.Message(28).ToString();
                        cu.mensagem.MessageType = MsgType.ERROR;
                    }
                }
                else if (cu.mensagem.MessageBase.ToUpper().Contains("PRIMARY"))
                {
                    cu.mensagem.Code = 37;
                    cu.mensagem.Message = MensagemPadrao.Message(37).ToString();
                    cu.mensagem.MessageType = MsgType.WARNING;
                }
                else if (cu.mensagem.MessageBase.ToUpper().Contains("UNIQUE KEY"))
                {
                    cu.mensagem.Code = 54;
                    cu.mensagem.Message = MensagemPadrao.Message(54).ToString();
                    cu.mensagem.MessageType = MsgType.WARNING;
                }
                else
                {
                    cu.mensagem.Code = 44;
                    cu.mensagem.Message = MensagemPadrao.Message(44).ToString();
                    cu.mensagem.MessageType = MsgType.ERROR;
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                cu.mensagem = new Validate() { Code = 42, Message = MensagemPadrao.Message(42).ToString(), MessageBase = ex.EntityValidationErrors.Select(m => m.ValidationErrors.First().ErrorMessage).First() };
            }
            catch (Exception ex)
            {
                cu.mensagem.Code = 17;
                cu.mensagem.Message = MensagemPadrao.Message(17).ToString();
                cu.mensagem.MessageBase = new App_DominioException(ex.InnerException.InnerException.Message ?? ex.Message, GetType().FullName).Message;
                cu.mensagem.MessageType = MsgType.ERROR;
            }
            return cu;
        }

        public IEnumerable<CondominoUnidadeViewModel> List(params object[] param)
        {
            throw new NotImplementedException();
        }

        public IPagedList PagedList(int? index, int pageSize = 50, params object[] param)
        {
            throw new NotImplementedException();
        }

    }
}