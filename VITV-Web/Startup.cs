using Microsoft.Owin;
using Owin;
[assembly: OwinStartupAttribute(typeof(VITV.Web.Startup))]
namespace VITV.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}