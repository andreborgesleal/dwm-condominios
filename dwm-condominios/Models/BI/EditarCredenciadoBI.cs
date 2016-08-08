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
    public class EditarCredenciadoBI : DWMContext<ApplicationContext>, IProcess<CredenciadoViewModel, ApplicationContext>
    {
        #region Constructor
        public EditarCredenciadoBI() { }

        public EditarCredenciadoBI(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            Create(_db, _seguranca_db);
        }
        #endregion

        public CredenciadoViewModel Run(Repository value)
        {
            CredenciadoViewModel r = (CredenciadoViewModel)value;
            CredenciadoViewModel result = new CredenciadoViewModel()
            {
                mensagem = new Validate() { Code = 0, Message = "Registro processado com sucesso" }
            };
            try
            {
                int _empresaId = sessaoCorrente.empresaId;

                CredenciadoModel CredenciadoModel = new CredenciadoModel(this.db, this.seguranca_db);
                if (r.CredenciadoID == 0)
                    result = CredenciadoModel.Insert(r);
                else
                    result = CredenciadoModel.Update(r);
            }
            catch (ArgumentException ex)
            {
                result.CondominoPFViewModel.mensagem = new Validate() { Code = 999, Message = MensagemPadrao.Message(999).ToString(), MessageBase = ex.Message };
            }
            catch (App_DominioException ex)
            {
                result.CondominoPFViewModel.mensagem = ex.Result;

                if (ex.InnerException != null)
                    result.CondominoPFViewModel.mensagem.MessageBase = new App_DominioException(ex.InnerException.Message ?? ex.Message, GetType().FullName).Message;
                else
                    result.CondominoPFViewModel.mensagem.MessageBase = new App_DominioException(ex.Result.Message, GetType().FullName).Message;
            }
            catch (DbUpdateException ex)
            {
                result.CondominoPFViewModel.mensagem.MessageBase = ex.InnerException.InnerException.Message ?? ex.Message;
                if (result.CondominoPFViewModel.mensagem.MessageBase.ToUpper().Contains("REFERENCE"))
                {
                    if (result.CondominoPFViewModel.mensagem.MessageBase.ToUpper().Contains("DELETE"))
                    {
                        result.CondominoPFViewModel.mensagem.Code = 16;
                        result.CondominoPFViewModel.mensagem.Message = MensagemPadrao.Message(16).ToString();
                        result.CondominoPFViewModel.mensagem.MessageType = MsgType.ERROR;
                    }
                    else
                    {
                        result.CondominoPFViewModel.mensagem.Code = 28;
                        result.CondominoPFViewModel.mensagem.Message = MensagemPadrao.Message(28).ToString();
                        result.CondominoPFViewModel.mensagem.MessageType = MsgType.ERROR;
                    }
                }
                else if (result.CondominoPFViewModel.mensagem.MessageBase.ToUpper().Contains("PRIMARY"))
                {
                    result.CondominoPFViewModel.mensagem.Code = 37;
                    result.CondominoPFViewModel.mensagem.Message = MensagemPadrao.Message(37).ToString();
                    result.CondominoPFViewModel.mensagem.MessageType = MsgType.WARNING;
                }
                else if (result.CondominoPFViewModel.mensagem.MessageBase.ToUpper().Contains("UNIQUE KEY"))
                {
                    result.CondominoPFViewModel.mensagem.Code = 54;
                    result.CondominoPFViewModel.mensagem.Message = MensagemPadrao.Message(54).ToString();
                    result.CondominoPFViewModel.mensagem.MessageType = MsgType.WARNING;
                }
                else
                {
                    result.CondominoPFViewModel.mensagem.Code = 44;
                    result.CondominoPFViewModel.mensagem.Message = MensagemPadrao.Message(44).ToString();
                    result.CondominoPFViewModel.mensagem.MessageType = MsgType.ERROR;
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                result.CondominoPFViewModel.mensagem = new Validate() { Code = 42, Message = MensagemPadrao.Message(42).ToString(), MessageBase = ex.EntityValidationErrors.Select(m => m.ValidationErrors.First().ErrorMessage).First() };
            }
            catch (Exception ex)
            {
                result.CondominoPFViewModel.mensagem.Code = 17;
                result.CondominoPFViewModel.mensagem.Message = MensagemPadrao.Message(17).ToString();
                result.CondominoPFViewModel.mensagem.MessageBase = new App_DominioException(ex.InnerException.InnerException.Message ?? ex.Message, GetType().FullName).Message;
                result.CondominoPFViewModel.mensagem.MessageType = MsgType.ERROR;
            }
            return result;
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
}