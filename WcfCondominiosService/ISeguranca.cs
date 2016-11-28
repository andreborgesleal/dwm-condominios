using System.ServiceModel;
using WcfCondominiosService.Models;

namespace WcfCondominiosService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISeguranca" in both code and config file together.
    [ServiceContract]
    public interface ISeguranca
    {
        [OperationContract]
        Auth Autenticar(string login, string senha, string ip);
        [OperationContract]
        Auth Validar(string Token);
    }
}
