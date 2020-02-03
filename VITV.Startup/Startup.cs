using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VITV.Startup.Startup))]
namespace VITV.Startup
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
