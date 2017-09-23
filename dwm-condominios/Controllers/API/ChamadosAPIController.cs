using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using DWM.Models.Entidades;
using DWM.Models.Repositories;
using App_Dominio.Pattern;
using App_Dominio.Models;
using DWM.Models.Persistence;
using DWM.Models.API;
using DWM.Models.Pattern;
using App_Dominio.Security;
using System.Web.Http.Cors;
using App_Dominio.Contratos;
using App_Dominio.Component;
using DWM.Models.BI;
using System.Linq;
using App_Dominio.Repositories;

namespace DWM.Controllers.API
{
    public class ChamadosAPIController : DwmApiController
    {

        [HttpPost]
        [AuthorizeFilter]
        public IEnumerable<ChamadoViewModel> ListChamados(Auth value)
        {
            // Validar Token
            Auth a = ValidarToken(value);
            if (a.Code != 0)
            {
                ChamadoViewModel chamadoViewModel = new ChamadoViewModel()
                {
                    mensagem = new Validate()
                    {
                        Code = 202,
                        Message = "Acesso Negado. Suas credencias não estão autorizadas para executar esta operação."
                    }
                };
                List<ChamadoViewModel> ret = new List<ChamadoViewModel>();
                ret.Add(chamadoViewModel);
                return ret;
            }
            

            // Listar
            PageSize = PageSize == null || PageSize == "" ? "8" : PageSize;
            Facade<ChamadoViewModel, ChamadoModel, ApplicationContext> facade = new Facade<ChamadoViewModel, ChamadoModel, ApplicationContext>();
            IEnumerable<ChamadoViewModel> list = facade.List(new ListViewChamado(), 0, int.Parse(PageSize), value.Token);

            
            return list;
        }

        [HttpPost]
        [AuthorizeFilter]
        public ChamadoViewModel ListChamadoDetalhe(ChamadoViewModel value)
        {
            // Validar Token
            ChamadoViewModel chamadoViewModel = (ChamadoViewModel)ValidarToken(value);
            if (chamadoViewModel.mensagem.Code != 0)
            {
                ChamadoViewModel cvm = new ChamadoViewModel()
                {
                    mensagem = new Validate()
                    {
                        Code = chamadoViewModel.mensagem.Code,
                        Message = "Acesso Negado. Suas credencias não estão autorizadas para executar esta operação."
                    }
                };
                return cvm;
            }

            Factory<ChamadoViewModel, ApplicationContext> factory = new Factory<ChamadoViewModel, ApplicationContext>();
            ChamadoViewModel obj = factory.Execute(new ChamadoEditBI(), chamadoViewModel, chamadoViewModel.sessionId);
            return obj;
        }

        public IEnumerable<FilaAtendimentoViewModel> ListFilaAtendimento(Auth value)
        {
            // Validar Token
            Auth a = ValidarToken(value);
            if (a.Code != 0)
            {
                FilaAtendimentoViewModel filaAtendimentoViewModel = new FilaAtendimentoViewModel()
                {
                    mensagem = new Validate()
                    {
                        Code = 202,
                        Message = "Acesso Negado. Suas credencias não estão autorizadas para executar esta operação."
                    }
                };
                List<FilaAtendimentoViewModel> ret = new List<FilaAtendimentoViewModel>();
                ret.Add(filaAtendimentoViewModel);
                return ret;
            }


            // Listar
            PageSize = PageSize == null || PageSize == "" ? "8" : PageSize;
            Facade<FilaAtendimentoViewModel, FilaAtendimentoModel, ApplicationContext> facade = new Facade<FilaAtendimentoViewModel, FilaAtendimentoModel, ApplicationContext>();
            IEnumerable<FilaAtendimentoViewModel> list = facade.List(new ListViewFilaAtendimento(), 0, int.Parse(PageSize), value.Token);


            return list;

        }

        public IEnumerable<FilaAtendimentoViewModel> ListFilaAtendimentoSemCondomino(Auth value)
        {
            // Validar Token
            Auth a = ValidarToken(value);
            if (a.Code != 0)
            {
                FilaAtendimentoViewModel filaAtendimentoViewModel = new FilaAtendimentoViewModel()
                {
                    mensagem = new Validate()
                    {
                        Code = 202,
                        Message = "Acesso Negado. Suas credencias não estão autorizadas para executar esta operação."
                    }
                };
                List<FilaAtendimentoViewModel> ret = new List<FilaAtendimentoViewModel>();
                ret.Add(filaAtendimentoViewModel);
                return ret;
            }


            // Listar
            PageSize = PageSize == null || PageSize == "" ? "8" : PageSize;
            Facade<FilaAtendimentoViewModel, FilaAtendimentoModel, ApplicationContext> facade = new Facade<FilaAtendimentoViewModel, FilaAtendimentoModel, ApplicationContext>();
            IEnumerable<FilaAtendimentoViewModel> list = facade.List(new ListViewFilaAtendimento(), 0, int.Parse(PageSize), value.Token).Where(x => x.Descricao != "Condôminos");

            return list;

        }

        public IEnumerable<FilaAtendimentoViewModel> ListFilaAtendimentoComCondomino(Auth value)
        {
            // Validar Token
            Auth a = ValidarToken(value);
            if (a.Code != 0)
            {
                FilaAtendimentoViewModel filaAtendimentoViewModel = new FilaAtendimentoViewModel()
                {
                    mensagem = new Validate()
                    {
                        Code = 202,
                        Message = "Acesso Negado. Suas credencias não estão autorizadas para executar esta operação."
                    }
                };
                List<FilaAtendimentoViewModel> ret = new List<FilaAtendimentoViewModel>();
                ret.Add(filaAtendimentoViewModel);
                return ret;
            }


            // Listar
            PageSize = PageSize == null || PageSize == "" ? "8" : PageSize;
            Facade<FilaAtendimentoViewModel, FilaAtendimentoModel, ApplicationContext> facade = new Facade<FilaAtendimentoViewModel, FilaAtendimentoModel, ApplicationContext>();
            IEnumerable<FilaAtendimentoViewModel> list = facade.List(new ListViewFilaAtendimento(), 0, int.Parse(PageSize), value.Token).Where(x => x.Descricao == "Condôminos");

            return list;

        }

        public IEnumerable<ChamadoMotivoViewModel> ListChamadoMotivo(Auth value)
        {
            // Validar Token
            Auth a = ValidarToken(value);
            if (a.Code != 0)
            {
                ChamadoMotivoViewModel chamadoMotivoViewModel = new ChamadoMotivoViewModel()
                {
                    mensagem = new Validate()
                    {
                        Code = 202,
                        Message = "Acesso Negado. Suas credencias não estão autorizadas para executar esta operação."
                    }
                };
                List<ChamadoMotivoViewModel> ret = new List<ChamadoMotivoViewModel>();
                ret.Add(chamadoMotivoViewModel);
                return ret;
            }


            // Listar
            PageSize = PageSize == null || PageSize == "" ? "8" : PageSize;
            Facade<ChamadoMotivoViewModel, ChamadoMotivoModel, ApplicationContext> facade = new Facade<ChamadoMotivoViewModel, ChamadoMotivoModel, ApplicationContext>();
            IEnumerable<ChamadoMotivoViewModel> list = facade.List(new ListViewChamadoMotivo(), 0, int.Parse(PageSize), value.Token);

            return list;

        }

        [HttpPost]
        public Auth CreateChamado(ChamadoViewModel value)
        {
            Auth auth = new Auth()
            {
                Code = 0,
                Mensagem = "Sucesso!"
            };

            ChamadoViewModel chamadoViewModel = (ChamadoViewModel)ValidarToken(value);
            if (chamadoViewModel.mensagem.Code != 0)
            {
                Auth cvm = new Auth()
                {
                    Mensagem = "Acesso Negado.",
                    Code = 202
                };
                return cvm;
            }

            ChamadoViewModel result = new ChamadoViewModel()
            {
                uri = this.ControllerContext.Controller.GetType().Name.Replace("Controller", "") + "/" + this.ControllerContext.RouteData.Values["action"].ToString(),
                Assunto = value.Assunto,
                ChamadoStatusID = value.ChamadoStatusID,
                ChamadoMotivoID = value.ChamadoMotivoID,
                CondominioID = value.empresaId,
                empresaId = value.empresaId,
                MensagemOriginal = value.MensagemOriginal,
                EdificacaoID = value.EdificacaoID,
                UnidadeID = value.UnidadeID,
                FilaSolicitanteID = value.FilaSolicitanteID,
                FilaAtendimentoID = value.FilaAtendimentoID,
                ChamadoAnexoViewModel = new ChamadoAnexoViewModel(),
                ChamadoFilaViewModel = new ChamadoFilaViewModel()
                {
                    Data = Funcoes.Brasilia(),
                    FilaAtendimentoID = value.FilaAtendimentoID.Value,
                },
                Rotas = new List<ChamadoFilaViewModel>(),
                Anexos = new List<ChamadoAnexoViewModel>(),
                sessionId = value.sessionId,
                Prioridade = "2",
            };

            //#region Envia Email
            //try
            //{
            //    FactoryLocalhost<AlertaRepository, ApplicationContext> factory = new FactoryLocalhost<AlertaRepository, ApplicationContext>();
            //    AlertaBI bi = new AlertaBI();
            //    value.uri = this.ControllerContext.Controller.GetType().Name.Replace("Controller", "") + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
            //    AlertaRepository a = factory.Execute(new AlertaBI(), value);
            //    if (a.mensagem.Code > 0)
            //        throw new Exception(a.mensagem.Message);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            //#endregion

            try
            {
                FacadeLocalhost<ChamadoViewModel, ChamadoModel, ApplicationContext> facade = new FacadeLocalhost<ChamadoViewModel, ChamadoModel, ApplicationContext>();
                facade.Save(result, App_Dominio.Enumeracoes.Crud.INCLUIR);
            }
            catch(Exception e)
            {
                Console.Write(e.Message);
            }
            

            return auth;
        }


        public Auth EditChamado(AnotacaoAPIModel value)
        {

            Auth auth = new Auth()
            {
                Code = 0,
                Mensagem = "Sucesso!"
            };

            ChamadoViewModel chamadoViewModel = (ChamadoViewModel)ValidarToken(value);
            if (chamadoViewModel.mensagem.Code != 0)
            {
                Auth cvm = new Auth()
                {
                    Mensagem = "Acesso Negado.",
                    Code = 202
                };
                return cvm;
            }


            ChamadoViewModel result = null;
            try
            {
                int? FilaAtendimentoID = null;
                if (value.FilaAtendimentoAtualID != value.FilaAtendimentoID.ToString())
                    FilaAtendimentoID = value.FilaAtendimentoID;

                #region Incluir Anotação
                result = new ChamadoViewModel()
                {
                    ChamadoID = value.ChamadoID,
                    uri = this.ControllerContext.Controller.GetType().Name.Replace("Controller", "") + "/" + this.ControllerContext.RouteData.Values["action"].ToString(),
                    ChamadoAnexoViewModel = new ChamadoAnexoViewModel(),
                    ChamadoFilaViewModel = new ChamadoFilaViewModel(),
                    ChamadoAnotacaoViewModel = new ChamadoAnotacaoViewModel()
                    {
                        ChamadoID = value.ChamadoID,
                        Mensagem = value.Anotacoes.FirstOrDefault().Mensagem
                    },
                    ChamadoStatusID = value.ChamadoStatusID,
                    FilaAtendimentoID = FilaAtendimentoID,
                    DescricaoFilaAtendimento = value.DescricaoFilaAtendimento,
                    DescricaoChamadoStatus = value.DescricaoChamadoStatus,
                    DataRedirecionamento = Funcoes.Brasilia(),
                    Rotas = new List<ChamadoFilaViewModel>(),
                    Anexos = new List<ChamadoAnexoViewModel>(),
                    mensagem = new Validate() { Code = 0 },
                    sessionId = value.sessionId,
                };
                if (value.FilaAtendimentoAtualID == value.FilaAtendimentoID.ToString())
                    result.IsUsuarioFila = false;
                else
                    result.IsUsuarioFila = true;

                result.ChamadoAnotacaoViewModel.uri = this.ControllerContext.Controller.GetType().Name.Replace("Controller", "") + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
                FactoryLocalhost<ChamadoAnotacaoViewModel, ApplicationContext> factory = new FactoryLocalhost<ChamadoAnotacaoViewModel, ApplicationContext>();
                factory.Execute(new ChamadoAnotacaoBI(), result, value.sessionId);
                if (factory.Mensagem.Code > 0)
                    throw new App_DominioException(factory.Mensagem);
                
                #endregion

                #region Emitir Alerta e enviar o e-mail para a fila destinatária
                FactoryLocalhost<AlertaRepository, ApplicationContext> factoryAlert = new FactoryLocalhost<AlertaRepository, ApplicationContext>();
                AlertaBI bi = new AlertaBI();
                AlertaRepository a = factoryAlert.Execute(new AlertaBI(), result);
                if (a.mensagem.Code > 0)
                    throw new Exception(a.mensagem.Message);
                #endregion

                #region Recupera o ChamadoViewModel
                FactoryLocalhost<ChamadoViewModel, ApplicationContext> factoryChamado = new FactoryLocalhost<ChamadoViewModel, ApplicationContext>();
                result = factoryChamado.Execute(new ChamadoEditBI(), result);
                #endregion
            }
            catch (App_DominioException ex)
            {
                ModelState.AddModelError("", ex.Result.MessageBase); // mensagem amigável ao usuário
            }
            catch (Exception ex)
            {
                App_DominioException.saveError(ex, GetType().FullName);
            }

            return auth;
        }


    }
}
