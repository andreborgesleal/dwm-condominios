using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DWM.Startup))]
namespace DWM
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
