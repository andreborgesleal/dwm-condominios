using System;
using System.Collections.Generic;
using App_Dominio.Contratos;
using App_Dominio.Entidades;
using App_Dominio.Component;
using DWM.Models.Repositories;
using DWM.Models.Entidades;
using System.Web.Mvc;
using App_Dominio.Enumeracoes;
using App_Dominio.Security;
using System.Linq;
using System.Data.Entity.Infrastructure;
using DWM.Models.Persistence;

namespace DWM.Models.BI
{
    public class CondominoCorrenteBI : DWMContextLocal, IProcess<CondominoViewModel, ApplicationContext>
    {
        #region Constructor
        public CondominoCorrenteBI() { }

        public void Create(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.db = _db;
            this.seguranca_db = _seguranca_db;
        }

        public CondominoCorrenteBI(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            Create(_db, _seguranca_db);
        }
        #endregion

        public CondominoViewModel Run(Repository value)
        {
            CondominoViewModel c;
            CondominoViewModel r;

            if (value is CondominoPFViewModel)
            {
                c = (CondominoPFViewModel)value;
                r = new CondominoPFViewModel();
            }
            else
            {
                c = (CondominoPJViewModel)value;
                r = new CondominoPJViewModel();
            }
            try
            {
                if (SessaoLocal.CondominoID > 0)
                {
                    r.CondominoID = SessaoLocal.CondominoID;
                    if (value is CondominoPFViewModel)
                    {
                        CondominoPFModel model = new CondominoPFModel(this.db, this.seguranca_db);
                        r = model.getObject((CondominoPFViewModel)r);
                    }
                    else
                    {
                        // escrever aqui o código para recuperar o condomino PJ
                    }
                }
                else
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

        public IEnumerable<CondominoViewModel> List(params object[] param)
        {
            throw new NotImplementedException();
        }

        public IPagedList PagedList(int? index, int pageSize = 50, params object[] param)
        {
            throw new NotImplementedException();
        }
    }
}