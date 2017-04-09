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

namespace DWM.Controllers.API
{
    public class GrupoCondominoAPIController : DwmApiController
    {
        public IEnumerable<GrupoCondominoViewModel> ListGrupos(Auth value)
        {
            // Validar Token
            Auth a = ValidarToken(value);
            if (a.Code != 0)
            {
                GrupoCondominoViewModel GrupoCondominoViewModel = new GrupoCondominoViewModel()
                {
                    mensagem = new Validate()
                    {
                        Code = 202,
                        Message = "Acesso Negado. Suas credencias não estão autorizadas para executar esta operação."
                    }
                };
                List<GrupoCondominoViewModel> ret = new List<GrupoCondominoViewModel>();
                ret.Add(GrupoCondominoViewModel);
                return ret;
            }
            

            // Listar
            PageSize = PageSize == null || PageSize == "" ? "8" : PageSize;
            Facade<GrupoCondominoViewModel, GrupoCondominoModel, ApplicationContext> facade = new Facade<GrupoCondominoViewModel, GrupoCondominoModel, ApplicationContext>();
            IEnumerable<GrupoCondominoViewModel> list = facade.List(new ListViewGrupoCondominos(), 0, int.Parse(PageSize), value.Token);

            return list;
        }
    }
}