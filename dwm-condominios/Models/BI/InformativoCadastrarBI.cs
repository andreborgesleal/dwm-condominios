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

namespace DWM.Models.BI
{
    public class InformativoCadastrarBI : DWMContext<ApplicationContext>, IProcess<InformativoViewModel, ApplicationContext>
    {
        #region Constructor
        public InformativoCadastrarBI() { }

        public InformativoCadastrarBI(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            Create(_db, _seguranca_db);
        }
        #endregion

        public InformativoViewModel Run(Repository value)
        {
            int EmailTipoID = (int)Enumeracoes.Enumeradores.EmailTipo.INFORMATIVO;
            InformativoViewModel r = (InformativoViewModel)value;
            InformativoViewModel result = new InformativoViewModel()
            {
                uri = r.uri,
                empresaId = sessaoCorrente.empresaId,
                CondominioID = r.CondominioID,
                EdificacaoID = r.EdificacaoID,
                GrupoCondominoID = r.GrupoCondominoID,
                DataInformativo = Funcoes.Brasilia(),
                DataPublicacao = r.DataPublicacao,
                DataExpiracao = r.DataExpiracao,
                Cabecalho = r.Cabecalho,
                Resumo = r.Resumo,
                MensagemDetalhada = r.MensagemDetalhada,
                Midia1 = r.Midia1,
                Midia2 = r.Midia2,
                InformativoAnuncio = "N",
                mensagem = new Validate() { Code = 0, Message = "Registro processado com sucesso" }
            };

            EmailLogViewModel EmailLogViewModel = new EmailLogViewModel()
            {
                uri = r.uri,
                empresaId = sessaoCorrente.empresaId,
                EmailTipoID = EmailTipoID, // "Informativo"
                CondominioID = sessaoCorrente.empresaId,
                EdificacaoID = r.EdificacaoID,
                GrupoCondominoID = r.GrupoCondominoID,
                DataEmail = Funcoes.Brasilia(),
                Assunto = db.EmailTipos.Find(EmailTipoID, sessaoCorrente.empresaId).Assunto,
                EmailMensagem = r.MensagemDetalhada
            };

            try
            {
                #region Passo 1: Incluir o informativo
                InformativoModel InformativoModel = new InformativoModel(this.db, this.seguranca_db);
                result = InformativoModel.Insert(result);
                if (result.mensagem.Code > 0)
                    throw new App_DominioException(result.mensagem);
                #endregion

                #region Passo 2: Enviar o e-mail de notificação
                EmailNotificacaoBI notificacaoBI = new EmailNotificacaoBI(this.db, this.seguranca_db);
                EmailLogViewModel = notificacaoBI.Run(EmailLogViewModel);
                if (EmailLogViewModel.mensagem.Code > 0)
                    throw new App_DominioException(EmailLogViewModel.mensagem);
                #endregion

                db.SaveChanges();
                seguranca_db.SaveChanges();

                result.mensagem.Code = 0; 
            }
            catch (ArgumentException ex)
            {
                result.mensagem = new Validate() { Code = 997, Message = MensagemPadrao.Message(997).ToString(), MessageBase = ex.Message };
            }
            catch (App_DominioException ex)
            {
                result.mensagem = ex.Result;

                if (ex.InnerException != null)
                    result.mensagem.MessageBase = new App_DominioException(ex.InnerException.Message ?? ex.Message, GetType().FullName).Message;
                else
                    result.mensagem.MessageBase = new App_DominioException(ex.Result.Message, GetType().FullName).Message;
            }
            catch (DbUpdateException ex)
            {
                result.mensagem.MessageBase = ex.InnerException.InnerException.Message ?? ex.Message;
                if (result.mensagem.MessageBase.ToUpper().Contains("REFERENCE") || result.mensagem.MessageBase.ToUpper().Contains("FOREIGN"))
                {
                    if (result.mensagem.MessageBase.ToUpper().Contains("DELETE"))
                    {
                        result.mensagem.Code = 16;
                        result.mensagem.Message = MensagemPadrao.Message(16).ToString();
                        result.mensagem.MessageType = MsgType.ERROR;
                    }
                    else
                    {
                        result.mensagem.Code = 28;
                        result.mensagem.Message = MensagemPadrao.Message(28).ToString();
                        result.mensagem.MessageType = MsgType.ERROR;
                    }
                }
                else if (result.mensagem.MessageBase.ToUpper().Contains("PRIMARY"))
                {
                    result.mensagem.Code = 37;
                    result.mensagem.Message = MensagemPadrao.Message(37).ToString();
                    result.mensagem.MessageType = MsgType.WARNING;
                }
                else if (result.mensagem.MessageBase.ToUpper().Contains("UNIQUE KEY"))
                {
                    result.mensagem.Code = 54;
                    result.mensagem.Message = MensagemPadrao.Message(54).ToString();
                    result.mensagem.MessageType = MsgType.WARNING;
                }
                else
                {
                    result.mensagem.Code = 44;
                    result.mensagem.Message = MensagemPadrao.Message(44).ToString();
                    result.mensagem.MessageType = MsgType.ERROR;
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                result.mensagem = new Validate() { Code = 42, Message = MensagemPadrao.Message(42).ToString(), MessageBase = ex.EntityValidationErrors.Select(m => m.ValidationErrors.First().ErrorMessage).First() };
            }
            catch (Exception ex)
            {
                result.mensagem.Code = 17;
                result.mensagem.Message = MensagemPadrao.Message(17).ToString();
                result.mensagem.MessageBase = new App_DominioException(ex.InnerException.InnerException.Message ?? ex.Message, GetType().FullName).Message;
                result.mensagem.MessageType = MsgType.ERROR;
            }
            return result;
        }

        public IEnumerable<InformativoViewModel> List(params object[] param)
        {
            var q = (from inf in db.Informativos
                     join cond in db.Condominios on inf.CondominioID equals cond.CondominioID
                     join ed in db.Edificacaos on inf.EdificacaoID equals ed.EdificacaoID
                     join gc in db.GrupoCondominos on inf.GrupoCondominoID equals gc.GrupoCondominoID
                     //where (inf.DataInformativo <= )
                     select new InformativoViewModel
                     {
                         Cabecalho = inf.Cabecalho,
                         CondominioID = inf.CondominioID,
                     }).ToList();

            return q;
        }

        public IPagedList PagedList(int? index, int pageSize = 50, params object[] param)
        {

            //throw new NotImplementedException();

            int pageIndex = index ?? 0;

            #region LINQ
            var q = (from inf in db.Informativos
                     //where (inf.DataInformativo <= )
                     select new InformativoViewModel
                     {
                         Cabecalho = inf.Cabecalho,
                         CondominioID = inf.CondominioID,
                         GrupoCondominoID = inf.GrupoCondominoID,
                         DataInformativo = inf.DataInformativo,
                         DataPublicacao = inf.DataPublicacao,
                         DataExpiracao = inf.DataExpiracao,
                         Resumo = inf.Resumo,
                         MensagemDetalhada = inf.MensagemDetalhada,
                         InformativoAnuncio = inf.InformativoAnuncio,
                         PageSize = pageSize,
                         TotalCount = (from inf1 in db.Informativos
                                       select inf1).Count()
                     }).Skip((index ?? 0) * pageSize).Take(pageSize).ToList();

            return new PagedList<InformativoViewModel>(q.ToList(), pageIndex, pageSize, q.Count() > 0 ? q.First().TotalCount : 0, "ListOperacaoParam", null, "div-list-static");
            #endregion
        }

    }
}