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
    public class RegisterBI : DWMContext<ApplicationContext>, IProcess<RegisterViewModel, ApplicationContext>
    {
        #region Constructor
        public RegisterBI() { }

        public void Create(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.db = _db;
            this.seguranca_db = _seguranca_db;
        }

        public RegisterBI(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            Create(_db, _seguranca_db);
        }
        #endregion

        private Validate Validate(RegisterViewModel value)
        {
            value.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString() };

            #region validar senha e confirmação de senha
            if (value.senha == null || value.senha == "")
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Senha").ToString();
                value.mensagem.MessageBase = "Senha deve ser informada";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.senha != value.confirmacaoSenha)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Senha").ToString();
                value.mensagem.MessageBase = "Senha e confirmação de senha não conferem";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }
            #endregion

            #region Validar se a unidade está ocupada por outro condômino
            if (db.CondominoUnidades.Where(info => info.CondominioID == value.UnidadeViewModel.CondominioID &&
                                                   info.EdificacaoID == value.UnidadeViewModel.EdificacaoID &&
                                                   info.UnidadeID == value.UnidadeViewModel.UnidadeID &&
                                                   info.DataFim == null).Count() > 0)
            {
                value.mensagem.Code = 57;
                value.mensagem.Message = MensagemPadrao.Message(57).text;
                value.mensagem.MessageBase = "Unidade está ocupada por outro condômino. Favor entrar em contato com a administração";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }
            #endregion

            #region Validar código de ativação
            if (value.UnidadeViewModel.Validador.Trim().Length > 0)
            {
                Unidade u = db.Unidades.Find(value.UnidadeViewModel.CondominioID, value.UnidadeViewModel.EdificacaoID, value.UnidadeViewModel.UnidadeID);
                if (u.Validador != value.UnidadeViewModel.Validador || (u.Validador == value.UnidadeViewModel.Validador && Funcoes.Brasilia().Date > u.DataExpiracao))
                {
                    value.mensagem.Code = 58;
                    value.mensagem.Message = MensagemPadrao.Message(58).text;
                    value.mensagem.MessageBase = "Código de ativação da unidade inválido. Favor entrar em contato com a administração";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
            }

            #endregion

            return value.mensagem;
        }

        public RegisterViewModel Run(Repository value)
        {
            RegisterViewModel r = (RegisterViewModel)value;
            try
            {
                int _empresaId = int.Parse(db.Parametros.Find(r.CondominioID, (int)Enumeracoes.Enumeradores.Param.EMPRESA).Valor);
                int _grupoId = int.Parse(db.Parametros.Find(r.CondominioID, (int)Enumeracoes.Enumeradores.Param.GRUPO_USUARIO).Valor);

                #region validar cadastro
                Validate validate = Validate(r);
                if (validate.Code > 0)
                    throw new ArgumentException(validate.MessageBase);
                #endregion

                #region Cadastrar o condômino como um usuário em DWM-Segurança

                #region Usuario 
                EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();

                Usuario user = new Usuario()
                {
                    nome = r.Nome,
                    login = r.Email,
                    empresaId = _empresaId,
                    dt_cadastro = Funcoes.Brasilia(),
                    situacao = "A",
                    isAdmin = "N",
                    senha = security.Criptografar(r.senha)
                };

                seguranca_db.Usuarios.Add(user);
                #endregion

                #region UsuarioGrupo
                UsuarioGrupo ug = new UsuarioGrupo()
                {
                    Usuario = user,
                    grupoId = _grupoId,
                    situacao = "A"
                };

                seguranca_db.UsuarioGrupos.Add(ug);
                #endregion

                #endregion

                #region Incluir o Condômino
                CondominoPFViewModel condominoPFViewModel = new CondominoPFViewModel()
                {
                    uri = r.uri,
                    empresaId = _empresaId,
                    CondominoID = r.CondominoID,
                    CondominioID = r.CondominioID,
                    Nome = r.Nome,
                    IndFiscal = r.IndFiscal,
                    IndProprietario = r.IndProprietario,
                    TelParticular1 = r.TelParticular1,
                    TelParticular2 = r.TelParticular2,
                    Email = r.Email,
                    IndSituacao = r.UnidadeViewModel.Validador.Trim().Length > 0 ? "A" : "D",
                    UsuarioID = user.usuarioId,
                    ProfissaoID = r.ProfissaoID,
                    DataNascimento = r.DataNascimento,
                    Sexo = r.Sexo,
                    IndAnimal = r.IndAnimal,
                    CondominoUnidadeViewModel = new CondominoUnidadeViewModel()
                    {
                        uri = r.uri,
                        CondominioID = r.UnidadeViewModel.CondominioID,
                        EdificacaoID = r.UnidadeViewModel.EdificacaoID,
                        UnidadeID = r.UnidadeViewModel.UnidadeID,
                        CondominoID = r.CondominoID,
                        DataInicio = Funcoes.Brasilia().Date
                    }
                };

                CondominoPFModel condominoPFModel = new CondominoPFModel(this.db, this.seguranca_db);
                condominoPFViewModel = condominoPFModel.Insert(condominoPFViewModel);
                if (condominoPFViewModel.mensagem.Code > 0)
                    throw new App_DominioException(condominoPFViewModel.mensagem);
                #endregion

                r.CondominoID = condominoPFViewModel.CondominoID;
                r.CondominoUnidadeViewModel.CondominioID = condominoPFViewModel.CondominoID;
                r.IndSituacao = condominoPFViewModel.IndSituacao;
                r.mensagem = condominoPFViewModel.mensagem;
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