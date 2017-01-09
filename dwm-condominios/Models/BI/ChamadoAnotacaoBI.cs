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
using System.Data.Entity;

namespace DWM.Models.BI
{
    public class ChamadoAnotacaoBI : DWMContextLocal, IProcess<ChamadoAnotacaoViewModel, ApplicationContext>
    {
        #region Constructor
        public ChamadoAnotacaoBI() { }

        public ChamadoAnotacaoBI(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            Create(_db, _seguranca_db);
        }
        #endregion

        public ChamadoAnotacaoViewModel Run(Repository value)
        {
            ChamadoViewModel repository = (ChamadoViewModel)value;
            ChamadoAnotacaoViewModel r = repository.ChamadoAnotacaoViewModel;
            ChamadoAnotacaoViewModel result = new ChamadoAnotacaoViewModel()
            {
                uri = r.uri,
                empresaId = sessaoCorrente.empresaId,
                ChamadoID = r.ChamadoID,
                DataAnotacao = Funcoes.Brasilia(),
                Mensagem = r.Mensagem,
                UsuarioID = SessaoLocal.usuarioId,
                mensagem = new Validate() { Code = 0, Message = "Registro processado com sucesso" }
            };

            try
            {
                int _empresaId = sessaoCorrente.empresaId;

                ChamadoAnotacaoModel model = new ChamadoAnotacaoModel();
                model.Create(this.db, this.seguranca_db);
                result = model.Insert(result);

                if (result.mensagem.Code > 0)
                    throw new App_DominioException(result.mensagem);

                #region Encaminha o chamado para a Fila de Atendimento
                if (repository.FilaAtendimentoID.HasValue)
                {
                    ChamadoFilaModel ChamadoFilaModel = new ChamadoFilaModel();
                    ChamadoFilaModel.Create(this.db, this.seguranca_db);
                    ChamadoFilaViewModel ChamadoFilaViewModel = new ChamadoFilaViewModel()
                    {
                        empresaId = SessaoLocal.empresaId,
                        uri = r.uri,
                        ChamadoID = repository.ChamadoID,
                        FilaAtendimentoID = repository.FilaAtendimentoID.Value,
                    };
                    if (repository.FilaAtendimentoID == DWMSessaoLocal.FilaCondominoID(this.sessaoCorrente, this.db))
                    {
                        Chamado Chamado = db.Chamados.Find(repository.ChamadoID);
                        if (Chamado.CredenciadoID.HasValue)
                            ChamadoFilaViewModel.UsuarioID = db.Credenciados.Find(Chamado.CredenciadoID).UsuarioID;
                        else if(Chamado.CondominoID.HasValue)
                            ChamadoFilaViewModel.UsuarioID = db.Condominos.Find(Chamado.CondominoID).UsuarioID;
                    }
                        
                    ChamadoFilaViewModel = ChamadoFilaModel.Insert(ChamadoFilaViewModel);
                    if (ChamadoFilaViewModel.mensagem.Code > 0)
                        throw new App_DominioException(ChamadoFilaViewModel.mensagem);
                }
                #endregion

                db.SaveChanges();
                seguranca_db.SaveChanges();

                result.mensagem.Code = -1; // Tem que devolver -1 porque na Superclasse, se devolver zero, vai executar novamente o SaveChanges.
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

        public IEnumerable<ChamadoAnotacaoViewModel> List(params object[] param)
        {
            ListViewChamadoAnotacao ListAnotacoes = new ListViewChamadoAnotacao(this.db, this.seguranca_db);
            return ListAnotacoes.Bind(0, 50, param);
        }

        public IPagedList PagedList(int? index, int pageSize = 50, params object[] param)
        {
            throw new NotImplementedException();
        }

    }
}