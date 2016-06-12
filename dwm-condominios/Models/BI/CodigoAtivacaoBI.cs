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
    public class CodigoAtivacaoBI : DWMContext<ApplicationContext>, IProcess<RegisterViewModel, ApplicationContext>
    {
        #region Constructor
        public CodigoAtivacaoBI() { }

        public void Create(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.db = _db;
            this.seguranca_db = _seguranca_db;
        }

        public CodigoAtivacaoBI(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            Create(_db, _seguranca_db);
        }
        #endregion

        public RegisterViewModel Run(Repository value)
        {
            RegisterViewModel r = (RegisterViewModel)value;
            try
            {
                #region Buscar os dados da unidade pelo Validador
                Unidade u = db.Unidades.Where(info => info.Validador == r.UnidadeViewModel.Validador).FirstOrDefault();

                RegisterViewModel registerViewModel = new RegisterViewModel();

                if (u != null)
                {
                    registerViewModel.CondominioID = u.CondominioID;
                    registerViewModel.Nome = u.NomeCondomino;
                    registerViewModel.Email = u.Email;
                    registerViewModel.UnidadeViewModel = new UnidadeViewModel()
                    {
                        CondominioID = u.CondominioID,
                        EdificacaoID = u.EdificacaoID,
                        UnidadeID = u.UnidadeID,
                        Validador = u.Validador,
                        EdificacaoDescricao = db.Edificacaos.Find(u.EdificacaoID).Descricao
                    };
                    registerViewModel.CondominoUnidadeViewModel = new CondominoUnidadeViewModel()
                    {
                        CondominioID = u.CondominioID,
                        EdificacaoID = u.EdificacaoID,
                        UnidadeID = u.UnidadeID
                    };
                }
                #endregion
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

        public IEnumerable<RegisterViewModel> List(params object[] param)
        {
            throw new NotImplementedException();
        }

        public IPagedList PagedList(int? index, int pageSize = 50, params object[] param)
        {
            throw new NotImplementedException();
        }
    }
}