using App_Dominio.Entidades;
using App_Dominio.Security;
using DWM.Models.Interfaces;
using System.IO;
using System.Web;

namespace DWM.Models.Repositories
{
    public class UsuarioViewModel : App_Dominio.Repositories.UsuarioRepository, IPathArquivos
    {
        public string Path()
        {
            EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
            return "../Users_Data/Empresas/" + security.getSessaoCorrente().empresaId.ToString() + "/Avatar/";
        }

        public string Extension(string FileID)
        {
            System.IO.FileInfo f = new System.IO.FileInfo(System.IO.Path.Combine(Path(), FileID));
            return f.Extension;
        }

        public string Avatar(string size = "30")
        {
            if (usuarioId == 0 || empresaId == 0)
            {
                EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
                Sessao sessaoCorrente = security.getSessaoCorrente();
                empresaId = sessaoCorrente.empresaId;
                usuarioId = sessaoCorrente.usuarioId;
            }

            FileInfo f = new FileInfo(HttpContext.Current.Server.MapPath("") + "..\\..\\Users_Data\\Empresas\\" + empresaId.ToString() + "\\Avatar\\" + usuarioId.ToString() + ".png");
            if (f.Exists)
                return "../Users_Data/Empresas/" + empresaId.ToString() + "/Avatar/" + usuarioId.ToString() + ".png";
            else
            {
                f = new FileInfo(HttpContext.Current.Server.MapPath("") + "..\\..\\Users_Data\\Empresas\\" + empresaId.ToString() + "\\Avatar\\" + usuarioId.ToString() + ".jpg");
                if (f.Exists)
                    return "../Users_Data/Empresas/" + empresaId.ToString() + "/Avatar/" + usuarioId.ToString() + ".jpg";
                else
                {
                    f = new FileInfo(HttpContext.Current.Server.MapPath("") + "..\\..\\Users_Data\\Empresas\\" + empresaId.ToString() + "\\Avatar\\" + usuarioId.ToString() + ".bmp");
                    if (f.Exists)
                        return "../Users_Data/Empresas/" + empresaId.ToString() + "/Avatar/" + usuarioId.ToString() + ".bmp";
                    else
                        return "http://api.ning.com/files/XDvieCk-6Hj1PFXyHT13r7Et-ybLOKWFR9fYd15dBrqFQHv6gCVuGdr4GYjaO0u*h2E0p*c5ZVHE-H41wNz4uAGNfcH8LLZS/top_8_silhouette_male_120.jpg?width=" + size;
                }
            }
        }
    }
}