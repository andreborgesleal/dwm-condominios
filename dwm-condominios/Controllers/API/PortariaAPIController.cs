using System;
using System.Collections.Generic;
using System.Web.Http;
using DWM.Models.Entidades;
using DWM.Models.Repositories;
using App_Dominio.Pattern;
using App_Dominio.Models;
using DWM.Models.Persistence;
using DWM.Models.API;
using DWM.Models.Pattern;
using App_Dominio.Contratos;

namespace DWM.Controllers.API
{
    public class PortariaAPIController : DwmApiController
    {
        public IEnumerable<VisitanteAcessoViewModel> ListVisitanteAcesso(Auth value)
        {
            Auth a = ValidarToken(value);
            if (a.Code != 0)
            {
                VisitanteAcessoViewModel visitanteAcessoViewModel = new VisitanteAcessoViewModel()
                {
                    mensagem = new Validate()
                    {
                        Code = 202,
                        Message = "Acesso Negado. Suas credencias não estão autorizadas para executar esta operação."
                    }
                };
                List<VisitanteAcessoViewModel> ret = new List<VisitanteAcessoViewModel>();
                ret.Add(visitanteAcessoViewModel);
                return ret;
            }

            // Listar
            PageSize = PageSize == null || PageSize == "" ? "8" : PageSize;
            Facade<VisitanteAcessoViewModel, VisitanteAcessoModel, ApplicationContext> facade = new Facade<VisitanteAcessoViewModel, VisitanteAcessoModel, ApplicationContext>();
            IEnumerable<VisitanteAcessoViewModel> list = facade.List(new ListViewVisitanteAcesso(), 0, int.Parse(PageSize), value.Token);

            return list;
        }

        public Auth CreateVisitanteAcesso(VisitanteAcessoViewModel value)
        {
            Auth auth = new Auth()
            {
                Code = 0,
                Mensagem = "Sucesso!"
            };

            VisitanteAcessoViewModel visitanteAcessoViewModel = (VisitanteAcessoViewModel)ValidarToken(value);
            if (visitanteAcessoViewModel.mensagem.Code != 0)
            {
                auth.Code = 202;
                auth.Mensagem = "Acesso Negado.";
                return auth;
            }

            SessaoLocal se = DWMSessaoLocal.GetSessaoLocal(value.sessionId);
            int? codUnidade = 0;
            int? codEdificacao = 0;
            int empresaId = se.empresaId;

            foreach (var un in se.Unidades)
            {
                codUnidade = un.UnidadeID;
                codEdificacao = un.EdificacaoID;
            }

            VisitanteAcessoViewModel result = new VisitanteAcessoViewModel()
            {
                AcessoID = value.AcessoID,
                uri = this.ControllerContext.Controller.GetType().Name.Replace("Controller", "") + "/" + this.ControllerContext.RouteData.Values["action"].ToString(),
                CondominioID = se.empresaId,
                empresaId = se.empresaId,
                EdificacaoID = codEdificacao,
                UnidadeID = codUnidade,
                sessionId = value.sessionId,
                HoraInicio = value.HoraInicio,
                HoraLimite = value.HoraLimite,
                DataAcesso = value.DataAcesso,
                DataInclusao = Funcoes.Brasilia(),
                Observacao = value.Observacao,
                Interfona = value.Interfona,
                VisitanteID = value.VisitanteID,
                DataAutorizacao = value.DataAutorizacao,
            };

            try
            {
                FacadeLocalhost<VisitanteAcessoViewModel, VisitanteAcessoModel, ApplicationContext> facade = new FacadeLocalhost<VisitanteAcessoViewModel, VisitanteAcessoModel, ApplicationContext>();

                if (result.AcessoID == 0)
                    facade.Save(result, App_Dominio.Enumeracoes.Crud.INCLUIR);
                else
                    facade.Save(result, App_Dominio.Enumeracoes.Crud.ALTERAR);
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }

            auth.Code = result.mensagem.Code.GetValueOrDefault();
            auth.Mensagem = result.mensagem.Message;

            return auth;
        }

        [HttpPost]
        public Auth DeleteAcesso(VisitanteAcessoViewModel value)
        {
            Auth auth = new Auth()
            {
                Code = 0,
                Mensagem = "Sucesso!",
                Token = value.sessionId
            };

            VisitanteAcessoViewModel visitanteViewModel = (VisitanteAcessoViewModel)ValidarToken(value);
            if (visitanteViewModel.mensagem.Code != 0)
            {
                auth.Code = 202;
                auth.Mensagem = "Acesso Negado.";
                return auth;
            }

            SessaoLocal se = DWMSessaoLocal.GetSessaoLocal(value.sessionId);

            int? codUnidade = 0;
            int? codEdificacao = 0;
            int empresaId = se.empresaId;

            foreach (var un in se.Unidades)
            {
                codUnidade = un.UnidadeID;
                codEdificacao = un.EdificacaoID;
            }

            VisitanteAcessoViewModel result = new VisitanteAcessoViewModel()
            {
                uri = this.ControllerContext.Controller.GetType().Name.Replace("Controller", "") + "/" + this.ControllerContext.RouteData.Values["action"].ToString(),
                VisitanteID = value.VisitanteID,
                AcessoID = value.AcessoID,
                sessionId = value.sessionId,
                CondominioID = value.CondominioID,
                EdificacaoID = codEdificacao,
                UnidadeID = codUnidade,
            };

            try
            {
                FacadeLocalhost<VisitanteAcessoViewModel, VisitanteAcessoModel, ApplicationContext> facade = new FacadeLocalhost<VisitanteAcessoViewModel, VisitanteAcessoModel, ApplicationContext>();
                facade.Save(result, App_Dominio.Enumeracoes.Crud.EXCLUIR);
            }
            catch (Exception e)
            {
                auth.Mensagem = e.Message;
                Console.Write(e.Message);
            }

            return auth;
        }

    }
}