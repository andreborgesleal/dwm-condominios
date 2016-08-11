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
using System.Data.Entity;

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
                uri = r.uri,
                empresaId = sessaoCorrente.empresaId,
                CredenciadoID = r.CredenciadoID,
                CondominoID = r.CondominoID,
                Nome = r.Nome,
                Email = r.Email,
                TipoCredenciadoID = r.TipoCredenciadoID,
                Sexo = r.Sexo,
                Observacao = r.Observacao,
                UsuarioID = r.UsuarioID,
                mensagem = new Validate() { Code = 0, Message = "Registro processado com sucesso" }
            };

            try
            {
                int _empresaId = sessaoCorrente.empresaId;

                CredenciadoModel CredenciadoModel = new CredenciadoModel(this.db, this.seguranca_db);

                if (r.CredenciadoID == 0) // Incluir credenciado
                {
                    #region Validar Credenciado
                    if (CredenciadoModel.Validate(result, Crud.INCLUIR).Code > 0)
                        throw new App_DominioException(result.mensagem);
                    #endregion

                    #region Cadastrar o credenciado como um usuário em DWM-Segurança

                    Random random = new Random();
                    string _senha = random.Next(9999, 999999).ToString();
                    string _keyword = random.Next(9999, 99999999).ToString();
                    int _grupoId = int.Parse(db.Parametros.Find(_empresaId, (int)Enumeracoes.Enumeradores.Param.GRUPO_CREDENCIADO).Valor);

                    #region Usuario 
                    EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();

                    Usuario user = new Usuario()
                    {
                        nome = r.Nome.ToUpper(),
                        login = r.Email,
                        empresaId = _empresaId,
                        dt_cadastro = Funcoes.Brasilia(),
                        situacao = "D",
                        isAdmin = "N",
                        senha = security.Criptografar(_senha),
                        keyword = _keyword,
                        dt_keyword = Funcoes.Brasilia().AddDays(1)
                    };

                    // Verifica se o E-mail do usuário já não existe para a empresa
                    if (seguranca_db.Usuarios.Where(info => info.empresaId == _empresaId && info.login == r.Email).Count() > 0)
                        throw new ArgumentException("E-mail já cadastrado");

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

                    seguranca_db.SaveChanges();

                    #endregion

                    #region Incluir o credenciado
                    result.UsuarioID = user.usuarioId;
                    result = CredenciadoModel.Insert(result);
                    result.mensagem.Field = _keyword;
                    #endregion
                }
                else
                {
                    #region Validar Credenciado
                    if (CredenciadoModel.Validate(result, Crud.ALTERAR).Code > 0)
                        throw new App_DominioException(result.mensagem);
                    #endregion

                    #region Atualiza o cadastro do usuário
                    Usuario user = seguranca_db.Usuarios.Find(result.UsuarioID);

                    user.login = result.Email;
                    user.nome = result.Nome.ToUpper();
                    user.dt_cadastro = Funcoes.Brasilia();

                    seguranca_db.Entry(user).State = EntityState.Modified;

                    seguranca_db.SaveChanges();
                    #endregion

                    #region Alterar credenciado
                    result = CredenciadoModel.Update(result);
                    #endregion
                }

                db.SaveChanges();

                if (result.mensagem.Code > 0)
                    throw new App_DominioException(result.mensagem);

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

        public IEnumerable<CredenciadoViewModel> List(params object[] param)
        {
            ListViewCredenciados ListCredenciados = new ListViewCredenciados(this.db, this.seguranca_db);
            return ListCredenciados.Bind(0, 50, param);
        }

        public IPagedList PagedList(int? index, int pageSize = 50, params object[] param)
        {
            throw new NotImplementedException();
        }

    }
}