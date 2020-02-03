using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VITV.Portal.Startup))]
namespace VITV.Portal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
