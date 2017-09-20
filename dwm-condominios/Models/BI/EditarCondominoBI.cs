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
using System.Web;

namespace DWM.Models.BI
{
    public class EditarCondominoBI : DWMContextLocal, IProcessAPI<CondominoEditViewModel, ApplicationContext>
    {
        #region Constructor
        public EditarCondominoBI() { }

        public EditarCondominoBI(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            Create(_db, _seguranca_db);
        }

        public EditarCondominoBI(ApplicationContext _db, SecurityContext _seguranca_db, string Token = null)
        {
            Create(_db, _seguranca_db, Token);
        }
        #endregion

        public CondominoEditViewModel Run(Repository value)
        {
            CondominoEditViewModel r = (CondominoEditViewModel)value;
            CondominoEditViewModel result = new CondominoEditViewModel()
            {
                mensagem = new Validate() { Code = 0, Message = "Registro processado com sucesso" }
            };
            try
            {
                int _empresaId = sessaoCorrente.empresaId;

                if (r.CondominoViewModel is CondominoPFViewModel)
                {
                    #region CondominoPF
                    CondominoPFModel condominoPFModel = new CondominoPFModel(this.db, this.seguranca_db);
                    result.CondominoViewModel = condominoPFModel.getObject((CondominoPFViewModel)r.CondominoViewModel);
                    if (result.CondominoViewModel == null)
                    {
                        result.CondominoViewModel = new CondominoPFViewModel()
                        {
                            mensagem = new Validate()
                        };

                        throw new App_DominioException("Acesso não permitido", "Error");
                    }
                    #endregion
                }
                else
                {
                    #region CondominoPJ
                    CondominoPJModel condominoPJModel = new CondominoPJModel(this.db, this.seguranca_db);
                    result.CondominoViewModel = condominoPJModel.getObject((CondominoPJViewModel)r.CondominoViewModel);
                    if (result.CondominoViewModel == null)
                    {
                        result.CondominoViewModel = new CondominoPJViewModel()
                        {
                            mensagem = new Validate()
                        };

                        throw new App_DominioException("Acesso não permitido", "Error");
                    }
                    #endregion
                }

                
                CredenciadoModel CredenciadoModel;
                ListViewCredenciados ListCredenciados;
                VeiculoModel VeiculoModel;
                ListViewVeiculos ListVeiculos;
                FuncionarioModel FuncionarioModel;
                ListViewFuncionarios ListFuncionarios;
                ListViewGrupoCondominoUsuarios ListGrupoCondominoUsuarios;

                if (String.IsNullOrEmpty(value.sessionId))
                {
                    CredenciadoModel = new CredenciadoModel(this.db, this.seguranca_db);
                    ListCredenciados = new ListViewCredenciados(this.db, this.seguranca_db);
                    VeiculoModel = new VeiculoModel(this.db, this.seguranca_db);
                    ListVeiculos = new ListViewVeiculos(this.db, this.seguranca_db);
                    FuncionarioModel = new FuncionarioModel(this.db, this.seguranca_db);
                    ListFuncionarios = new ListViewFuncionarios(this.db, this.seguranca_db);
                    ListGrupoCondominoUsuarios = new ListViewGrupoCondominoUsuarios(this.db, this.seguranca_db);
                }
                else
                {
                    CredenciadoModel = new CredenciadoModel(this.db, this.seguranca_db, value.sessionId);
                    ListCredenciados = new ListViewCredenciados(this.db, this.seguranca_db, value.sessionId);
                    VeiculoModel = new VeiculoModel(this.db, this.seguranca_db, value.sessionId);
                    ListVeiculos = new ListViewVeiculos(this.db, this.seguranca_db, value.sessionId);
                    FuncionarioModel = new FuncionarioModel(this.db, this.seguranca_db, value.sessionId);
                    ListFuncionarios = new ListViewFuncionarios(this.db, this.seguranca_db, value.sessionId);
                    ListGrupoCondominoUsuarios = new ListViewGrupoCondominoUsuarios(this.db, this.seguranca_db, value.sessionId);
                }
                
                result.CredenciadoViewModel = CredenciadoModel.CreateRepository();
                result.CredenciadoViewModel.CondominoID = result.CondominoViewModel.CondominoID;
                result.Credenciados = ListCredenciados.Bind(0, 50, result.CondominoViewModel.CondominoID);
                

                #region Veículos
                
                result.VeiculoViewModel = VeiculoModel.CreateRepository();
                result.VeiculoViewModel.CondominioID = sessaoCorrente.empresaId;
                result.VeiculoViewModel.EdificacaoID = r.UnidadeViewModel.EdificacaoID;
                result.VeiculoViewModel.UnidadeID = r.UnidadeViewModel.UnidadeID;
                result.VeiculoViewModel.CondominoID = result.CondominoViewModel.CondominoID;

                result.Veiculos = ListVeiculos.Bind(0, 50, result.VeiculoViewModel.CondominioID, result.VeiculoViewModel.EdificacaoID, result.VeiculoViewModel.UnidadeID, result.VeiculoViewModel.CondominoID);
                #endregion

                #region Funcionários
                result.FuncionarioViewModel = FuncionarioModel.CreateRepository();
                result.FuncionarioViewModel.CondominioID = sessaoCorrente.empresaId;
                result.FuncionarioViewModel.EdificacaoID = r.UnidadeViewModel.EdificacaoID;
                result.FuncionarioViewModel.UnidadeID = r.UnidadeViewModel.UnidadeID;
                result.FuncionarioViewModel.CondominoID = result.CondominoViewModel.CondominoID;

                result.Funcionarios = ListFuncionarios.Bind(0, 50, result.FuncionarioViewModel.CondominioID, result.FuncionarioViewModel.EdificacaoID, result.FuncionarioViewModel.UnidadeID, result.FuncionarioViewModel.CondominoID);
                #endregion

                #region Grupo de Condôminos
                
                result.GrupoCondominoUsuarios = ListGrupoCondominoUsuarios.Bind(0, 50, result.CondominoViewModel.CondominoID);
                #endregion

                #region Unidade
                result.UnidadeViewModel = new UnidadeViewModel()
                {
                    CondominioID = _empresaId,
                    EdificacaoID = r.UnidadeViewModel.EdificacaoID,
                    EdificacaoDescricao = r.UnidadeViewModel.EdificacaoID > 0 ? db.Edificacaos.Find(r.UnidadeViewModel.EdificacaoID).Descricao : "",
                    UnidadeID = r.UnidadeViewModel.UnidadeID,
                    Codigo = db.Unidades.Find(_empresaId, r.UnidadeViewModel.EdificacaoID, r.UnidadeViewModel.UnidadeID).Codigo,
                };
                #endregion

                if (SessaoLocal.CondominoID > 0)
                {
                    #region Valida permissão do usuário para editar o condômino
                    if (r.CondominoViewModel.CondominoID != SessaoLocal.CondominoID)
                        throw new App_DominioException("Acesso não permitido", "Error");
                    bool flag = false;
                    foreach (Unidade unidade in SessaoLocal.Unidades)
                    {
                        if (r.UnidadeViewModel.UnidadeID == unidade.UnidadeID && r.UnidadeViewModel.EdificacaoID == unidade.EdificacaoID)
                            flag = true;
                    }
                    if (!flag)
                        throw new App_DominioException("Acesso não permitido", "Error");
                    #endregion
                }

            }
            catch (ArgumentException ex)
            {
                result.CondominoViewModel.mensagem = new Validate() { Code = 999, Message = MensagemPadrao.Message(999).ToString(), MessageBase = ex.Message };
            }
            catch (App_DominioException ex)
            {
                result.CondominoViewModel.mensagem.Code = 999;

                if (ex.InnerException != null)
                    result.CondominoViewModel.mensagem.MessageBase = new App_DominioException(ex.InnerException.Message ?? ex.Message, GetType().FullName).Message;
                else
                    result.CondominoViewModel.mensagem.MessageBase = new App_DominioException(ex.Message, GetType().FullName).Message;
            }
            catch (DbUpdateException ex)
            {
                result.CondominoViewModel.mensagem.MessageBase = ex.InnerException.InnerException.Message ?? ex.Message;
                if (result.CondominoViewModel.mensagem.MessageBase.ToUpper().Contains("REFERENCE"))
                {
                    if (result.CondominoViewModel.mensagem.MessageBase.ToUpper().Contains("DELETE"))
                    {
                        result.CondominoViewModel.mensagem.Code = 16;
                        result.CondominoViewModel.mensagem.Message = MensagemPadrao.Message(16).ToString();
                        result.CondominoViewModel.mensagem.MessageType = MsgType.ERROR;
                    }
                    else
                    {
                        result.CondominoViewModel.mensagem.Code = 28;
                        result.CondominoViewModel.mensagem.Message = MensagemPadrao.Message(28).ToString();
                        result.CondominoViewModel.mensagem.MessageType = MsgType.ERROR;
                    }
                }
                else if (result.CondominoViewModel.mensagem.MessageBase.ToUpper().Contains("PRIMARY"))
                {
                    result.CondominoViewModel.mensagem.Code = 37;
                    result.CondominoViewModel.mensagem.Message = MensagemPadrao.Message(37).ToString();
                    result.CondominoViewModel.mensagem.MessageType = MsgType.WARNING;
                }
                else if (result.CondominoViewModel.mensagem.MessageBase.ToUpper().Contains("UNIQUE KEY"))
                {
                    result.CondominoViewModel.mensagem.Code = 54;
                    result.CondominoViewModel.mensagem.Message = MensagemPadrao.Message(54).ToString();
                    result.CondominoViewModel.mensagem.MessageType = MsgType.WARNING;
                }
                else
                {
                    result.CondominoViewModel.mensagem.Code = 44;
                    result.CondominoViewModel.mensagem.Message = MensagemPadrao.Message(44).ToString();
                    result.CondominoViewModel.mensagem.MessageType = MsgType.ERROR;
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                result.CondominoViewModel.mensagem = new Validate() { Code = 42, Message = MensagemPadrao.Message(42).ToString(), MessageBase = ex.EntityValidationErrors.Select(m => m.ValidationErrors.First().ErrorMessage).First() };
            }
            catch (Exception ex)
            {
                result.CondominoViewModel.mensagem.Code = 17;
                result.CondominoViewModel.mensagem.Message = MensagemPadrao.Message(17).ToString();
                result.CondominoViewModel.mensagem.MessageBase = new App_DominioException(ex.InnerException.InnerException.Message ?? ex.Message, GetType().FullName).Message;
                result.CondominoViewModel.mensagem.MessageType = MsgType.ERROR;
            }
            return result;
        }

        public IEnumerable<CondominoEditViewModel> List(params object[] param)
        {
            throw new NotImplementedException();
        }

        public IPagedList PagedList(int? index, int pageSize = 50, params object[] param)
        {
            throw new NotImplementedException();
        }

    }
}