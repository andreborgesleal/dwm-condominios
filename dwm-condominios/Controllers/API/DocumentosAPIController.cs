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
using DWM.Controllers.API;
using System.Linq;

namespace dwm_condominios.Controllers.API
{
    public class DocumentosAPIController : DwmApiController
    {
        [HttpPost]
        [AuthorizeFilter]
        public IEnumerable<ArquivoViewModel> ListDocumentos(Auth value)
        {
            // Validar Token
            Auth a = ValidarToken(value);
            if (a.Code != 0)
            {
                ArquivoViewModel arquivoiewModel = new ArquivoViewModel()
                {
                    mensagem = new Validate()
                    {
                        Code = 202,
                        Message = "Acesso Negado. Suas credencias não estão autorizadas para executar esta operação."
                    }
                };
                List<ArquivoViewModel> ret = new List<ArquivoViewModel>
                {
                    arquivoiewModel
                };
                return ret;
            }

            // Listar
            // Parametros
            PageSize = PageSize == null || PageSize == "" ? "8" : PageSize;
            DateTime Data1 = Funcoes.Brasilia().AddMonths(-6);
            DateTime Data2 = Funcoes.Brasilia().Date;

            SessaoLocal s = DWMSessaoLocal.GetSessaoLocal(a.Token);
            var _EdificacaoID = s.Unidades.FirstOrDefault().EdificacaoID;
            var _UnidadeID = s.Unidades.FirstOrDefault().UnidadeID;
            var _CondominoID = s.CondominoID;
            var _Grupo = s.GrupoCondominoID;
            var _Nome = "";

            Facade<ArquivoViewModel, ArquivoModel, ApplicationContext> facade = new Facade<ArquivoViewModel, ArquivoModel, ApplicationContext>();
            IEnumerable<ArquivoViewModel> list = facade.List(new ListViewArquivo(), 0, int.Parse(PageSize), value.Token, Data1, Data2, _EdificacaoID, _UnidadeID, _CondominoID, 0, null);

            return list;
        }
    }
}