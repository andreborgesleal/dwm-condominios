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
    public class EditarCondominoBI : DWMContextLocal, IProcess<CondominoEditViewModel, ApplicationContext>
    {
        #region Constructor
        public EditarCondominoBI() { }

        public EditarCondominoBI(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            Create(_db, _seguranca_db);
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

                #region CondominoPF
                CondominoPFModel condominoPFModel = new CondominoPFModel(this.db, this.seguranca_db);
                result.CondominoPFViewModel = condominoPFModel.getObject(r.CondominoPFViewModel);
                if (result.CondominoPFViewModel == null)
                {
                    result.CondominoPFViewModel = new CondominoPFViewModel()
                    {
                        mensagem = new Validate()
                    };

                    throw new App_DominioException("Acesso não permitido", "Error");
                }
                    
                #endregion

                #region Credenciado
                CredenciadoModel CredenciadoModel = new CredenciadoModel(this.db, this.seguranca_db);
                result.CredenciadoViewModel = CredenciadoModel.CreateRepository();
                result.CredenciadoViewModel.CondominoID = result.CondominoPFViewModel.CondominoID;

                ListViewCredenciados ListCredenciados = new ListViewCredenciados(this.db, this.seguranca_db);
                result.Credenciados = ListCredenciados.Bind(0, 50, result.CondominoPFViewModel.CondominoID);
                #endregion

                #region Veículos
                VeiculoModel VeiculoModel = new VeiculoModel(this.db, this.seguranca_db);
                result.VeiculoViewModel = VeiculoModel.CreateRepository();
                result.VeiculoViewModel.CondominioID = sessaoCorrente.empresaId;
                result.VeiculoViewModel.EdificacaoID = r.UnidadeViewModel.EdificacaoID;
                result.VeiculoViewModel.UnidadeID = r.UnidadeViewModel.UnidadeID;
                result.VeiculoViewModel.CondominoID = result.CondominoPFViewModel.CondominoID;

                ListViewVeiculos ListVeiculos = new ListViewVeiculos(this.db, this.seguranca_db);
                result.Veiculos = ListVeiculos.Bind(0, 50, result.VeiculoViewModel.CondominioID, result.VeiculoViewModel.EdificacaoID, result.VeiculoViewModel.UnidadeID, result.VeiculoViewModel.CondominoID);
                #endregion

                #region Funcionários
                FuncionarioModel FuncionarioModel = new FuncionarioModel(this.db, this.seguranca_db);
                result.FuncionarioViewModel = FuncionarioModel.CreateRepository();
                result.FuncionarioViewModel.CondominioID = sessaoCorrente.empresaId;
                result.FuncionarioViewModel.EdificacaoID = r.UnidadeViewModel.EdificacaoID;
                result.FuncionarioViewModel.UnidadeID = r.UnidadeViewModel.UnidadeID;
                result.FuncionarioViewModel.CondominoID = result.CondominoPFViewModel.CondominoID;

                ListViewFuncionarios ListFuncionarios = new ListViewFuncionarios(this.db, this.seguranca_db);
                result.Funcionarios = ListFuncionarios.Bind(0, 50, result.FuncionarioViewModel.CondominioID, result.FuncionarioViewModel.EdificacaoID, result.FuncionarioViewModel.UnidadeID, result.FuncionarioViewModel.CondominoID);
                #endregion

                #region Grupo de Condôminos
                ListViewGrupoCondominoUsuarios ListGrupoCondominoUsuarios = new ListViewGrupoCondominoUsuarios(this.db, this.seguranca_db);
                result.GrupoCondominoUsuarios = ListGrupoCondominoUsuarios.Bind(0, 50, result.CondominoPFViewModel.CondominoID);
                #endregion

                result.UnidadeViewModel = new UnidadeViewModel()
                {
                    CondominioID = _empresaId,
                    EdificacaoID = r.UnidadeViewModel.EdificacaoID,
                    EdificacaoDescricao = r.UnidadeViewModel.EdificacaoID > 0 ? db.Edificacaos.Find(r.UnidadeViewModel.EdificacaoID).Descricao : "",
                    UnidadeID = r.UnidadeViewModel.UnidadeID
                };

                if (SessaoLocal.CondominoID > 0)
                {
                    #region Valida permissão do usuário para editar o condômino
                    if (r.CondominoPFViewModel.CondominoID != SessaoLocal.CondominoID)
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
                result.CondominoPFViewModel.mensagem = new Validate() { Code = 999, Message = MensagemPadrao.Message(999).ToString(), MessageBase = ex.Message };
            }
            catch (App_DominioException ex)
            {
                result.CondominoPFViewModel.mensagem.Code = 999;

                if (ex.InnerException != null)
                    result.CondominoPFViewModel.mensagem.MessageBase = new App_DominioException(ex.InnerException.Message ?? ex.Message, GetType().FullName).Message;
                else
                    result.CondominoPFViewModel.mensagem.MessageBase = new App_DominioException(ex.Message, GetType().FullName).Message;
            }
            catch (DbUpdateException ex)
            {
                result.CondominoPFViewModel.mensagem.MessageBase = ex.InnerException.InnerException.Message ?? ex.Message;
                if (result.CondominoPFViewModel.mensagem.MessageBase.ToUpper().Contains("REFERENCE"))
                {
                    if (result.CondominoPFViewModel.mensagem.MessageBase.ToUpper().Contains("DELETE"))
                    {
                        result.CondominoPFViewModel.mensagem.Code = 16;
                        result.CondominoPFViewModel.mensagem.Message = MensagemPadrao.Message(16).ToString();
                        result.CondominoPFViewModel.mensagem.MessageType = MsgType.ERROR;
                    }
                    else
                    {
                        result.CondominoPFViewModel.mensagem.Code = 28;
                        result.CondominoPFViewModel.mensagem.Message = MensagemPadrao.Message(28).ToString();
                        result.CondominoPFViewModel.mensagem.MessageType = MsgType.ERROR;
                    }
                }
                else if (result.CondominoPFViewModel.mensagem.MessageBase.ToUpper().Contains("PRIMARY"))
                {
                    result.CondominoPFViewModel.mensagem.Code = 37;
                    result.CondominoPFViewModel.mensagem.Message = MensagemPadrao.Message(37).ToString();
                    result.CondominoPFViewModel.mensagem.MessageType = MsgType.WARNING;
                }
                else if (result.CondominoPFViewModel.mensagem.MessageBase.ToUpper().Contains("UNIQUE KEY"))
                {
                    result.CondominoPFViewModel.mensagem.Code = 54;
                    result.CondominoPFViewModel.mensagem.Message = MensagemPadrao.Message(54).ToString();
                    result.CondominoPFViewModel.mensagem.MessageType = MsgType.WARNING;
                }
                else
                {
                    result.CondominoPFViewModel.mensagem.Code = 44;
                    result.CondominoPFViewModel.mensagem.Message = MensagemPadrao.Message(44).ToString();
                    result.CondominoPFViewModel.mensagem.MessageType = MsgType.ERROR;
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                result.CondominoPFViewModel.mensagem = new Validate() { Code = 42, Message = MensagemPadrao.Message(42).ToString(), MessageBase = ex.EntityValidationErrors.Select(m => m.ValidationErrors.First().ErrorMessage).First() };
            }
            catch (Exception ex)
            {
                result.CondominoPFViewModel.mensagem.Code = 17;
                result.CondominoPFViewModel.mensagem.Message = MensagemPadrao.Message(17).ToString();
                result.CondominoPFViewModel.mensagem.MessageBase = new App_DominioException(ex.InnerException.InnerException.Message ?? ex.Message, GetType().FullName).Message;
                result.CondominoPFViewModel.mensagem.MessageType = MsgType.ERROR;
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