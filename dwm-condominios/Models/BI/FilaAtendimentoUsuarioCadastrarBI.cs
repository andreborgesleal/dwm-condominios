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
    public class FilaAtendimentoUsuarioCadastrarBI : DWMContextLocal, IProcess<FilaAtendimentoUsuarioViewModel, ApplicationContext>
    {
        private string Operacao { get; set; }

        #region Constructor
        public FilaAtendimentoUsuarioCadastrarBI(string operacao)
        {
            this.Operacao = operacao;
        }

        public FilaAtendimentoUsuarioCadastrarBI() : base()
        {
        }

        public FilaAtendimentoUsuarioCadastrarBI(ApplicationContext _db, SecurityContext _segurancaDb)
        {
            this.Create(_db, _segurancaDb);
        }

        #endregion

        public FilaAtendimentoUsuarioViewModel Run(Repository value)
        {
            FilaAtendimentoUsuarioViewModel r = (FilaAtendimentoUsuarioViewModel)value;
            FilaAtendimentoUsuarioViewModel result = new FilaAtendimentoUsuarioViewModel()
            {
                uri = r.uri,
                empresaId = sessaoCorrente.empresaId,
                FilaAtendimentoID = r.FilaAtendimentoID,
                UsuarioID = r.UsuarioID,
                Situacao = r.Situacao,
                mensagem = new Validate() { Code = 0, Message = "Registro processado com sucesso" }
            };

            try
            {
                int _empresaId = SessaoLocal.empresaId;

                FilaAtendimentoUsuarioModel FilaAtendimentoUsuarioModel = new FilaAtendimentoUsuarioModel(this.db, this.seguranca_db);

                if (Operacao == "I") // Incluir credenciado
                {
                    #region Incluir o Usuário na Fila
                    result = FilaAtendimentoUsuarioModel.Insert(result);
                    #endregion
                }
                else if (Operacao == "A") // Alterar credenciado
                {
                    #region Alterar situação do usuário na fila
                    result = FilaAtendimentoUsuarioModel.Update(result);
                    #endregion
                }
                else // Excluir credenciado
                {
                    #region Excluir Usuário da Fila
                    result = FilaAtendimentoUsuarioModel.Delete(result);
                    #endregion
                }

                if (result.mensagem.Code > 0)
                    throw new App_DominioException(result.mensagem);

                db.SaveChanges();

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

        public IEnumerable<FilaAtendimentoUsuarioViewModel> List(params object[] param)
        {
            ListViewFilaAtendimentoUsuario ListFilaAtendimentoUsuarios = new ListViewFilaAtendimentoUsuario(this.db, this.seguranca_db);
            return ListFilaAtendimentoUsuarios.Bind(0, 50, param);
        }

        public IPagedList PagedList(int? index, int pageSize = 50, params object[] param)
        {
            throw new NotImplementedException();
        }

    }
}