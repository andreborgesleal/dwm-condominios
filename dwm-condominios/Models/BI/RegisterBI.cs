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

            return value.mensagem;

            #endregion
        }

        public RegisterViewModel Run(Repository value)
        {
            RegisterViewModel r = (RegisterViewModel)value;
            try
            {
                EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
                int _empresaId = int.Parse(System.Configuration.ConfigurationManager.AppSettings["empresaId"]);

                #region validar senha
                Validate validate = Validate(r);
                if (validate.Code > 0)
                    throw new ArgumentException(validate.MessageBase);
                #endregion

                #region Incluir o Apostador (Membro) e a sua conta corrente virtual
                ContaVirtualViewModel contaVirtualViewModel = new ContaVirtualViewModel()
                {
                    uri = r.uri,
                    empresaId = _empresaId,
                    TipoContaID = 1, // 1-Conta corrente 
                    DataAbertura = Funcoes.Brasilia().Date,
                    ValorSaldo = 0,
                    MembroViewModel = new MembroViewModel()
                    {
                        uri = r.uri,
                        empresaId = _empresaId,
                        Nome = r.Nome,
                        Email = r.Email,
                        Telefone = r.Telefone,
                        CPF = r.CPF,
                        Banco = r.Banco,
                        Agencia = r.Agencia,
                        Conta = r.Conta,
                        IndSituacao = "D"
                    }
                };

                ContaVirtualModel contaVirtualModel = new ContaVirtualModel(this.db, this.seguranca_db);
                contaVirtualViewModel = contaVirtualModel.Insert(contaVirtualViewModel);
                if (contaVirtualViewModel.mensagem.Code > 0)
                    throw new App_DominioException(contaVirtualViewModel.mensagem);
                #endregion

                #region Cadastrar o apostador como um usuário em DWM-Segurança

                #region Usuario 
                Random random = new Random(DateTime.Now.Millisecond);
                string n_aleatorio = random.Next(1000, 80000).ToString();

                Usuario user = new Usuario()
                {
                    nome = r.Nome,
                    login = r.Email,
                    empresaId = _empresaId,
                    dt_cadastro = Funcoes.Brasilia(),
                    situacao = "D",
                    isAdmin = "N",
                    senha = security.Criptografar(r.senha),
                    keyword = n_aleatorio,
                    dt_keyword = Funcoes.Brasilia().AddDays(1)
                };

                seguranca_db.Usuarios.Add(user);
                #endregion

                #region UsuarioGrupo
                int _grupoId = seguranca_db.Grupos.Where(info => info.empresaId == _empresaId && info.descricao == "Apostador").Select(info => info.grupoId).FirstOrDefault();

                UsuarioGrupo ug = new UsuarioGrupo()
                {
                    Usuario = user,
                    grupoId = _grupoId,
                    situacao = "A"
                };

                seguranca_db.UsuarioGrupos.Add(ug);
                #endregion

                #endregion

                r.MembroID = contaVirtualViewModel.MembroViewModel.MembroID;
                r.IndSituacao = contaVirtualViewModel.MembroViewModel.IndSituacao;
                r.Keyword = n_aleatorio;
                r.mensagem = contaVirtualViewModel.mensagem;
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