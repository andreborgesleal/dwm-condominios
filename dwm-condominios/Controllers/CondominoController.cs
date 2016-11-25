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

namespace DWM.Controllers
{
    public class CondominoController : DwmRootController<CondominoUnidadeViewModel, CondominoUnidadeModel, ApplicationContext>
    {
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
                int _EdificacaoID = EdificacaoID == null || EdificacaoID == "" ? 0 : int.Parse(EdificacaoID);
                int _UnidadeID = UnidadeID == null || UnidadeID == "" ? 0 : int.Parse(UnidadeID);

                EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
                ViewBag.empresaId = security.getSessaoCorrente().empresaId;

                ListViewCondominoUnidade l = new ListViewCondominoUnidade();
                return this._List(index, pageSize, "Browse", l, _EdificacaoID, _UnidadeID, Nome);
            }
            else
                return View();
        }

        #endregion

        #endregion

        #region Index
        /*[AuthorizeFilter]*/
        public ActionResult Index(int id, int EdificacaoID, int UnidadeID)
        {
            ViewBag.ValidateRequest = true;
            if (ViewBag.ValidateRequest)
            {
                //BindBreadCrumb(getListName(), ClearBreadCrumbOnBrowse());

                BindBreadCrumb(getBreadCrumbText("Condômino", null));

                Factory<CondominoEditViewModel, ApplicationContext> factory = new Factory<CondominoEditViewModel, ApplicationContext>();
                CondominoEditViewModel value = new CondominoEditViewModel()
                {
                    UnidadeViewModel = new UnidadeViewModel()
                    {
                        EdificacaoID = EdificacaoID,
                        UnidadeID = UnidadeID,
                    },
                    CondominoPFViewModel = new CondominoPFViewModel()
                    {
                        CondominoID = id,
                    },
                    CredenciadoViewModel = new CredenciadoViewModel()
                    {
                        CondominoID = id,
                    }
                };
                CondominoEditViewModel obj = factory.Execute(new EditarCondominoBI(), value);
                return View(obj);
            }
            return View();
        }

        [AuthorizeFilter]
        public ActionResult EditCondomino(int CondominioID, int CondominoID, string Nome, string Email, string IndFiscal, 
                                            string IndSituacao, string TelParticular1, string TelParticular2, string DataNascimento,
                                            string Sexo, string IndProprietario, string IndAnimal, string ProfissaoID, string Observacao)
        {
            if (ViewBag.ValidateRequest)
            {
                CondominoPFViewModel result = null;
                try
                {
                    CondominoPFViewModel value = new CondominoPFViewModel()
                    {
                        CondominioID = CondominioID,
                        CondominoID = CondominoID,
                        Nome = Nome,
                        Email = Email,
                        IndFiscal = IndFiscal,
                        IndSituacao = IndSituacao,
                        TelParticular1 = TelParticular1,
                        TelParticular2 = TelParticular2,
                        Sexo = Sexo,
                        IndProprietario = IndProprietario,
                        IndAnimal = IndAnimal,
                        ProfissaoID = ProfissaoID != null && int.Parse(ProfissaoID) > 0 ? int.Parse(ProfissaoID) : 0,
                        Observacao = Observacao
                    };

                    value.uri = this.ControllerContext.Controller.GetType().Name.Replace("Controller", "") + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
                    Facade<CondominoPFViewModel, CondominoPFModel, ApplicationContext> facade = new Facade<CondominoPFViewModel, CondominoPFModel, ApplicationContext>();
                    result = facade.Save(value, Crud.ALTERAR);

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

                if (ViewBag.ValidateRequest)
                {
                    CondominoEditViewModel result = null;
                    try
                    {
                        #region Incluir/Editar Credenciado
                        result = new CondominoEditViewModel()
                        {
                            UnidadeViewModel = new UnidadeViewModel(),
                            CondominoPFViewModel = new CondominoPFViewModel(),
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
                    CondominoPFViewModel = new CondominoPFViewModel()
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
                            CondominoPFViewModel = new CondominoPFViewModel(),
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
                    CondominoPFViewModel = new CondominoPFViewModel()
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
                            CondominoPFViewModel = new CondominoPFViewModel(),
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
                    CondominoPFViewModel = new CondominoPFViewModel()
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


        #endregion

        #region Enviar Token
        [AuthorizeFilter]
        public ActionResult EnviarToken()
        {
            if (ViewBag.ValidateRequest)
            {
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

    }
}