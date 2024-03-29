﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using App_Dominio.Security;
using App_Dominio.Contratos;
using App_Dominio.Controllers;
using System.Data.Entity.Validation;
using App_Dominio.Entidades;
using DWM.Models;
using App_Dominio.Enumeracoes;
using DWM.Models.Entidades;
using DWM.Models.BI;
using DWM.Models.Pattern;
using App_Dominio.Pattern;
using System.Collections.Generic;
using App_Dominio.Repositories;
using DWM.Models.Repositories;

namespace DWM.Controllers
{
    [Authorize]
    public class AccountController : SuperController
    {
        #region Inheritance
        public override int _sistema_id() { return (int)DWM.Models.Enumeracoes.Sistema.DWMCONDOMINIOS; }

        public override string getListName()
        {
            return "Login";
        }

        public override ActionResult List(int? index, int? pageSize = 40, string descricao = null)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Login
        [AllowAnonymous]
        public ActionResult Login(string id)
        {
            try
            {
                if (Request.Url.Host.ToLower().Contains("parcparadiso"))
                    id = "ParcParadiso";

                if (String.IsNullOrEmpty(id) )
                    throw new ArgumentException();

                Condominio Condominio = DWMSessaoLocal.GetCondominioByPathInfo(id);
                if (Condominio == null)
                    throw new ArgumentException();

                LoginViewModel value = new LoginViewModel()
                {
                    Condominio = Condominio
                };
                return View(value); // RedirectToAction("Default", "Home");
            }
            catch(Exception ex)
            {
                return View("Error");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                EmpresaSecurity<App_DominioContext> security = new EmpresaSecurity<App_DominioContext>();
                try
                {
                    #region Autorizar
                    Validate result = security.Autorizar(model.UserName.Trim(), model.Password, _sistema_id());
                    if (result.Code > 0)
                        throw new ArgumentException(result.Message);
                    #endregion

                    Sessao s = security.getSessaoCorrente();

                    string sessaoId = result.Field;

                    return RedirectToAction("Default", "Home");
                }
                catch (ArgumentException ex)
                {
                    Error(ex.Message);
                }
                catch (App_DominioException ex)
                {
                    Error("Erro na autorização de acesso. Favor entre em contato com o administrador do sistema");
                }
                catch (DbEntityValidationException ex)
                {
                    Error("Não foi possível autorizar o seu acesso. Favor entre em contato com o administrador do sistema");
                }
                catch (Exception ex)
                {
                    Error("Erro na autorização de acesso. Favor entre em contato com o administrador do sistema");
                }
            }
            else
                Error("Erro de preenchimento de login e senha");

            // If we got this far, something failed, redisplay form
            Condominio Condominio = DWMSessaoLocal.GetCondominioByPathInfo(Request["PathInfo"]);
            if (Condominio == null)
                throw new ArgumentException();

            LoginViewModel value = new LoginViewModel()
            {
                Condominio = Condominio
            };

            return View(value);
        }
        #endregion

        #region Register (Cadastre-se)
        [AllowAnonymous]
        public ActionResult Register(string id)
        {
            RegisterViewModel value = new RegisterViewModel();
            Condominio entity = null;
            if (id != null && id != "")
            {
                // Primeiro o sistema verifica se o id informado é um PathInfo ("ParcParadiso", "Alhpaville", etc)
                // Se não for, o sistema interpreta que o id = validador gerado a partir do envio do token para o condômino por e-mail pela administração

                entity = DWMSessaoLocal.GetCondominioByPathInfo(id);
                if (entity == null)
                    value.UnidadeViewModel = new UnidadeViewModel()
                    {
                        Validador = id
                    };
                else
                    value.CondominioID = entity.CondominioID;

                Factory<RegisterViewModel, ApplicationContext> factory = new Factory<RegisterViewModel, ApplicationContext>();
                value = factory.Execute(new CodigoAtivacaoBI(), value);
                if (entity != null)
                    value.Condominio = entity;
            }
            else
                return RedirectToAction("Login", "Account");

            return View(value);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel value, FormCollection collection)
        {
            if (ModelState.IsValid)
                try
                {
                    value.uri = this.ControllerContext.Controller.GetType().Name.Replace("Controller", "") + "/" + this.ControllerContext.RouteData.Values["action"].ToString();

                    value.UnidadeViewModel = new UnidadeViewModel()
                    {
                        CondominioID = value.CondominioID,
                        EdificacaoID = int.Parse(collection["EdificacaoID"]),
                        UnidadeID = int.Parse(collection["UnidadeID"]),
                        TipoCondomino = collection["TipoCondomino"],
                        Validador = collection["Validador"] 
                    };

                    if (collection ["TipoCondomino"] == "J" && collection["cnpj"].Trim() != "")
                    {
                        value.IndFiscal = collection["cnpj"];
                        value.senha = collection["pwd"];
                        value.confirmacaoSenha = collection["pwdConfirm"];
                    }

                    Factory<RegisterViewModel, ApplicationContext> factory = new Factory<RegisterViewModel, ApplicationContext>();
                    value = factory.Execute(new RegisterBI(), value);

                    if (value.mensagem.Code > 0)
                        throw new ArgumentException(value.mensagem.MessageBase);

                    #region Enviar E-mail
                    using (ApplicationContext db = new ApplicationContext())
                    {
                        using (SecurityContext seguranca_db = new SecurityContext())
                        {
                            var mail = new EnviarEmailRegisterBI(db, seguranca_db);

                            value = mail.Run(value);

                            if (value.mensagem.Code > 0)
                                throw new ArgumentException(value.mensagem.MessageBase);
                        }
                    }
                    #endregion

                    Success("Seu cadastro foi realizado com sucesso.");

                    return RedirectToAction("Login", "Account", new { id = collection["PathInfo"] });
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError("", MensagemPadrao.Message(999).ToString()); // mensagem amigável ao usuário
                    Attention(ex.Message); // Mensagem em inglês com a descrição detalhada do erro e fica no topo da tela
                }
                catch (Exception ex)
                {
                    App_DominioException.saveError(ex, GetType().FullName);
                    ModelState.AddModelError("", MensagemPadrao.Message(17).ToString()); // mensagem amigável ao usuário
                    Error(ex.Message); // Mensagem em inglês com a descrição detalhada do erro e fica no topo da tela
                }
            else
            {
                value.mensagem = new Validate()
                {
                    Code = 999,
                    Message = MensagemPadrao.Message(999).ToString(),
                    MessageBase = ModelState.Values.Where(erro => erro.Errors.Count > 0).First().Errors[0].ErrorMessage == "" ? MensagemPadrao.Message(999).ToString() : ModelState.Values.Where(erro => erro.Errors.Count > 0).First().Errors[0].ErrorMessage
                };
                ModelState.AddModelError("", value.mensagem.Message); // mensagem amigável ao usuário
                Attention(value.mensagem.MessageBase);
                if (collection["UnidadeID"] != null && collection["EdificacaoID"] != null)
                {
                    value.UnidadeViewModel = new Models.Repositories.UnidadeViewModel()
                    {
                        CondominioID = value.CondominioID,
                        EdificacaoID = int.Parse(collection["EdificacaoID"]),
                        UnidadeID = int.Parse(collection["UnidadeID"]),
                        TipoCondomino = collection["TipoCondomino"],
                    };
                }
                else
                {
                    value.UnidadeViewModel = new Models.Repositories.UnidadeViewModel()
                    {
                        CondominioID = value.CondominioID,
                        TipoCondomino = collection["TipoCondomino"]
                    };
                };
            }

            Condominio entity = DWMSessaoLocal.GetCondominioByPathInfo(collection["PathInfo"]);
            if (entity == null)
                return View("Error");

            value.CondominioID = entity.CondominioID;
            value.Condominio = entity;
            value.DescricaoTipoEdificacao = collection["DescricaoTipoEdificacao"];
            return View(value);
        }
        #endregion

        #region Ativar credenciado
        [AllowAnonymous]
        public ActionResult AtivarCredenciado(string id, string key)
        {
            UsuarioRepository value = new UsuarioRepository();
            if (id != null && id != "")
            {
                value.usuarioId = int.Parse(id);
                value.keyword = key;
                Factory<UsuarioRepository, ApplicationContext> factory = new Factory<UsuarioRepository, ApplicationContext>();
                value = factory.Execute(new CodigoValidacaoCredenciadoBI(), value);
                if (value.mensagem.Code == -1)
                {
                    Condominio Condominio = DWMSessaoLocal.GetCondominioByID(value.empresaId);
                    if (Condominio == null)
                        throw new ArgumentException();

                    ViewBag.Condominio = Condominio;
                    return View(value);
                }
                else
                {
                    ModelState.AddModelError("", value.mensagem.MessageBase); // mensagem amigável ao usuário
                    Error(value.mensagem.Message); // Mensagem em inglês com a descrição detalhada do erro e fica no topo da tela
                }
                if (value.empresaId > 0)
                {
                    Condominio c = DWMSessaoLocal.GetCondominioByID(value.empresaId);
                    return RedirectToAction("Login", "Account", new { id = c.PathInfo });
                }
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult AtivarCredenciado(UsuarioRepository value)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (value.usuarioId != 0)
                    {
                        Factory<UsuarioRepository, ApplicationContext> factory = new Factory<UsuarioRepository, ApplicationContext>();
                        value = factory.Execute(new CodigoAtivacaoCredenciadoBI(), value);
                        if (value.mensagem.Code > 0)
                            throw new App_DominioException(value.mensagem);
                        Success("Residente ativado com sucesso. Faça seu login para acessar o sistema");
                        Condominio c = DWMSessaoLocal.GetCondominioByID(value.empresaId);
                        return RedirectToAction("Login", "Account", new { id = c.PathInfo });
                    };
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
            }
            else
                Error("Dados incorretos");

            Condominio Condominio = DWMSessaoLocal.GetCondominioByID(value.empresaId);
            if (Condominio == null)
                throw new ArgumentException();

            ViewBag.Condominio = Condominio;

            return View(value);
        }
        #endregion

        #region Termo de Uso e Política de Privacidade
        [AllowAnonymous]
        public ActionResult TermoUso(string id)
        {
            if (String.IsNullOrEmpty(id))
                throw new ArgumentException();

            Condominio Condominio = DWMSessaoLocal.GetCondominioByPathInfo(id);
            if (Condominio == null)
                throw new ArgumentException();

            ViewBag.Condominio = Condominio;

            return View();
        }

        [AllowAnonymous]
        public ActionResult Politica(string id)
        {
            if (String.IsNullOrEmpty(id))
                throw new ArgumentException();

            Condominio Condominio = DWMSessaoLocal.GetCondominioByPathInfo(id);
            if (Condominio == null)
                throw new ArgumentException();

            ViewBag.Condominio = Condominio;

            return View();
        }
        #endregion

        #region Alterar Senha
        public ActionResult AlterarSenha()
        {
            return View();
        }

        #endregion

        #region Esqueci minha senha
        [AllowAnonymous]
        public ActionResult Forgot(string id)
        {
            if (String.IsNullOrEmpty(id))
                throw new ArgumentException();

            Condominio Condominio = DWMSessaoLocal.GetCondominioByPathInfo(id);
            if (Condominio == null)
                throw new ArgumentException();

            ViewBag.Condominio = Condominio;

            return View(); // RedirectToAction("Default", "Home");
        }

        [ValidateInput(false)]
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Forgot(UsuarioViewModel value, FormCollection collection)
        {
            try
            {
                if (string.IsNullOrEmpty(collection["empresaId"]))
                    throw new Exception("Identificador do condomínio não localizado. Favor entrar em contato com a administração.");

                value.uri = this.ControllerContext.Controller.GetType().Name.Replace("Controller", "") + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
                FactoryLocalhost<UsuarioViewModel, ApplicationContext> factory = new FactoryLocalhost<UsuarioViewModel, ApplicationContext>();
                value = factory.Execute(new EsqueciMinhaSenhaBI(), value);
                if (factory.Mensagem.Code > 0)
                    throw new App_DominioException(factory.Mensagem);

                Success("E-mail com as intruções de renovação de senha enviado com sucesso");
            }
            catch (App_DominioException ex)
            {
                ModelState.AddModelError("", ex.Result.MessageBase); // mensagem amigável ao usuário
                Error(ex.Result.Message); // Mensagem em inglês com a descrição detalhada do erro e fica no topo da tela
                Condominio Condominio1 = DWMSessaoLocal.GetCondominioByPathInfo(collection["PathInfo"]);
                ViewBag.Condominio = Condominio1;
                return View(value);
            }
            catch (Exception ex)
            {
                App_DominioException.saveError(ex, GetType().FullName);
                ModelState.AddModelError("", MensagemPadrao.Message(17).ToString()); // mensagem amigável ao usuário
                Error(ex.Message); // Mensagem em inglês com a descrição detalhada do erro e fica no topo da tela
                Condominio Condominio1 = DWMSessaoLocal.GetCondominioByPathInfo(collection["PathInfo"]);
                ViewBag.Condominio = Condominio1;
                return View(value);
            }

            Condominio Condominio = DWMSessaoLocal.GetCondominioByPathInfo(collection["PathInfo"]);
            if (Condominio == null)
                throw new ArgumentException();

            ViewBag.Condominio = Condominio;

            return RedirectToAction("Login", "Account", new { id = collection["PathInfo"] });
        }

        [AllowAnonymous]
        public ActionResult EsqueciMinhaSenha(string id, string key)
        {
            UsuarioRepository value = new UsuarioRepository();
            if (id != null && id != "")
            {
                value.usuarioId = int.Parse(id);
                value.keyword = key;
                Factory<UsuarioRepository, ApplicationContext> factory = new Factory<UsuarioRepository, ApplicationContext>();
                value = factory.Execute(new CodigoValidacaoCredenciadoBI(), value);
                if (value.mensagem.Code == -1)
                {
                    Condominio Condominio = DWMSessaoLocal.GetCondominioByID(value.empresaId);
                    if (Condominio == null)
                        throw new ArgumentException();

                    ViewBag.Condominio = Condominio;
                    return View(value);
                }
                else
                {
                    ModelState.AddModelError("", value.mensagem.MessageBase); // mensagem amigável ao usuário
                    Error(value.mensagem.Message); // Mensagem em inglês com a descrição detalhada do erro e fica no topo da tela
                }

                if (value.empresaId > 0)
                {
                    Condominio c = DWMSessaoLocal.GetCondominioByID(value.empresaId);
                    return RedirectToAction("Login", "Account", new { id = c.PathInfo });
                }

            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult EsqueciMinhaSenha(UsuarioRepository value)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (value.usuarioId != 0)
                    {
                        Factory<UsuarioRepository, ApplicationContext> factory = new Factory<UsuarioRepository, ApplicationContext>();
                        value = factory.Execute(new CodigoAtivacaoCredenciadoBI(), value);
                        if (value.mensagem.Code > 0)
                            throw new App_DominioException(value.mensagem);
                        Success("Senha alterada com sucesso. Faça seu login para acessar o sistema");
                        Condominio c = DWMSessaoLocal.GetCondominioByID(value.empresaId);
                        return RedirectToAction("Login", "Account", new { id = c.PathInfo });
                    };
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
            }
            else
                Error("Dados incorretos");

            return View(value);
        }

        #endregion

        [AllowAnonymous]
        public ActionResult LogOff()
        {
            System.Web.HttpContext web = System.Web.HttpContext.Current;

            SessaoLocal s = DWMSessaoLocal.GetSessaoLocal();

            if (s != null)
            {
                Condominio Condominio = DWMSessaoLocal.GetCondominioByID(s.empresaId);
                new EmpresaSecurity<App_DominioContext>().EncerrarSessao(web.Session.SessionID);
                return RedirectToAction("Login", "Account", new { id = Condominio.PathInfo });
            }

            return RedirectToAction("Login", "Account", new { id = "" });
        }

        [AllowAnonymous]
        public JsonResult GetNames(string term, int tag)
        {
            var results = new Models.Enumeracoes.BindDropDownList().UnidadesDesocupadas("Selecione...", "", term, tag);
            return new JsonResult()
            {
                Data = results,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }

        [AllowAnonymous]
        public JsonResult GetUnidade(int CondominioID, int EdificacaoID, int UnidadeID)
        {
            UnidadeViewModel key = new UnidadeViewModel()
            {
                CondominioID = CondominioID,
                EdificacaoID = EdificacaoID,
                UnidadeID = UnidadeID
            };
            var results = GetUnidade(key);
            return new JsonResult()
            {
                Data = results,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        private UnidadeViewModel GetUnidade(UnidadeViewModel key)
        {
            Factory<UnidadeViewModel, ApplicationContext> factory = new Factory<UnidadeViewModel, ApplicationContext>();
            UnidadeViewModel value = factory.Execute(new GetUnidadeBI(), key);
            return value;
        }

    }
}