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
using App_Dominio.Contratos;
using App_Dominio.Repositories;

namespace DWM.Controllers
{
    public class ChamadoController : DwmRootController<ChamadoViewModel, ChamadoModel, ApplicationContext>
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
            if (ViewBag.ValidateRequest)
            {
                ListViewChamado cham = new ListViewChamado();
                return this._List(index, pageSize, "Browse", cham, descricao);
            }
            else
                return View();
        }
        #endregion
        #endregion

        public override void BeforeCreate(ref ChamadoViewModel value, FormCollection collection)
        {
            base.BeforeCreate(ref value, collection);

            #region Anexos
            value.Anexos = new List<ChamadoAnexoViewModel>();
            if (collection["File1ID"] != null && collection["File1ID"] != "")
            {
                ChamadoAnexoViewModel anexo1 = new ChamadoAnexoViewModel()
                {
                    FileID = collection["File1ID"],
                    DataAnexo = Funcoes.Brasilia(),
                    NomeOriginal = collection["NomeOriginal1"],
                };
                ((List<ChamadoAnexoViewModel>)value.Anexos).Add(anexo1);
            }
            if (collection["File2ID"] != null && collection["File2ID"] != "")
            {
                ChamadoAnexoViewModel anexo2 = new ChamadoAnexoViewModel()
                {
                    FileID = collection["File2ID"],
                    DataAnexo = Funcoes.Brasilia(),
                    NomeOriginal = collection["NomeOriginal2"],
                };
                ((List<ChamadoAnexoViewModel>)value.Anexos).Add(anexo2);
            }
            if (collection["File3ID"] != null && collection["File3ID"] != "")
            {
                ChamadoAnexoViewModel anexo3 = new ChamadoAnexoViewModel()
                {
                    FileID = collection["File3ID"],
                    DataAnexo = Funcoes.Brasilia(),
                    NomeOriginal = collection["NomeOriginal3"],
                };
                ((List<ChamadoAnexoViewModel>)value.Anexos).Add(anexo3);
            }
            #endregion

            #region Identificação do Condômino
            if (collection ["__EdificacaoID"] != null && collection["__EdificacaoID"] != "")
            {
                value.EdificacaoID = int.Parse(collection["__EdificacaoID"]);
                value.UnidadeID = int.Parse(collection["__UnidadeID"]);
                value.CondominoID = int.Parse(collection["__CondominoID"]);
            }
            #endregion

            if (Request ["_ChamadoMotivoID"] != null && Request["_ChamadoMotivoID"] != "")
                value.ChamadoMotivoID = int.Parse(collection["_ChamadoMotivoID"]);

            if (Request["_FilaSolicitanteID"] != null && Request["_FilaSolicitanteID"] != "")
                value.FilaSolicitanteID = int.Parse(collection["_FilaSolicitanteID"]);

            if (Request["_FilaAtendimentoID"] != null && Request["_FilaAtendimentoID"] != "")
            {
                value.FilaAtendimentoID = int.Parse(collection["_FilaAtendimentoID"]);
                value.ChamadoFilaViewModel = new ChamadoFilaViewModel()
                {
                    Data = Funcoes.Brasilia(),
                    FilaAtendimentoID = value.FilaAtendimentoID.Value
                };
            }

            value.MensagemOriginal = collection["MensagemOriginal"];
            value.uri = this.ControllerContext.Controller.GetType().Name.Replace("Controller", "") + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
        }

        public override void OnCreateError(ref ChamadoViewModel value, FormCollection collection)
        {
            base.OnCreateError(ref value, collection);
            FacadeLocalhost<ChamadoViewModel, ChamadoModel, ApplicationContext> facade = new FacadeLocalhost<ChamadoViewModel, ChamadoModel, ApplicationContext>();
            Validate validate = value.mensagem;

            value = facade.CreateRepository(Request);
            value.mensagem = validate;
            value.MensagemOriginal = collection["MensagemOriginal"] ?? "";
        }

        public override ActionResult AfterCreate(ChamadoViewModel value, FormCollection collection)
        {
            try
            {
                FactoryLocalhost<AlertaRepository, ApplicationContext> factory = new FactoryLocalhost<AlertaRepository, ApplicationContext>();
                AlertaBI bi = new AlertaBI();
                value.uri = this.ControllerContext.Controller.GetType().Name.Replace("Controller", "") + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
                AlertaRepository a = factory.Execute(new AlertaBI(), value);
                if (a.mensagem.Code > 0)
                    throw new Exception(a.mensagem.Message);
            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }

            return RedirectToAction("Browse");
        }

        [AuthorizeFilter]
        public ActionResult Index(int id)
        {
            ViewBag.ValidateRequest = true;
            if (ViewBag.ValidateRequest)
            {
                BindBreadCrumb(getBreadCrumbText("Chamado", null));

                FactoryLocalhost<ChamadoViewModel, ApplicationContext> factory = new FactoryLocalhost<ChamadoViewModel, ApplicationContext>();
                ChamadoViewModel value = new ChamadoViewModel()
                {
                    ChamadoID = id
                };

                ChamadoViewModel obj = factory.Execute(new ChamadoEditBI(), value);
                return View(obj);
            }
            return View();
        }

        [ValidateInput(false)]
        [AuthorizeFilter]
        public ActionResult EditAnotacao(int ChamadoID, string Mensagem, string FilaAtendimentoAtualID, string _FilaAtendimentoID, string DescricaoFilaAtendimento, string _ChamadoStatusID, string DescricaoChamadoStatus)
        {
            if (ModelState.IsValid)
            {
                if (ViewBag.ValidateRequest)
                {
                    ChamadoViewModel result = null;
                    try
                    {
                        int? FilaAtendimentoID = null;
                        if (FilaAtendimentoAtualID != _FilaAtendimentoID)
                            FilaAtendimentoID = int.Parse(_FilaAtendimentoID);

                        #region Incluir Anotação
                        result = new ChamadoViewModel()
                        {
                            ChamadoID = ChamadoID,
                            ChamadoAnexoViewModel = new ChamadoAnexoViewModel(),
                            ChamadoFilaViewModel = new ChamadoFilaViewModel(),
                            ChamadoAnotacaoViewModel = new ChamadoAnotacaoViewModel()
                            {
                                ChamadoID = ChamadoID,
                                Mensagem = Mensagem
                            },
                            ChamadoStatusID = _ChamadoStatusID == null || _ChamadoStatusID == "" ? 0 : int.Parse(_ChamadoStatusID),
                            FilaAtendimentoID = FilaAtendimentoID,
                            DescricaoFilaAtendimento = DescricaoFilaAtendimento,
                            DescricaoChamadoStatus = DescricaoChamadoStatus,
                            DataRedirecionamento = Funcoes.Brasilia(),
                            Rotas = new List<ChamadoFilaViewModel>(),
                            Anexos = new List<ChamadoAnexoViewModel>(),
                            mensagem = new Validate() { Code = 0 }
                        };
                        if (FilaAtendimentoAtualID == _FilaAtendimentoID)
                            result.IsUsuarioFila = false;
                        else
                            result.IsUsuarioFila = true;

                        result.ChamadoAnotacaoViewModel.uri = this.ControllerContext.Controller.GetType().Name.Replace("Controller", "") + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
                        FactoryLocalhost<ChamadoAnotacaoViewModel, ApplicationContext> factory = new FactoryLocalhost<ChamadoAnotacaoViewModel, ApplicationContext>();
                        result.Anotacoes = factory.Execute(new ChamadoAnotacaoBI(), result, ChamadoID);
                        if (factory.Mensagem.Code > 0)
                            throw new App_DominioException(factory.Mensagem);

                        ChamadoAnotacaoModel model = new ChamadoAnotacaoModel();
                        ChamadoAnotacaoViewModel ChamadoAnotacaoViewModel = model.CreateRepository(Request);
                        result.ChamadoAnotacaoViewModel.ChamadoID = ChamadoID;
                        #endregion

                        if (FilaAtendimentoAtualID != _FilaAtendimentoID)
                            result.IsUsuarioFila = false;
                        else
                            result.IsUsuarioFila = true;

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

                    return View("_Anotacao", result);
                }
                else
                {
                    return View();
                }
            }
            else
            {
                //CondominoEditViewModel result = new CondominoEditViewModel()
                //{
                //    UnidadeViewModel = new UnidadeViewModel()
                //    {
                //        EdificacaoID = VeiculoViewModel.EdificacaoID,
                //        UnidadeID = VeiculoViewModel.UnidadeID,
                //        CondominioID = VeiculoViewModel.CondominioID
                //    },
                //    CondominoPFViewModel = new CondominoPFViewModel()
                //    {
                //        CondominoID = VeiculoViewModel.CondominoID
                //    },
                //    VeiculoViewModel = VeiculoViewModel
                //};

                //Factory<CondominoEditViewModel, ApplicationContext> factory = new Factory<CondominoEditViewModel, ApplicationContext>();
                //CondominoEditViewModel obj = factory.Execute(new EditarCondominoBI(), result);
                //result.Veiculos = obj.Veiculos;

                //Error("Erro de preenhcimento em campos");

                return View("_Anotacao", result);
            }

        }


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