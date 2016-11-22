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
    public class FilaAtendimentoUsuarioBI : DWMContextLocal, IProcess<FilaAtendimentoUsuarioEditViewModel, ApplicationContext>
    {
        #region Constructor
        public FilaAtendimentoUsuarioBI() { }

        public FilaAtendimentoUsuarioBI(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            Create(_db, _seguranca_db);
        }

        #endregion

        public FilaAtendimentoUsuarioEditViewModel Run(Repository value)
        {
            FilaAtendimentoUsuarioEditViewModel r = (FilaAtendimentoUsuarioEditViewModel)value;
            FilaAtendimentoUsuarioEditViewModel result = new FilaAtendimentoUsuarioEditViewModel()
            {
                mensagem = new Validate() { Code = 0, Message = "Registro processado com sucesso" }
            };
            try
            {
                int _empresaId = sessaoCorrente.empresaId;

                #region FilaAtendimentoUsuarioEditViewModel
                FilaAtendimentoUsuarioModel Model = new FilaAtendimentoUsuarioModel(this.db, this.seguranca_db);
                result.FilaAtendimentoUsuarioViewModel = Model.CreateRepository();
                result.FilaAtendimentoUsuarioViewModel.FilaAtendimentoID = r.FilaAtendimentoUsuarioViewModel.FilaAtendimentoID;

                ListViewFilaAtendimentoUsuario list = new ListViewFilaAtendimentoUsuario(this.db, this.seguranca_db);
                result.FilaAtendimentoUsuarios = list.Bind(0, 50, result.FilaAtendimentoUsuarioViewModel.FilaAtendimentoID);
                #endregion

            }
            catch (ArgumentException ex)
            {
                result.FilaAtendimentoUsuarioViewModel.mensagem = new Validate() { Code = 999, Message = MensagemPadrao.Message(999).ToString(), MessageBase = ex.Message };
            }
            catch (App_DominioException ex)
            {
                result.FilaAtendimentoUsuarioViewModel.mensagem = ex.Result;

                if (ex.InnerException != null)
                    result.FilaAtendimentoUsuarioViewModel.mensagem.MessageBase = new App_DominioException(ex.InnerException.Message ?? ex.Message, GetType().FullName).Message;
                else
                    result.FilaAtendimentoUsuarioViewModel.mensagem.MessageBase = new App_DominioException(ex.Result.Message, GetType().FullName).Message;
            }
            catch (DbUpdateException ex)
            {
                result.FilaAtendimentoUsuarioViewModel.mensagem.MessageBase = ex.InnerException.InnerException.Message ?? ex.Message;
                if (result.FilaAtendimentoUsuarioViewModel.mensagem.MessageBase.ToUpper().Contains("REFERENCE"))
                {
                    if (result.FilaAtendimentoUsuarioViewModel.mensagem.MessageBase.ToUpper().Contains("DELETE"))
                    {
                        result.FilaAtendimentoUsuarioViewModel.mensagem.Code = 16;
                        result.FilaAtendimentoUsuarioViewModel.mensagem.Message = MensagemPadrao.Message(16).ToString();
                        result.FilaAtendimentoUsuarioViewModel.mensagem.MessageType = MsgType.ERROR;
                    }
                    else
                    {
                        result.FilaAtendimentoUsuarioViewModel.mensagem.Code = 28;
                        result.FilaAtendimentoUsuarioViewModel.mensagem.Message = MensagemPadrao.Message(28).ToString();
                        result.FilaAtendimentoUsuarioViewModel.mensagem.MessageType = MsgType.ERROR;
                    }
                }
                else if (result.FilaAtendimentoUsuarioViewModel.mensagem.MessageBase.ToUpper().Contains("PRIMARY"))
                {
                    result.FilaAtendimentoUsuarioViewModel.mensagem.Code = 37;
                    result.FilaAtendimentoUsuarioViewModel.mensagem.Message = MensagemPadrao.Message(37).ToString();
                    result.FilaAtendimentoUsuarioViewModel.mensagem.MessageType = MsgType.WARNING;
                }
                else if (result.FilaAtendimentoUsuarioViewModel.mensagem.MessageBase.ToUpper().Contains("UNIQUE KEY"))
                {
                    result.FilaAtendimentoUsuarioViewModel.mensagem.Code = 54;
                    result.FilaAtendimentoUsuarioViewModel.mensagem.Message = MensagemPadrao.Message(54).ToString();
                    result.FilaAtendimentoUsuarioViewModel.mensagem.MessageType = MsgType.WARNING;
                }
                else
                {
                    result.FilaAtendimentoUsuarioViewModel.mensagem.Code = 44;
                    result.FilaAtendimentoUsuarioViewModel.mensagem.Message = MensagemPadrao.Message(44).ToString();
                    result.FilaAtendimentoUsuarioViewModel.mensagem.MessageType = MsgType.ERROR;
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                result.FilaAtendimentoUsuarioViewModel.mensagem = new Validate() { Code = 42, Message = MensagemPadrao.Message(42).ToString(), MessageBase = ex.EntityValidationErrors.Select(m => m.ValidationErrors.First().ErrorMessage).First() };
            }
            catch (Exception ex)
            {
                result.FilaAtendimentoUsuarioViewModel.mensagem.Code = 17;
                result.FilaAtendimentoUsuarioViewModel.mensagem.Message = MensagemPadrao.Message(17).ToString();
                result.FilaAtendimentoUsuarioViewModel.mensagem.MessageBase = new App_DominioException(ex.InnerException.InnerException.Message ?? ex.Message, GetType().FullName).Message;
                result.FilaAtendimentoUsuarioViewModel.mensagem.MessageType = MsgType.ERROR;
            }
            return result;
        }

        public IEnumerable<FilaAtendimentoUsuarioEditViewModel> List(params object[] param)
        {
            throw new NotImplementedException();
        }

        public IPagedList PagedList(int? index, int pageSize = 50, params object[] param)
        {
            throw new NotImplementedException();
        }

    }
}