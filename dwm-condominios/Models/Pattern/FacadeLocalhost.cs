using App_Dominio.Component;
using App_Dominio.Contratos;
using App_Dominio.Entidades;
using App_Dominio.Enumeracoes;
using App_Dominio.Pattern;
using App_Dominio.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using DWM.Models.Entidades;

namespace DWM.Models.Pattern
{
    public class FacadeLocalhost<R, T, D> : Facade<R, T, D>
        where R : Repository
        where T : ICrudModelLocal<R, D> 
        where D : DbContext
    {
        public override R Save(R value, Crud operation)
        {
            using (db = getContextInstance())
            {
                using (seguranca_db = new SecurityContext())
                {
                    try
                    {
                        string Token = null;

                        if (!String.IsNullOrEmpty(value.sessionId))
                            Token = value.sessionId;

                        getSessao(Token);

                        value.empresaId = sessaoCorrente.empresaId;

                        T model = getModel();
                        model.Open(db, seguranca_db, Token);

                        if (operation == Crud.INCLUIR)
                            value = model.Insert(value);
                        else if (operation == Crud.ALTERAR)
                            value = model.Update(value);
                        else if (operation == Crud.EXCLUIR)
                            value = model.Delete(value);

                        if (value.mensagem.Code == 0)
                        {
                            db.SaveChanges();
                            seguranca_db.SaveChanges();
                        }
                    }
                    catch (ArgumentException ex)
                    {
                        value.mensagem = new Validate() { Code = 999, Message = MensagemPadrao.Message(999).ToString(), MessageBase = ex.Message };
                    }
                    catch (DbUpdateException ex)
                    {
                        value.mensagem.MessageBase = ex.InnerException.InnerException.Message ?? ex.Message;
                        if (value.mensagem.MessageBase.ToUpper().Contains("REFERENCE"))
                        {
                            if (value.mensagem.MessageBase.ToUpper().Contains("DELETE"))
                            {
                                value.mensagem.Code = 16;
                                value.mensagem.Message = MensagemPadrao.Message(16).ToString();
                                value.mensagem.MessageType = MsgType.ERROR;
                            }
                            else
                            {
                                value.mensagem.Code = 28;
                                value.mensagem.Message = MensagemPadrao.Message(28).ToString();
                                value.mensagem.MessageType = MsgType.ERROR;
                            }
                        }
                        else if (value.mensagem.MessageBase.ToUpper().Contains("PRIMARY"))
                        {
                            value.mensagem.Code = 37;
                            value.mensagem.Message = MensagemPadrao.Message(37).ToString();
                            value.mensagem.MessageType = MsgType.WARNING;
                        }
                        else if (value.mensagem.MessageBase.ToUpper().Contains("UNIQUE KEY"))
                        {
                            value.mensagem.Code = 54;
                            value.mensagem.Message = MensagemPadrao.Message(54).ToString();
                            value.mensagem.MessageType = MsgType.WARNING;
                        }
                        else
                        {
                            value.mensagem.Code = 44;
                            value.mensagem.Message = MensagemPadrao.Message(44).ToString();
                            value.mensagem.MessageType = MsgType.ERROR;
                        }
                    }
                    catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                    {
                        value.mensagem = new Validate() { Code = 42, Message = MensagemPadrao.Message(42).ToString(), MessageBase = ex.EntityValidationErrors.Select(m => m.ValidationErrors.First().ErrorMessage).First() };
                    }
                    catch (Exception ex)
                    {
                        value.mensagem.Code = 17;
                        value.mensagem.Message = MensagemPadrao.Message(17).ToString();
                        value.mensagem.MessageBase = new App_DominioException(ex.InnerException.InnerException.Message ?? ex.Message, GetType().FullName).Message;
                        value.mensagem.MessageType = MsgType.ERROR;
                    }
                }
            }

            return value;
        }

    }
}