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
using App_Dominio.Repositories;
using App_Dominio.Negocio;
using System.Data.Entity;

namespace DWM.Models.BI
{
    public class AtivarMembroBI : DWMContext<ApplicationContext>, IProcess<UsuarioRepository, ApplicationContext>
    {
        #region Constructor
        public AtivarMembroBI() { }

        public void Create(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.db = _db;
            this.seguranca_db = _seguranca_db;
        }

        public AtivarMembroBI(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            Create(_db, _seguranca_db);
        }
        #endregion

        public UsuarioRepository Run(Repository value)
        {
            UsuarioRepository r = (UsuarioRepository)value;
            try
            {
                int _empresaId = int.Parse(System.Configuration.ConfigurationManager.AppSettings["empresaId"]);

                Usuario entity = null;
                if (seguranca_db.Usuarios.Where(info => info.keyword == r.keyword && info.empresaId == _empresaId).Count() > 0)
                {
                    entity = seguranca_db.Usuarios.Where(info => info.keyword == r.keyword && info.empresaId == _empresaId).FirstOrDefault();
                    // Verifica se o código de ativação não expirou
                    if (entity.dt_keyword < Funcoes.Brasilia())
                        throw new ArgumentException("Data de ativação do cadastro expirou.");
                    else
                    {
                        #region Ativa o Usuário no módulo de Segurança e limpa a chave no cadastro de usuários
                        entity.keyword = null;
                        entity.dt_keyword = null;
                        entity.situacao = "A";

                        seguranca_db.Entry(entity).State = EntityState.Modified;
                        #endregion

                        #region Ativar o Apostador
                        Membro m = db.Membros.Where(info => info.Email == entity.login).FirstOrDefault();
                        MembroModel MembroModel = new MembroModel(this.db, this.seguranca_db);
                        MembroViewModel MembroViewModel = MembroModel.MapToRepository(m);

                        MembroViewModel.IndSituacao = "A";
                        MembroViewModel.uri = r.uri;

                        MembroViewModel = MembroModel.Update(MembroViewModel);
                        if (MembroViewModel.mensagem.Code > 0)
                            throw new ArgumentException(MembroViewModel.mensagem.MessageBase);
                        #endregion

                        r.mensagem = MembroViewModel.mensagem;
                    }
                }
                else
                    throw new ArgumentException("Cadastro já ativado ou código de verificação incorreto");
            }
            catch (ArgumentException ex)
            {
                r.mensagem = new Validate() { Code = 999, Message = MensagemPadrao.Message(999).ToString(), MessageBase = ex.Message };
            }
            catch (App_DominioException ex)
            {
                r.mensagem = ex.Result;

                if (ex.InnerException != null)
                    r.mensagem.MessageBase = new App_DominioException(ex.InnerException.Message ?? ex.Message, GetType().FullName).Message;
                else
                    r.mensagem.MessageBase = new App_DominioException(ex.Result.Message, GetType().FullName).Message;
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

        public IEnumerable<UsuarioRepository> List(params object[] param)
        {
            throw new NotImplementedException();
        }

        public IPagedList PagedList(int? index, int pageSize = 50, params object[] param)
        {
            throw new NotImplementedException();
        }

    }
}