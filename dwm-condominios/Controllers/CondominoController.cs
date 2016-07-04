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

namespace DWM.Controllers
{
    public class CondominoController : DwmRootController<CondominoUnidadeViewModel, CondominoUnidadeModel, ApplicationContext>
    {
        #region Inheritance
        public override int _sistema_id() { return (int)DWM.Models.Enumeracoes.Sistema.DWMCONDOMINIOS; }

        public override string getListName()
        {
            return "Condômino";
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
                Factory<CondominoEditViewModel, ApplicationContext> factory = new Factory<CondominoEditViewModel, ApplicationContext>();
                CondominoEditViewModel value = new CondominoEditViewModel()
                {
                    UnidadeViewModel = new UnidadeViewModel()
                    {
                        EdificacaoID = EdificacaoID,
                        UnidadeID = UnidadeID,
                        CondominioID = id
                    },
                    CondominoPFViewModel = new CondominoPFViewModel()
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
                        throw new Exception(result.mensagem.MessageBase);

                    Success("Condômino alterado com sucesso");
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

        #endregion

        #region Enviar Token
        [AuthorizeFilter]
        public ActionResult EnviarToken()
        {
            ViewBag.ValidateRequest = true;
            if (ViewBag.ValidateRequest)
            {
                EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
                ViewBag.empresaId = security.getSessaoCorrente().empresaId;

                return View(new UnidadeViewModel());
            }
            return View();
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