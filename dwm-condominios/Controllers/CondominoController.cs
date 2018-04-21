using App_Dominio.Controllers;
using App_Dominio.Security;
using DWM.Models.Enumeracoes;
using DWM.Models.Persistence;
using DWM.Models.Repositories;
using System.Web.Mvc;
using DWM.Models.Entidades;
using App_Dominio.Entidades;
using App_Dominio.Pattern;
using DWM.Models.BI;
using System;
using App_Dominio.Enumeracoes;
using DWM.Models.Pattern;
using System.Collections.Generic;
using App_Dominio.Models;
using System.Web;
using System.Linq;
using System.IO;
using System.Web.Helpers;
using App_Dominio.Repositories;

namespace DWM.Controllers
{
    public class CondominoController : DwmRootController<CondominoUnidadeViewModel, CondominoUnidadeModel, ApplicationContext>
    {
        private int _avatarWidth = 100; // ToDo - Change the size of the stored avatar image
        private int _avatarHeight = 100; // ToDo - Change the size of the stored avatar image

        #region Inheritance
        public override int _sistema_id() { return (int)DWM.Models.Enumeracoes.Sistema.DWMCONDOMINIOS; }

        public override string getListName()
        {
            return "Listar";
        }

        #region List
        public override ActionResult List(int? index, int? pageSize = 25, string descricao = null)
        {
            return ListParam(index, 25, Request["EdificacaoID"], Request["UnidadeID"], descricao);
        }

        [AuthorizeFilter]
        public ActionResult ListParam(int? index, int? pageSize = 25, string EdificacaoID = null, string UnidadeID = null, string Nome = null)
        {
            ViewBag.ValidateRequest = true;
            if (ViewBag.ValidateRequest)
            {
                ViewBag.TipoEdificacao = DWMSessaoLocal.GetTipoEdificacao(null).Descricao;
                int _EdificacaoID = EdificacaoID == null || EdificacaoID == "" ? 0 : int.Parse(EdificacaoID);
                int _UnidadeID = UnidadeID == null || UnidadeID == "" ? 0 : int.Parse(UnidadeID);

                EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
                Sessao sessaoCorrente = security.getSessaoCorrente();
                SessaoLocal SessaoLocal = DWMSessaoLocal.GetSessaoLocal();

                ViewBag.empresaId = sessaoCorrente.empresaId;
                ViewBag.CondominoID = SessaoLocal.CondominoID;

                ListViewCondominoUnidade l = new ListViewCondominoUnidade();
                if (SessaoLocal.CondominoID == 0)
                    return this._List(index, pageSize, "Browse", l, _EdificacaoID, _UnidadeID, Nome);
                else
                    return this._List(index, pageSize, "Browse", l, SessaoLocal.CondominoID);
            }
            else
                return View();
        }

        [AuthorizeFilter]
        public ActionResult ListDesativados(int? index, int? pageSize = 25, string EdificacaoID = null, string UnidadeID = null, string Nome = null)
        {
            ViewBag.ValidateRequest = true;
            if (ViewBag.ValidateRequest)
            {
                int _EdificacaoID = EdificacaoID == null || EdificacaoID == "" ? 0 : int.Parse(EdificacaoID);
                int _UnidadeID = UnidadeID == null || UnidadeID == "" ? 0 : int.Parse(UnidadeID);

                EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
                Sessao sessaoCorrente = security.getSessaoCorrente();
                SessaoLocal SessaoLocal = DWMSessaoLocal.GetSessaoLocal();

                ViewBag.empresaId = sessaoCorrente.empresaId;
                ViewBag.CondominoID = SessaoLocal.CondominoID;

                ListViewCondominosDesativados l = new ListViewCondominosDesativados();
                if (SessaoLocal.CondominoID == 0)
                    return this._List(index, pageSize, "Browse", l, _EdificacaoID, _UnidadeID, Nome);
                else
                    return this._List(index, pageSize, "Browse", l, SessaoLocal.CondominoID);
            }
            else
                return View();
        }

        [AuthorizeFilter]
        public ActionResult ListInativos()
        {
            if (ViewBag.ValidateRequest)
            {
                ViewBag.TipoEdificacao = DWMSessaoLocal.GetTipoEdificacao(null).Descricao;
                BindBreadCrumb("Condômino", ClearBreadCrumbOnBrowse());
                ListViewCondominosInativos l = new ListViewCondominosInativos();
                return this._List(0, 1000, "Browse", l);
            }
            else
                return View();
        }

        /// <summary>
        /// Este método é acionado pelo funcionalidade CHAMADO
        /// </summary>
        /// <param name="index"></param>
        /// <param name="pageSize"></param>
        /// <param name="EdificacaoID"></param>
        /// <param name="UnidadeID"></param>
        /// <param name="Nome"></param>
        /// <returns></returns>
        public ActionResult ListCondomino(int? index, int? pageSize = 25, string EdificacaoID = null, string UnidadeID = null, string Nome = null)
        {
            ViewBag.ValidateRequest = true;
            if (ViewBag.ValidateRequest)
            {
                ViewBag.TipoEdificacao = DWMSessaoLocal.GetTipoEdificacao(null).Descricao;
                int _EdificacaoID = EdificacaoID == null || EdificacaoID == "" ? 0 : int.Parse(EdificacaoID);
                int _UnidadeID = UnidadeID == null || UnidadeID == "" ? 0 : int.Parse(UnidadeID);
                ViewBag.TipoEdificacao = DWMSessaoLocal.GetTipoEdificacao(null).Descricao;

                EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
                Sessao sessaoCorrente = security.getSessaoCorrente();
                SessaoLocal SessaoLocal = DWMSessaoLocal.GetSessaoLocal();

                ViewBag.empresaId = security.getSessaoCorrente().empresaId;

                ListViewCondominoUnidadeChamado l = new ListViewCondominoUnidadeChamado();

                if (SessaoLocal.CondominoID == 0)
                    return this._List(index, pageSize, "Browse", l, _EdificacaoID, _UnidadeID, Nome);
                else
                    return this._List(index, pageSize, "Browse", l, SessaoLocal.CondominoID);
            }
            else
                return View();
        }
        #endregion

        #endregion

        #region Index
        [AuthorizeFilter]
        public ActionResult Index(int id, int EdificacaoID, int UnidadeID, string TipoPessoa)
        {
            if (ViewBag.ValidateRequest)
            {
                //BindBreadCrumb(getListName(), ClearBreadCrumbOnBrowse());

                BindBreadCrumb(getBreadCrumbText("Condômino", null));

                SessaoLocal s = DWMSessaoLocal.GetSessaoLocal();
                ViewBag.SessaoLocal = s;

                Factory<CondominoEditViewModel, ApplicationContext> factory = new Factory<CondominoEditViewModel, ApplicationContext>();
                CondominoEditViewModel value = new CondominoEditViewModel()
                {
                    UnidadeViewModel = new UnidadeViewModel()
                    {
                        EdificacaoID = EdificacaoID,
                        UnidadeID = UnidadeID,
                    },
                    CredenciadoViewModel = new CredenciadoViewModel()
                    {
                        CondominoID = id,
                    }
                };
                if (TipoPessoa == "PF")
                    value.CondominoViewModel = new CondominoPFViewModel()
                    {
                        CondominoID = id,
                    };
                else
                    value.CondominoViewModel = new CondominoPJViewModel()
                    {
                        CondominoID = id,
                    };

                CondominoEditViewModel obj = factory.Execute(new EditarCondominoBI(), value);
                if (obj.CondominoViewModel.mensagem.Code > 0)
                    obj = null;
                    
                return View(obj);
            }
            return View();
        }

        [AuthorizeFilter]
        public ActionResult EditCondomino(int CondominioID, int CondominoID, string Nome, string Email, string IndFiscal, string DataCadastro,
                                            string IndSituacao, string TelParticular1, string TelParticular2, string DataNascimento, string usuarioId,
                                            string Sexo, string IndProprietario, string IndAnimal, string ProfissaoID, string Observacao, 
                                            string cnpj, string Administrador, string RamoAtividadeID)
        {
            if (ViewBag.ValidateRequest)
            {
                CondominoViewModel result = null;
                try
                {
                    SessaoLocal s = DWMSessaoLocal.GetSessaoLocal();
                    ViewBag.SessaoLocal = s;
                    CondominoViewModel value = null;

                    if (IndFiscal != null && IndFiscal.Trim().Length > 0)
                    {
                        value = new CondominoPFViewModel()
                        {
                            CondominioID = CondominioID,
                            CondominoID = CondominoID,
                            Nome = Nome,
                            Email = Email,
                            IndFiscal = IndFiscal,
                            IndSituacao = IndSituacao,
                            DataCadastro = DataCadastro != null && DataCadastro != "" ? Funcoes.StringToDate(DataCadastro).Value : Funcoes.Brasilia(),
                            TelParticular1 = TelParticular1,
                            TelParticular2 = TelParticular2,
                            Sexo = Sexo,
                            IndProprietario = IndProprietario,
                            IndAnimal = IndAnimal,
                            DataNascimento = Funcoes.StringToDate(DataNascimento),
                            ProfissaoID = ProfissaoID != null && int.Parse(ProfissaoID) > 0 ? int.Parse(ProfissaoID) : 0,
                            Observacao = Observacao,
                            UsuarioViewModel = new UsuarioViewModel()
                            {
                                empresaId = CondominioID,
                                usuarioId = usuarioId != null && usuarioId != "" ? int.Parse(usuarioId) : 0
                            }
                        };

                        value.uri = this.ControllerContext.Controller.GetType().Name.Replace("Controller", "") + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
                        Facade<CondominoPFViewModel, CondominoPFModel, ApplicationContext> facade = new Facade<CondominoPFViewModel, CondominoPFModel, ApplicationContext>();
                        result = facade.Save((CondominoPFViewModel)value, Crud.ALTERAR);
                    }
                    else
                    {
                        value = new CondominoPJViewModel()
                        {
                            CondominioID = CondominioID,
                            CondominoID = CondominoID,
                            Nome = Nome,
                            Email = Email,
                            IndFiscal = cnpj,
                            IndSituacao = IndSituacao,
                            DataCadastro = DataCadastro != null && DataCadastro != "" ? Funcoes.StringToDate(DataCadastro).Value : Funcoes.Brasilia(),
                            TelParticular1 = TelParticular1,
                            TelParticular2 = TelParticular2,
                            IndProprietario = IndProprietario,
                            Administrador = Administrador,
                            RamoAtividadeID = RamoAtividadeID != null && RamoAtividadeID != "" ? int.Parse(RamoAtividadeID) : 0,
                            Observacao = Observacao,
                            UsuarioViewModel = new UsuarioViewModel()
                            {
                                empresaId = CondominioID,
                                usuarioId = usuarioId != null && usuarioId != "" ? int.Parse(usuarioId) : 0
                            }
                        };

                        value.uri = this.ControllerContext.Controller.GetType().Name.Replace("Controller", "") + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
                        Facade<CondominoPJViewModel, CondominoPJModel, ApplicationContext> facade = new Facade<CondominoPJViewModel, CondominoPJModel, ApplicationContext>();
                        result = facade.Save((CondominoPJViewModel)value, Crud.ALTERAR);
                    }

                    if (result.mensagem.Code > 0)
                        throw new App_DominioException(result.mensagem);

                    Success("Condômino alterado com sucesso");
                }
                catch (App_DominioException ex)
                {
                    ModelState.AddModelError("", ex.Result.MessageBase); // mensagem amigável ao usuário
                    Error(ex.Result.Message); // Mensagem em inglês com a descrição detalhada do erro e fica no topo da tela
                }
                catch (Exception ex)
                {
                    App_DominioException.saveError(ex, GetType().FullName);
                    ModelState.AddModelError("", MensagemPadrao.Message(17).ToString()); // mensagem amigável ao usuário
                    Error(ex.Message); // Mensagem em inglês com a descrição detalhada do erro e fica no topo da tela
                }

                return View(result);
            }
            else
                return View();
        }

        [AuthorizeFilter]
        public ActionResult EditCredenciado(CredenciadoViewModel CredenciadoViewModel, string Operacao)
        {
            if (ModelState.IsValid)
            {
                int CredenciadoViewModel_CondominoID = CredenciadoViewModel.CondominoID;
                int CredenciadoViewModel_CredenciadoID = CredenciadoViewModel.CredenciadoID;
                int? CredenciadoViewModel_UsuarioID = CredenciadoViewModel.UsuarioID;
                string CredenciadoViewModel_Nome = CredenciadoViewModel.Nome;
                string CredenciadoViewModel_Email = CredenciadoViewModel.Email;
                int CredenciadoViewModel_TipoCredenciadoID = CredenciadoViewModel.TipoCredenciadoID;
                string CredenciadoViewModel_Observacao = CredenciadoViewModel.Observacao;
                string CredenciadoViewModel_Sexo = CredenciadoViewModel.Sexo;
                string CredenciadoViewModel_IndVisitantePermanente = CredenciadoViewModel.IndVisitantePermanente;

                if (ViewBag.ValidateRequest)
                {
                    CondominoEditViewModel result = null;
                    try
                    {
                        #region Incluir/Editar Credenciado
                        result = new CondominoEditViewModel()
                        {
                            UnidadeViewModel = new UnidadeViewModel(),
                            CondominoViewModel = new CondominoPFViewModel(),
                            CredenciadoViewModel = new CredenciadoViewModel()
                            {
                                CredenciadoID = CredenciadoViewModel_CredenciadoID,
                                CondominoID = CredenciadoViewModel_CondominoID,
                                Nome = CredenciadoViewModel_Nome,
                                Email = CredenciadoViewModel_Email,
                                TipoCredenciadoID = CredenciadoViewModel_TipoCredenciadoID,
                                Sexo = CredenciadoViewModel_Sexo,
                                Observacao = CredenciadoViewModel_Observacao,
                                UsuarioID = CredenciadoViewModel_UsuarioID,
                                IndVisitantePermanente = CredenciadoViewModel_IndVisitantePermanente,
                                mensagem = new App_Dominio.Contratos.Validate() { Code = 0 }
                            },
                            mensagem = new App_Dominio.Contratos.Validate() {  Code = 0}
                        };

                        result.CredenciadoViewModel.uri = this.ControllerContext.Controller.GetType().Name.Replace("Controller", "") + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
                        FactoryLocalhost<CredenciadoViewModel, ApplicationContext> factory = new FactoryLocalhost<CredenciadoViewModel, ApplicationContext>();
                        result.Credenciados = factory.Execute(new EditarCredenciadoBI(Operacao), result.CredenciadoViewModel, CredenciadoViewModel_CondominoID, Operacao);
                        if (factory.Mensagem.Code > 0)
                            throw new App_DominioException(factory.Mensagem);

                        //if (result.CredenciadoViewModel.CredenciadoID == 0 && !string.IsNullOrEmpty(result.CredenciadoViewModel.Email) )
                        //{
                        //    #region envio de e-mail ao credenciado para ativação
                        //    result.CredenciadoViewModel.mensagem.Field = factory.Mensagem.Field; // senha do credenciado
                        //    CredenciadoViewModel repository = factory.Execute(new EnviarEmailCredenciadoBI(), result.CredenciadoViewModel);
                        //    if (repository.mensagem.Code > 0)
                        //        throw new ArgumentException(repository.mensagem.MessageBase);
                        //    #endregion
                        //}

                        CredenciadoModel CredenciadoModel = new CredenciadoModel();
                        result.CredenciadoViewModel = CredenciadoModel.CreateRepository(Request);
                        result.CredenciadoViewModel.CondominoID = CredenciadoViewModel_CondominoID;
                        #endregion

                        Success("Registro processado com sucesso");
                    }
                    catch (App_DominioException ex)
                    {
                        ModelState.AddModelError("", ex.Result.MessageBase); // mensagem amigável ao usuário
                        Error(ex.Result.Message); // Mensagem em inglês com a descrição detalhada do erro e fica no topo da tela
                    }
                    catch (Exception ex)
                    {
                        App_DominioException.saveError(ex, GetType().FullName);
                        ModelState.AddModelError("", MensagemPadrao.Message(17).ToString()); // mensagem amigável ao usuário
                        Error(ex.Message); // Mensagem em inglês com a descrição detalhada do erro e fica no topo da tela
                    }

                    return View("_Credenciado", result);
                }
                else
                {
                    return View();
                }
            }
            else
            {
                CondominoEditViewModel result = new CondominoEditViewModel()
                {
                    UnidadeViewModel = new UnidadeViewModel(),
                    CondominoViewModel = new CondominoPFViewModel()
                    {
                        CondominoID = CredenciadoViewModel.CondominoID
                    },
                    CredenciadoViewModel = CredenciadoViewModel
                };

                Factory<CondominoEditViewModel, ApplicationContext> factory = new Factory<CondominoEditViewModel, ApplicationContext>();
                CondominoEditViewModel obj = factory.Execute(new EditarCondominoBI(), result);
                result.Credenciados = obj.Credenciados;

                Error("Erro de preenhcimento em campos");

                return View("_Credenciado", result);
            }
        }

        [AuthorizeFilter]
        public ActionResult EditVeiculo(VeiculoViewModel VeiculoViewModel, string OperacaoVei)
        {
            if (ModelState.IsValid)
            {
                if (ViewBag.ValidateRequest)
                {
                    CondominoEditViewModel result = null;
                    try
                    {
                        #region Incluir/Editar Veiculo
                        result = new CondominoEditViewModel()
                        {
                            UnidadeViewModel = new UnidadeViewModel(),
                            CondominoViewModel = new CondominoPFViewModel(),
                            CredenciadoViewModel = new CredenciadoViewModel(),
                            VeiculoViewModel = new VeiculoViewModel()
                            {
                                CondominioID = VeiculoViewModel.CondominioID,
                                EdificacaoID = VeiculoViewModel.EdificacaoID,
                                UnidadeID = VeiculoViewModel.UnidadeID,
                                CondominoID = VeiculoViewModel.CondominoID,
                                Placa = VeiculoViewModel.Placa,
                                Marca = VeiculoViewModel.Marca,
                                Cor = VeiculoViewModel.Cor,
                                Descricao = VeiculoViewModel.Descricao,
                                Condutor = VeiculoViewModel.Condutor
                            },
                            mensagem = new App_Dominio.Contratos.Validate() { Code = 0 }
                        };

                        result.VeiculoViewModel.uri = this.ControllerContext.Controller.GetType().Name.Replace("Controller", "") + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
                        FactoryLocalhost<VeiculoViewModel, ApplicationContext> factory = new FactoryLocalhost<VeiculoViewModel, ApplicationContext>();
                        result.Veiculos = factory.Execute(new EditarVeiculoBI(OperacaoVei), result.VeiculoViewModel, VeiculoViewModel.CondominioID, VeiculoViewModel.EdificacaoID, VeiculoViewModel.UnidadeID,  VeiculoViewModel.CondominoID, OperacaoVei);
                        if (factory.Mensagem.Code > 0)
                            throw new App_DominioException(factory.Mensagem);

                        VeiculoModel VeiculoModel = new VeiculoModel();
                        result.VeiculoViewModel = VeiculoModel.CreateRepository(Request);
                        result.VeiculoViewModel.CondominioID = VeiculoViewModel.CondominioID;
                        result.VeiculoViewModel.EdificacaoID = VeiculoViewModel.EdificacaoID;
                        result.VeiculoViewModel.UnidadeID = VeiculoViewModel.UnidadeID;
                        result.VeiculoViewModel.CondominoID = VeiculoViewModel.CondominoID;
                        #endregion

                        Success("Registro processado com sucesso");
                    }
                    catch (App_DominioException ex)
                    {
                        ModelState.AddModelError("", ex.Result.MessageBase); // mensagem amigável ao usuário
                        Error(ex.Result.Message); // Mensagem em inglês com a descrição detalhada do erro e fica no topo da tela
                    }
                    catch (Exception ex)
                    {
                        App_DominioException.saveError(ex, GetType().FullName);
                        ModelState.AddModelError("", MensagemPadrao.Message(17).ToString()); // mensagem amigável ao usuário
                        Error(ex.Message); // Mensagem em inglês com a descrição detalhada do erro e fica no topo da tela
                    }

                    return View("_Veiculos", result);
                }
                else
                {
                    return View();
                }
            }
            else
            {
                CondominoEditViewModel result = new CondominoEditViewModel()
                {
                    UnidadeViewModel = new UnidadeViewModel()
                    {
                        EdificacaoID = VeiculoViewModel.EdificacaoID,
                        UnidadeID = VeiculoViewModel.UnidadeID,
                        CondominioID = VeiculoViewModel.CondominioID
                    },
                    CondominoViewModel = new CondominoPFViewModel()
                    {
                        CondominoID = VeiculoViewModel.CondominoID
                    },
                    VeiculoViewModel = VeiculoViewModel
                };

                Factory<CondominoEditViewModel, ApplicationContext> factory = new Factory<CondominoEditViewModel, ApplicationContext>();
                CondominoEditViewModel obj = factory.Execute(new EditarCondominoBI(), result);
                result.Veiculos = obj.Veiculos;

                Error("Erro de preenhcimento em campos");

                return View("_Veiculos", result);
            }
        }

        [AuthorizeFilter]
        public ActionResult EditFuncionario(FuncionarioViewModel FuncionarioViewModel, string OperacaoFunc)
        {
            if (ModelState.IsValid)
            {
                if (ViewBag.ValidateRequest)
                {
                    CondominoEditViewModel result = null;
                    try
                    {
                        #region Incluir/Editar Funcionário
                        result = new CondominoEditViewModel()
                        {
                            UnidadeViewModel = new UnidadeViewModel(),
                            CondominoViewModel = new CondominoPFViewModel(),
                            CredenciadoViewModel = new CredenciadoViewModel(),
                            VeiculoViewModel = new VeiculoViewModel(),
                            FuncionarioViewModel = new FuncionarioViewModel()
                            {
                                FuncionarioID = FuncionarioViewModel.FuncionarioID,
                                CondominioID = FuncionarioViewModel.CondominioID,
                                EdificacaoID = FuncionarioViewModel.EdificacaoID,
                                UnidadeID = FuncionarioViewModel.UnidadeID,
                                CondominoID = FuncionarioViewModel.CondominoID,
                                Nome = FuncionarioViewModel.Nome,
                                Funcao = FuncionarioViewModel.Funcao,
                                Sexo = FuncionarioViewModel.Sexo,
                                Dia = FuncionarioViewModel.Dia,
                                HoraInicial = FuncionarioViewModel.HoraInicial,
                                HoraFinal = FuncionarioViewModel.HoraFinal,
                                RG = FuncionarioViewModel.RG
                            },
                            mensagem = new App_Dominio.Contratos.Validate() { Code = 0 }
                        };

                        result.FuncionarioViewModel.uri = this.ControllerContext.Controller.GetType().Name.Replace("Controller", "") + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
                        FactoryLocalhost<FuncionarioViewModel, ApplicationContext> factory = new FactoryLocalhost<FuncionarioViewModel, ApplicationContext>();
                        result.Funcionarios = factory.Execute(new EditarFuncionarioBI(OperacaoFunc), result.FuncionarioViewModel, FuncionarioViewModel.CondominioID, FuncionarioViewModel.EdificacaoID, FuncionarioViewModel.UnidadeID, FuncionarioViewModel.CondominoID, OperacaoFunc);
                        if (factory.Mensagem.Code > 0)
                            throw new App_DominioException(factory.Mensagem);
                        FuncionarioModel FuncionarioModel = new FuncionarioModel();
                        result.FuncionarioViewModel = FuncionarioModel.CreateRepository(Request);
                        result.FuncionarioViewModel.FuncionarioID = FuncionarioViewModel.FuncionarioID;
                        result.FuncionarioViewModel.CondominioID = FuncionarioViewModel.CondominioID;
                        result.FuncionarioViewModel.EdificacaoID = FuncionarioViewModel.EdificacaoID;
                        result.FuncionarioViewModel.UnidadeID = FuncionarioViewModel.UnidadeID;
                        result.FuncionarioViewModel.CondominoID = FuncionarioViewModel.CondominoID;
                        #endregion

                        Success("Registro processado com sucesso");
                    }
                    catch (App_DominioException ex)
                    {
                        ModelState.AddModelError("", ex.Result.MessageBase); // mensagem amigável ao usuário
                        Error(ex.Result.Message); // Mensagem em inglês com a descrição detalhada do erro e fica no topo da tela
                    }
                    catch (Exception ex)
                    {
                        App_DominioException.saveError(ex, GetType().FullName);
                        ModelState.AddModelError("", MensagemPadrao.Message(17).ToString()); // mensagem amigável ao usuário
                        Error(ex.Message); // Mensagem em inglês com a descrição detalhada do erro e fica no topo da tela
                    }

                    return View("_Funcionarios", result);
                }
                else
                {
                    return View();
                }
            }
            else
            {
                CondominoEditViewModel result = new CondominoEditViewModel()
                {
                    UnidadeViewModel = new UnidadeViewModel()
                    {
                        EdificacaoID = FuncionarioViewModel.EdificacaoID,
                        UnidadeID = FuncionarioViewModel.UnidadeID,
                        CondominioID = FuncionarioViewModel.CondominioID
                    },
                    CondominoViewModel = new CondominoPFViewModel()
                    {
                        CondominoID = FuncionarioViewModel.CondominoID
                    },
                    VeiculoViewModel = new VeiculoViewModel(),
                    FuncionarioViewModel = FuncionarioViewModel
                };

                Factory<CondominoEditViewModel, ApplicationContext> factory = new Factory<CondominoEditViewModel, ApplicationContext>();
                CondominoEditViewModel obj = factory.Execute(new EditarCondominoBI(), result);
                result.Funcionarios = obj.Funcionarios;

                Error("Erro de preenhcimento em campos");

                return View("_Funcionarios", result);
            }
        }

        [AuthorizeFilter]
        public ActionResult EditGrupoCondomino(GrupoCondominoUsuarioViewModel GrupoCondominoUsuarioViewModel, string Operacao)
        {
            if (ViewBag.ValidateRequest)
            {
                IEnumerable<GrupoCondominoUsuarioViewModel> Grupos = null;
                try
                {
                    #region Incluir/Excluir Grupo do Condômino
                    GrupoCondominoUsuarioViewModel.uri = this.ControllerContext.Controller.GetType().Name.Replace("Controller", "") + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
                    Factory<GrupoCondominoUsuarioViewModel, ApplicationContext> factory = new Factory<GrupoCondominoUsuarioViewModel, ApplicationContext>();

                    Grupos = factory.Execute(new GrupoCondominoUsuarioBI(Operacao), GrupoCondominoUsuarioViewModel, GrupoCondominoUsuarioViewModel.CondominoID);
                    #endregion
                }
                catch (App_DominioException ex)
                {
                    ModelState.AddModelError("", ex.Result.MessageBase); // mensagem amigável ao usuário
                    Error(ex.Result.Message); // Mensagem em inglês com a descrição detalhada do erro e fica no topo da tela
                }
                catch (Exception ex)
                {
                    App_DominioException.saveError(ex, GetType().FullName);
                    ModelState.AddModelError("", MensagemPadrao.Message(17).ToString()); // mensagem amigável ao usuário
                    Error(ex.Message); // Mensagem em inglês com a descrição detalhada do erro e fica no topo da tela
                }

                return View("_GrupoCondomino", Grupos);
            }
            else
                return View();
        }
        #endregion

        #region Vincular Unidade ao Condômino
        [AuthorizeFilter]
        public ActionResult Append(int CondominoID)
        {
            if (ViewBag.ValidateRequest)
            {
                BindBreadCrumb(getBreadCrumbText("Condômino", null));
                Facade<CondominoUnidadeViewModel, CondominoUnidadeModel, ApplicationContext> facade = new Facade<CondominoUnidadeViewModel, CondominoUnidadeModel, ApplicationContext>();
                CondominoUnidadeViewModel obj = facade.CreateRepository(Request);

                return View(obj);
            }
            return View();
        }

        [AuthorizeFilter]
        [HttpPost]
        public ActionResult Append(CondominoUnidadeViewModel value, FormCollection collection)
        {
            if (ViewBag.ValidateRequest)
            {
                value.CondominoViewModel = new CondominoPFViewModel();
                value.DataInicio = Funcoes.Brasilia().Date;
                CondominoUnidadeViewModel ret = SetCreate(value, collection);

                if (ret.mensagem.Code == 0)
                    return RedirectToAction("Browse");
                else
                    return View(ret);
            }
            else
                return null;
        }

        public override CondominoUnidadeViewModel Insert(CondominoUnidadeViewModel value)
        {
            Facade<CondominoUnidadeViewModel, CondominoUnidadeModel, ApplicationContext> facade = new Facade<CondominoUnidadeViewModel, CondominoUnidadeModel, ApplicationContext>();
            return facade.Save(value, Crud.INCLUIR);
        }


        #endregion

        #region Ativar condômino e notificar por e-mail a ativação
        [AuthorizeFilter]
        public ActionResult Ativar(int CondominioID, int EdificacaoID, int UnidadeID, int CondominoID)
        {
            if (ViewBag.ValidateRequest)
            {
                BindBreadCrumb(getBreadCrumbText("Condômino", null));
                FactoryLocalhost<CondominoUnidadeViewModel, ApplicationContext> factory = new FactoryLocalhost<CondominoUnidadeViewModel, ApplicationContext>();

                CondominoUnidadeViewModel value = new CondominoUnidadeViewModel()
                {
                    CondominioID = CondominioID,
                    CondominoID = CondominoID,
                    EdificacaoID = EdificacaoID,
                    UnidadeID = UnidadeID,
                    uri = this.ControllerContext.Controller.GetType().Name.Replace("Controller", "") + "/" + this.ControllerContext.RouteData.Values["action"].ToString()
                };

                CondominoUnidadeViewModel cu = factory.Execute(new CondominoAtivarBI(), value);

                return RedirectToAction("ListInativos");
            }
            return View();
        }

        #endregion

        #region Desativar Condômino da Unidade 
        [AuthorizeFilter]
        public ActionResult Desativar(int CondominioID, int EdificacaoID, int UnidadeID, int CondominoID)
        {
            if (ViewBag.ValidateRequest)
            {
                BindBreadCrumb(getBreadCrumbText("Condômino", null));
                Facade<CondominoUnidadeViewModel, CondominoUnidadeModel, ApplicationContext> facade = new Facade<CondominoUnidadeViewModel, CondominoUnidadeModel, ApplicationContext>();
                CondominoUnidadeViewModel value = new CondominoUnidadeViewModel()
                {
                    CondominioID = CondominioID,
                    EdificacaoID = EdificacaoID,
                    UnidadeID = UnidadeID,
                    CondominoID = CondominoID
                };
                CondominoUnidadeViewModel obj = facade.getObject(value);
                return View(obj);
            }
            return View();
        }

        [AuthorizeFilter]
        [HttpPost]
        public ActionResult Desativar(CondominoUnidadeViewModel value, FormCollection collection)
        {
            if (ViewBag.ValidateRequest)
            {
                value.CondominoViewModel = new CondominoPFViewModel();
                value.DataFim = Funcoes.Brasilia().Date;
                CondominoUnidadeViewModel ret = SetEdit(value, collection, "Condômino");

                if (ret.mensagem.Code == 0)
                    return RedirectToAction("Browse");
                else
                    return View(ret);
            }
            else
                return null;
        }

        public override CondominoUnidadeViewModel Update(CondominoUnidadeViewModel value)
        {
            Facade<CondominoUnidadeViewModel, CondominoUnidadeModel, ApplicationContext> facade = new Facade<CondominoUnidadeViewModel, CondominoUnidadeModel, ApplicationContext>();
            return facade.Save(value, Crud.ALTERAR);
        }


        #endregion

        #region Enviar Token
        [AuthorizeFilter]
        public ActionResult EnviarToken()
        {
            if (ViewBag.ValidateRequest)
            {
                BindBreadCrumb("Condômino", ClearBreadCrumbOnBrowse());
                //BindBreadCrumb(getBreadCrumbText("Condômino", null));
                UnidadeModel UnidadeModel = new UnidadeModel();
                UnidadeViewModel UnidadeViewModel = UnidadeModel.CreateRepository(Request);
                return View(UnidadeViewModel);
            }
            return View();
        }

        [HttpPost]
        [AuthorizeFilter]
        public ActionResult EnviarToken(UnidadeViewModel UnidadeViewModel)
        {
            if (ModelState.IsValid)
            {
                if (ViewBag.ValidateRequest)
                {
                    try
                    {
                        #region Procesar envio do token
                        UnidadeViewModel.uri = this.ControllerContext.Controller.GetType().Name.Replace("Controller", "") + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
                        UnidadeViewModel.mensagem = new App_Dominio.Contratos.Validate() { Code = 0 };

                        FactoryLocalhost<UnidadeViewModel, ApplicationContext> factory = new FactoryLocalhost<UnidadeViewModel, ApplicationContext>();
                        UnidadeViewModel = factory.Execute(new GerarTokenBI(), UnidadeViewModel);
                        if (factory.Mensagem.Code > 0)
                            throw new App_DominioException(factory.Mensagem);

                        #endregion

                        Success("Token enviado com sucesso para o e-mail do condômino.");
                    }
                    catch (App_DominioException ex)
                    {
                        ModelState.AddModelError("", ex.Result.MessageBase); // mensagem amigável ao usuário
                        Error(ex.Result.Message); // Mensagem em inglês com a descrição detalhada do erro e fica no topo da tela
                    }
                    catch (Exception ex)
                    {
                        App_DominioException.saveError(ex, GetType().FullName);
                        ModelState.AddModelError("", MensagemPadrao.Message(17).ToString()); // mensagem amigável ao usuário
                        Error(ex.Message); // Mensagem em inglês com a descrição detalhada do erro e fica no topo da tela
                    }

                    UnidadeModel model = new UnidadeModel();
                    UnidadeViewModel = model.CreateRepository(Request);
                }
            }
            return View(UnidadeViewModel);
        }
        #endregion

        #region Retorno as Unidades de uma dada Edificação
        [AllowAnonymous]
        public JsonResult GetNames(string term, int tag)
        {
            var results = new BindDropDownList().Unidades("Selecione...", "", term, tag);

            return new JsonResult()
            {
                Data = results,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }
        #endregion

        #region Avatar
        [HttpGet]
        [AuthorizeFilter]
        public ActionResult Avatar()
        {
            if (ViewBag.ValidateRequest)
            {
                EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
                UsuarioRepository u = security.getUsuarioRepositoryById();

                UsuarioViewModel r = new UsuarioViewModel()
                {
                    usuarioId = u.usuarioId,
                    empresaId = u.empresaId,
                    dt_cadastro = u.dt_cadastro,
                    Grupos = u.Grupos,
                    isAdmin = u.isAdmin,
                    login = u.login,
                    nome = u.nome,
                    nome_sistema = u.nome_sistema,
                    situacao = u.situacao
                };

                return View(r);
            }
            return View();
        }

        [ValidateAntiForgeryToken]
        [AuthorizeFilter]
        public ActionResult Avatar(IEnumerable<HttpPostedFileBase> files)
        {
            if (ViewBag.ValidateRequest)
            {
                string errorMessage = "";

                if (files != null && files.Count() > 0)
                {
                    // Get one only
                    var file = files.FirstOrDefault();
                    // Check if the file is an image
                    if (file != null && IsImage(file))
                    {
                        // Verify that the user selected a file
                        if (file != null && file.ContentLength > 0)
                        {
                            var webPath = SaveTemporaryFile(file);
                            return Json(new { success = true, fileName = webPath.Replace("/", "\\") }); // success
                        }
                        errorMessage = "Arquivo não pode ser de tamnho nulo."; //failure
                    }
                    errorMessage = "Formato de arquivo inválido."; //failure
                }
                errorMessage = "Nenhum arquivo foi enviado."; //failure

                return Json(new { success = false, errorMessage = errorMessage });
            }
            return View();
        }

        [HttpPost]
        public ActionResult Save(string t, string l, string h, string w, string fileName, string key)
        {
            try
            {
                // Get file from temporary folder
                var fn = Path.Combine(Server.MapPath("~/Temp"), Path.GetFileName(fileName));

                // Calculate dimesnions
                int top = Convert.ToInt32(t.Replace("-", "").Replace("px", ""));
                int left = Convert.ToInt32(l.Replace("-", "").Replace("px", ""));
                int height = Convert.ToInt32(h.Replace("-", "").Replace("px", ""));
                int width = Convert.ToInt32(w.Replace("-", "").Replace("px", ""));

                // Get image and resize it, ...
                var img = new WebImage(fn);
                img.Resize(width, height);
                // ... crop the part the user selected, ...
                img.Crop(top, left, img.Height - top - _avatarHeight, img.Width - left - _avatarWidth);
                // ... delete the temporary file,...
                System.IO.File.Delete(fn);
                // ... and save the new one.

                string oldName = new UsuarioViewModel().Avatar();
                if (oldName.Substring(0,4) != "http")
                    System.IO.File.Delete(Server.MapPath(oldName));

                string newName = String.Format("{0}" + new FileInfo(fn).Extension, key);
                //AssociadoViewModel value = (AssociadoViewModel)getModel().getObject(new AssociadoViewModel() { associadoId = int.Parse(key) });
                //if (value.avatar != null)
                //    newName = value.avatar;

                //string newFileName = System.Configuration.ConfigurationManager.AppSettings["Avatar"] + "/" + newName; // Path.GetFileName(fn);
                string newFileLocation = HttpContext.Server.MapPath(new UsuarioViewModel().Path());
                if (Directory.Exists(Path.GetDirectoryName(newFileLocation)) == false)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(newFileLocation));
                }

                img.FileName = Path.Combine(newFileLocation, Path.GetFileName(newName));  //newName;
                img.Save(img.FileName);
                return Json(new { success = true, avatarFileLocation = newFileLocation });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = "Não foi possível fazer o upload do arquivo.\nERRORINFO: " + ex.Message });
            }
        }

        private bool IsImage(HttpPostedFileBase file)
        {
            if (file.ContentType.Contains("image"))
            {
                return true;
            }

            var extensions = new string[] { ".jpg", ".png", ".gif", ".jpeg" }; // add more if you like...

            // linq from Henrik Stenbæk
            return extensions.Any(item => file.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));
        }

        private string SaveTemporaryFile(HttpPostedFileBase file)
        {
            // Define destination
            var folderName = "/Temp";
            var serverPath = HttpContext.Server.MapPath(folderName);
            if (Directory.Exists(serverPath) == false)
            {
                Directory.CreateDirectory(serverPath);
            }

            // Generate unique file name
            var fileName = Path.GetFileName(file.FileName);
            fileName = SaveTemporaryAvatarFileImage(file, serverPath, fileName);

            // Clean up old files after every save
            CleanUpTempFolder(1);

            return Path.Combine(folderName, fileName);
        }

        private string SaveTemporaryAvatarFileImage(HttpPostedFileBase file, string serverPath, string fileName)
        {
            var img = new WebImage(file.InputStream);
            double ratio = (double)img.Height / (double)img.Width;

            string fullFileName = Path.Combine(serverPath, fileName);

            img.Resize(400, (int)(400 * ratio)); // ToDo - Change the value of the width of the image oin the screen

            if (System.IO.File.Exists(fullFileName))
                System.IO.File.Delete(fullFileName);

            img.Save(fullFileName);

            return Path.GetFileName(img.FileName);
        }

        private void CleanUpTempFolder(int hoursOld)
        {
            try
            {
                DateTime fileCreationTime;
                DateTime currentUtcNow = DateTime.UtcNow;

                var serverPath = HttpContext.Server.MapPath("/Temp");
                if (Directory.Exists(serverPath))
                {
                    string[] fileEntries = Directory.GetFiles(serverPath);
                    foreach (var fileEntry in fileEntries)
                    {
                        fileCreationTime = System.IO.File.GetCreationTimeUtc(fileEntry);
                        var res = currentUtcNow - fileCreationTime;
                        if (res.TotalHours > hoursOld)
                        {
                            System.IO.File.Delete(fileEntry);
                        }
                    }
                }
            }
            catch
            {
            }
        }

        #endregion
    }
}