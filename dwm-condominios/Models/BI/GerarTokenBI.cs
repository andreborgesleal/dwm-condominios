﻿using System;
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
    public class GerarTokenBI : DWMContextLocal, IProcess<UnidadeViewModel, ApplicationContext>
    {
        #region Constructor
        public GerarTokenBI() { }

        public GerarTokenBI(ApplicationContext _db, SecurityContext _segurancaDb)
        {
            this.Create(_db, _segurancaDb);
        }

        #endregion

        public UnidadeViewModel Run(Repository value)
        {
            UnidadeViewModel r = (UnidadeViewModel)value;
            UnidadeViewModel result = new UnidadeViewModel();
            try
            {
                Guid guid = Guid.NewGuid();
                string Validador = guid.ToString();

                UnidadeModel model = new UnidadeModel(this.db, this.seguranca_db);
                Unidade u = model.Find(r);
                result = model.MapToRepository(u);
                result.uri = r.uri;
                result.Validador = Validador;
                result.DataExpiracao = Funcoes.Brasilia().Date.AddDays(2);
                result.NomeCondomino = r.NomeCondomino != null ? r.NomeCondomino.ToUpper() : "";
                result.Email = r.Email != null ? r.Email.ToLower() : "";

                #region Verifica se a unidade está ocupada 
                int quantidade = db.CondominoUnidades.Where(info => info.CondominioID == sessaoCorrente.empresaId
                                                                    && info.EdificacaoID == r.EdificacaoID
                                                                    && info.UnidadeID == r.UnidadeID
                                                                    && info.DataFim == null).Count();
                if (quantidade > 0)
                {
                    result.mensagem = new Validate() { Code = 19, Message = "A unidade informada já se encontra ocupada por outro condômino. É necessário desocupar a unidade para executar o envio do Token de cadastro." };
                    throw new App_DominioException(result.mensagem);
                }
                    
                #endregion

                result = model.Update(result);

                if (result.mensagem.Code > 0)
                    throw new App_DominioException(result.mensagem);

                db.SaveChanges();
                seguranca_db.SaveChanges();
                #region envio de e-mail ao condômino para registro

                EnviarEmailTokenBI EnviarEmailToken = new EnviarEmailTokenBI(this.db, this.seguranca_db);
                result.EdificacaoDescricao = db.Edificacaos.Find(result.EdificacaoID).Descricao;
                result.EdificacaoDescricaoTipoEdificacao = DWMSessaoLocal._GetTipoEdificacao(result.CondominioID, this.db).Descricao;
                result = EnviarEmailToken.Run(result);
                if (result.mensagem.Code > 0)
                    throw new ArgumentException(result.mensagem.MessageBase);
                #endregion

                result.mensagem.Code = -1; // Tem que devolver -1 porque na Superclasse, se devolver zero, vai executar novamente o SaveChanges.
                result.mensagem.Field = Validador;
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

        public IEnumerable<UnidadeViewModel> List(params object[] param)
        {
            throw new NotImplementedException();
        }

        public IPagedList PagedList(int? index, int pageSize = 50, params object[] param)
        {
            throw new NotImplementedException();
        }
    }
}