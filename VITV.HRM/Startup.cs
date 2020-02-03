using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VITV.HRM.Startup))]
namespace VITV.HRM
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
