using App_Dominio.Models;
using App_Dominio.Pattern;
using DWM.Models.Entidades;
using DWM.Models.Persistence;
using DWM.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using WcfCondominiosService.Models;

namespace WcfCondominiosService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Informativo" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Informativo.svc or Informativo.svc.cs at the Solution Explorer and start debugging.
    public class Informativo : IInformativo
    {
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "list/{Token}/{PageSize}")]
        public IEnumerable<InformativoViewModel> ListInformativos(string Token, string PageSize)
        {
            // Validar Token
            Seguranca seg = new Seguranca();
            Auth a = seg.Validar(Token);
            if (a.Code != 0)
                return new List<InformativoViewModel>();

            // Listar
            PageSize = PageSize == null || PageSize == "" ? "8" : PageSize;
            DateTime Data1 = Funcoes.Brasilia().AddDays(-20);
            DateTime Data2 = Funcoes.Brasilia().Date;
            Facade<InformativoViewModel, InformativoModel, ApplicationContext> facade = new Facade<InformativoViewModel, InformativoModel, ApplicationContext>();
            IEnumerable<InformativoViewModel> list = facade.List(new ListViewInformativo(), 0, int.Parse(PageSize), Token, Data1, Data2);

            int contador = 0;
            foreach(InformativoViewModel info in list)
            {
                if (info.DataExpiracao.HasValue)
                    list.ElementAt(contador)._DataExpiracao = info.DataExpiracao.Value.ToString("dd/MM/yyyy");

                if (info.DataInformativo != null)
                    list.ElementAt(contador)._DataInformativo = info.DataInformativo.ToString("dd/MM/yyyy");

                if (info.DataPublicacao != null)
                    list.ElementAt(contador)._DataPublicacao = info.DataPublicacao.ToString("dd/MM/yyyy");

                contador++;
            }


            return list;
        }
    }
}
