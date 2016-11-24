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
    public class FilaAtendimentoController : DwmRootController<FilaAtendimentoViewModel, FilaAtendimentoModel, ApplicationContext>
    {
        #region Inheritance
        public override int _sistema_id() { return (int)DWM.Models.Enumeracoes.Sistema.DWMCONDOMINIOS; }

        public override string getListName()
        {
            return "Listar";
        }
        #endregion

        #region List
        [AuthorizeFilter]
        public override ActionResult List(int? index, int? pageSize = 50, string descricao = null)
        {
            if (ViewBag.ValidateRequest)
            {
                ListViewFilaAtendimento list = new ListViewFilaAtendimento();
                return this._List(index, pageSize, "Browse", list, descricao);
            }
            else
                return View();
        }
        #endregion

        #region Create
        public override void BeforeCreate(ref FilaAtendimentoViewModel value, FormCollection collection)
        {
            base.BeforeCreate(ref value, collection);
            value.VisibilidadeCondomino = collection["VisibilidadeCondomino"] == "on" ? "S" : "N";
        }
        #endregion

        #region Edit
        [AuthorizeFilter]
        public ActionResult Edit(int FilaAtendimentoID)
        {
            return _Edit(new FilaAtendimentoViewModel() { FilaAtendimentoID = FilaAtendimentoID });
        }

        public override void BeforeEdit(ref FilaAtendimentoViewModel value, FormCollection collection)
        {
            BeforeCreate(ref value, collection);
        }
        #endregion

        #region Delete
        [AuthorizeFilter]
        public ActionResult Delete(int FilaAtendimentoID)
        {
            return Edit(FilaAtendimentoID);
        }

        public override void BeforeDelete(ref FilaAtendimentoViewModel value, FormCollection collection)
        {
            BeforeCreate(ref value, collection);
        }

        #endregion

        #region FilaAtendimentoUsuario
        public ActionResult Index(int id)
        {
            ViewBag.ValidateRequest = true;
            if (ViewBag.ValidateRequest)
            {
                //BindBreadCrumb(getListName(), ClearBreadCrumbOnBrowse());

                BindBreadCrumb(getBreadCrumbText("Fila de Atendimento", null));

                Factory<FilaAtendimentoUsuarioEditViewModel, ApplicationContext> factory = new Factory<FilaAtendimentoUsuarioEditViewModel, ApplicationContext>();
                FilaAtendimentoUsuarioEditViewModel value = new FilaAtendimentoUsuarioEditViewModel()
                {
                    FilaAtendimentoUsuarioViewModel = new FilaAtendimentoUsuarioViewModel()
                    {
                        FilaAtendimentoID = id,
                    }
                };
                FilaAtendimentoUsuarioEditViewModel obj = factory.Execute(new FilaAtendimentoUsuarioBI(), value);
                return View(obj);
            }
            return View();
        }

        [AuthorizeFilter]
        public ActionResult EditUser(string FilaAtendimentoID, string UsuarioID, string Situacao, string Operacao)
        {
            if (ModelState.IsValid)
            {
                if (ViewBag.ValidateRequest)
                {
                    FilaAtendimentoUsuarioEditViewModel result = null;
                    try
                    {
                        #region Incluir/Editar FilaAtendimentoUsuario
                        result = new FilaAtendimentoUsuarioEditViewModel()
                        {
                            FilaAtendimentoUsuarioViewModel = new FilaAtendimentoUsuarioViewModel()
                            {
                                FilaAtendimentoID = int.Parse(FilaAtendimentoID),
                                UsuarioID = int.Parse(UsuarioID),
                                Situacao = Situacao,
                                mensagem = new App_Dominio.Contratos.Validate() { Code = 0 }
                            },
                            mensagem = new App_Dominio.Contratos.Validate() { Code = 0 }
                        };

                        result.FilaAtendimentoUsuarioViewModel.uri = this.ControllerContext.Controller.GetType().Name.Replace("Controller", "") + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
                        FactoryLocalhost<FilaAtendimentoUsuarioViewModel, ApplicationContext> factory = new FactoryLocalhost<FilaAtendimentoUsuarioViewModel, ApplicationContext>();
                        result.FilaAtendimentoUsuarios = factory.Execute(new FilaAtendimentoUsuarioCadastrarBI(Operacao), result.FilaAtendimentoUsuarioViewModel, int.Parse(FilaAtendimentoID));
                        if (factory.Mensagem.Code > 0)
                            throw new App_DominioException(factory.Mensagem);

                        FilaAtendimentoUsuarioModel FilaAtendimentoUsuarioModel = new FilaAtendimentoUsuarioModel();
                        result.FilaAtendimentoUsuarioViewModel = FilaAtendimentoUsuarioModel.CreateRepository(Request);
                        result.FilaAtendimentoUsuarioViewModel.FilaAtendimentoID = int.Parse(FilaAtendimentoID);
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

                    return View("_EditUser", result);
                }
                else
                {
                    return View();
                }
            }
            else
            {
                FilaAtendimentoUsuarioEditViewModel result = new FilaAtendimentoUsuarioEditViewModel()
                {
                    FilaAtendimentoUsuarioViewModel = new FilaAtendimentoUsuarioViewModel()
                    {
                        FilaAtendimentoID = int.Parse(FilaAtendimentoID),
                        mensagem = new App_Dominio.Contratos.Validate() { Code = 0 }
                    },
                    mensagem = new App_Dominio.Contratos.Validate() { Code = 0 }
                };

                Factory<FilaAtendimentoUsuarioEditViewModel, ApplicationContext> factory = new Factory<FilaAtendimentoUsuarioEditViewModel, ApplicationContext>();
                FilaAtendimentoUsuarioEditViewModel obj = factory.Execute(new FilaAtendimentoUsuarioBI(), result);
                result.FilaAtendimentoUsuarios = obj.FilaAtendimentoUsuarios;

                Error("Erro de preenhcimento em campos");

                return View("_EditUser", result);
            }
        }


        #endregion


    }
}