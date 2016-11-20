using System;
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
        public async Task<ActionResult> Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(); // RedirectToAction("Default", "Home");
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

            // If we got this far, something failed, redisplay form
            return View(model);

        }
        #endregion

        #region Register (Cadastre-se)
        [AllowAnonymous]
        public ActionResult Register(string id)
        {
            RegisterViewModel value = new RegisterViewModel();
            if (id != null && id != "")
            {
                if (id.Trim().Length > 10)
                {
                    value.UnidadeViewModel = new Models.Repositories.UnidadeViewModel()
                    {
                        Validador = id
                    };
                }
                else
                {
                    value.CondominioID = int.Parse(id);
                }

                Factory<RegisterViewModel, ApplicationContext> factory = new Factory<RegisterViewModel, ApplicationContext>();
                value = factory.Execute(new CodigoAtivacaoBI(), value);
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

                    value.UnidadeViewModel = new Models.Repositories.UnidadeViewModel()
                    {
                        CondominioID = value.CondominioID,
                        EdificacaoID = int.Parse(collection["EdificacaoID"]),
                        UnidadeID = int.Parse(collection["UnidadeID"])
                    };

                    // Aciona o pattern local do aplicativo - Pattern Localhost (e não o do App_Dominio)
                    Factory<RegisterViewModel, ApplicationContext> factory = new Factory<RegisterViewModel, ApplicationContext>();
                    value = factory.Execute(new RegisterBI(), value);

                    if (value.mensagem.Code > 0)
                        throw new ArgumentException(value.mensagem.MessageBase);

                    #region Enviar E-mail
                    value = factory.Execute(new EnviarEmailBI(), value);

                    if (value.mensagem.Code > 0)
                        throw new ArgumentException(value.mensagem.MessageBase);
                    #endregion

                    Success("Seu cadastro foi realizado com sucesso.");
                    return RedirectToAction("Login", "Account");
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
                        UnidadeID = int.Parse(collection["UnidadeID"])
                    };
                }
                else
                {
                    value.UnidadeViewModel = new Models.Repositories.UnidadeViewModel()
                    {
                        CondominioID = value.CondominioID,
                    };
                };
            }

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
                    return View(value);
                else
                {
                    ModelState.AddModelError("", value.mensagem.MessageBase); // mensagem amigável ao usuário
                    Error(value.mensagem.Message); // Mensagem em inglês com a descrição detalhada do erro e fica no topo da tela
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
                        return RedirectToAction("Login", "Account");
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

        #region Termo de Uso e Política de Privacidade
        [AllowAnonymous]
        public ActionResult TermoUso()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Politica()
        {
            return View();
        }
        #endregion

        [AllowAnonymous]
        public ActionResult LogOff()
        {
            System.Web.HttpContext web = System.Web.HttpContext.Current;
            new EmpresaSecurity<App_DominioContext>().EncerrarSessao(web.Session.SessionID);

            return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        public JsonResult GetNames(string term, int tag)
        {
            var results = new Models.Enumeracoes.BindDropDownList().Unidades("", "", term, tag);

            return new JsonResult()
            {
                Data = results,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }

    }

    
}