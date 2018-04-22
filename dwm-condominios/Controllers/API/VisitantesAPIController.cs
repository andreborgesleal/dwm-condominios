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
    public class VisitantesAPIController : DwmApiController
    {
       public IEnumerable<VisitanteViewModel> ListVisitantes(Auth value)
       {
            Auth a = ValidarToken(value);
            if (a.Code != 0)
            {
                VisitanteViewModel visitanteViewModel = new VisitanteViewModel()
                {
                    mensagem = new Validate()
                    {
                        Code = 202,
                        Message = "Acesso Negado. Suas credencias não estão autorizadas para executar esta operação."
                    }
                };
                List<VisitanteViewModel> ret = new List<VisitanteViewModel>();
                ret.Add(visitanteViewModel);
                return ret;
            }

            // Listar
            PageSize = PageSize == null || PageSize == "" ? "8" : PageSize;
            Facade<VisitanteViewModel, VisitanteModel, ApplicationContext> facade = new Facade<VisitanteViewModel, VisitanteModel, ApplicationContext>();
            IEnumerable<VisitanteViewModel> list = facade.List(new ListViewVisitante(), 0, int.Parse(PageSize), value.Token);

            return list;
        }


        [HttpPost]
        public Auth CreateVisitante(VisitanteViewModel value)
        {
            Auth auth = new Auth()
            {
                Code = 0,
                Mensagem = "Sucesso!"
            };

            VisitanteViewModel visitanteViewModel = (VisitanteViewModel)ValidarToken(value);
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
            
            foreach(var un in se.Unidades)
            {
                codUnidade = un.UnidadeID;
                codEdificacao = un.EdificacaoID;
            }

            VisitanteViewModel result = new VisitanteViewModel()
            {
                uri = this.ControllerContext.Controller.GetType().Name.Replace("Controller", "") + "/" + this.ControllerContext.RouteData.Values["action"].ToString(),
                CondominioID = se.empresaId,
                empresaId = se.empresaId,
                EdificacaoID = codEdificacao,
                UnidadeID = codUnidade,
                sessionId = value.sessionId,
                Cor = value.Cor,
                CPF = value.CPF,
                Descricao = value.Descricao,
                DataInclusao = Funcoes.Brasilia(),
                Email = value.Email,
                Marca = value.Marca,
                Nome = value.Nome,
                OrgaoEmissor = value.OrgaoEmissor,
                Placa = value.Placa,
                PrestadorCondominio = value.PrestadorCondominio,
                RG = value.RG,
                Sexo = value.Sexo,
                Telefone = value.Telefone,
                Situacao = "A",
                VisitanteID = value.VisitanteID,
                Fotografia = value.Fotografia,
                VisitanteUnidadeViewModel = new List<VisitanteUnidadeViewModel>()
                {
                    new VisitanteUnidadeViewModel()
                    {
                        CondominioID = se.empresaId,
                        CondominoID = se.CondominoID,
                        CredenciadoID = se.CredenciadoID == 0 ? null : se.CredenciadoID,
                        EdificacaoID = codEdificacao.Value,
                        empresaId = se.empresaId,
                        UnidadeID = codUnidade.Value
                    }
                }
            };

            try
            {
                FacadeLocalhost<VisitanteViewModel, VisitanteModel, ApplicationContext> facade = new FacadeLocalhost<VisitanteViewModel, VisitanteModel, ApplicationContext>();
                
                if (result.VisitanteID == 0)
                    facade.Save(result, App_Dominio.Enumeracoes.Crud.INCLUIR);
                else
                    facade.Save(result, App_Dominio.Enumeracoes.Crud.ALTERAR);
            }
            catch (Exception e)
            {
                auth.Code = -1;
                auth.Mensagem = e.Message;
                Console.Write(e.Message);
            }

            return auth;
        }

        [HttpPost]
        public Auth DeleteVisitante(VisitanteViewModel value)
        {
            Auth auth = new Auth()
            {
                Code = 0,
                Mensagem = "Sucesso!"
            };

            VisitanteViewModel visitanteViewModel = (VisitanteViewModel)ValidarToken(value);
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

            VisitanteViewModel result = new VisitanteViewModel()
            {
                uri = this.ControllerContext.Controller.GetType().Name.Replace("Controller", "") + "/" + this.ControllerContext.RouteData.Values["action"].ToString(),
                VisitanteID = value.VisitanteID,
                sessionId = value.sessionId,
            };

            try
            {
                FacadeLocalhost<VisitanteViewModel, VisitanteModel, ApplicationContext> facade = new FacadeLocalhost<VisitanteViewModel, VisitanteModel, ApplicationContext>();
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