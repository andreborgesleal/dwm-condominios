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


namespace DWM.Models.BI
{
    public class ChamadoEditBI : DWMContextLocal, IProcessAPI<ChamadoViewModel, ApplicationContext>
    {
        #region Constructor
        public ChamadoEditBI() { }

        public ChamadoEditBI(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            Create(_db, _seguranca_db);
        }

        #endregion

        public ChamadoViewModel Run(Repository value)
        {
            ChamadoViewModel r = (ChamadoViewModel)value;
            ChamadoViewModel result = new ChamadoViewModel()
            {
                mensagem = new Validate() { Code = -1, Message = "Registro processado com sucesso" } 
            };
            try
            {
                int _empresaId = sessaoCorrente.empresaId;

                ChamadoModel model = new ChamadoModel();
                model.Create(this.db, this.seguranca_db, r.sessionId);
                result = model.MapToRepository(this.db.Chamados.Find(r.ChamadoID));

                #region Valida permissão do usuário para editar o chamado
                if ((SessaoLocal.CondominoID > 0 || SessaoLocal.CredenciadoID > 0) && !(result.CondominioID == _empresaId &&
                      (result.CondominoID.HasValue && result.CondominoID == SessaoLocal.CondominoID || 
                       result.CredenciadoID.HasValue && result.CredenciadoID == SessaoLocal.CredenciadoID ||
                       result.UsuarioID == SessaoLocal.usuarioId ||
                       result.UsuarioFilaID.HasValue && result.UsuarioFilaID == SessaoLocal.usuarioId ||
                       result.Rotas.Where(info => info.UsuarioID == SessaoLocal.usuarioId).Count() > 0)))
                {
                    // Verifica se o usuário está em alguma fila de atendimento
                    bool HeIsAtTheFila = false;
                    foreach (ChamadoFilaViewModel fil in result.Rotas)
                    {
                        int FilaAtendimentoID = fil.FilaAtendimentoID;
                        if ((from fila in db.FilaAtendimentos
                             join filaUsu in db.FilaAtendimentoUsuarios on fila.FilaAtendimentoID equals filaUsu.FilaAtendimentoID
                             where fila.CondominioID == SessaoLocal.empresaId &&
                                   fila.FilaAtendimentoID == FilaAtendimentoID &&
                                   filaUsu.UsuarioID == SessaoLocal.usuarioId
                             select fila).Count() > 0)
                            HeIsAtTheFila = true;
                    }
                    if (!HeIsAtTheFila)
                        result = null;
                }

                result.mensagem.Code = -1; // se for igual a zero vai executar no factorylocalhost o db.savechanges
                    
                #endregion
            }
            catch (ArgumentException ex)
            {
                result.mensagem = new Validate() { Code = 999, Message = MensagemPadrao.Message(999).ToString(), MessageBase = ex.Message };
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
                if (result.mensagem.MessageBase.ToUpper().Contains("REFERENCE"))
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

        public IEnumerable<ChamadoViewModel> List(params object[] param)
        {
            throw new NotImplementedException();
        }

        public IPagedList PagedList(int? index, int pageSize = 50, params object[] param)
        {
            throw new NotImplementedException();
        }

    }
}