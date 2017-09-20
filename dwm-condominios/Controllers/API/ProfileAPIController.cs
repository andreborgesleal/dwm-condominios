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
using DWM.Models.BI;
using App_Dominio.Enumeracoes;
using System.Linq;
using System.Web;

namespace DWM.Controllers.API
{
    public class ProfileAPIController : DwmApiController
    {
        [HttpPost]
        [AuthorizeFilter]
        public CondominoEditViewModel GetProfile(Auth value)
        {
            Auth a = ValidarToken(value);

            if (a.Code != 0)
            {
                CondominoEditViewModel condominoEditViewModel = new CondominoEditViewModel()
                {
                    mensagem = new Validate()
                    {
                        Code = 202,
                        Message = "Acesso Negado. Suas credencias não estão autorizadas para executar esta operação."
                    }
                };

                return condominoEditViewModel;
            }

            SessaoLocal s = DWMSessaoLocal.GetSessaoLocal(a.Token);

            FactoryLocalhost<CondominoEditViewModel, ApplicationContext> factory = new FactoryLocalhost<CondominoEditViewModel, ApplicationContext>();
            CondominoEditViewModel condominoEditViewModel2 = new CondominoEditViewModel()
            {
                UnidadeViewModel = new UnidadeViewModel()
                {
                    EdificacaoID = s.Unidades.FirstOrDefault().EdificacaoID,
                    UnidadeID = s.Unidades.FirstOrDefault().UnidadeID,
                },
                CredenciadoViewModel = new CredenciadoViewModel()
                {
                    CondominoID = s.CondominoID,
                }
            };

            condominoEditViewModel2.CondominoViewModel = new CondominoPFViewModel()
            {
                CondominoID = s.CondominoID
            };
            condominoEditViewModel2.sessionId = s.sessaoId;


            
            CondominoEditViewModel obj = factory.Execute(new EditarCondominoBI(), condominoEditViewModel2, value.Token);
            if (obj.CondominoViewModel.mensagem.Code > 0)
                obj = null;

            return obj;

        }
    }
}