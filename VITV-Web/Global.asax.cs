using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using VITV.Web.Helpers;
using VITV.Web.Mappings;

namespace VITV.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
                        
            Bootstrapper.Run();
            AutoMapperConfiguration.Configure();

            ModelBinders.Binders.Add(typeof(DateTime), new DateTimeBinder());
            ModelBinders.Binders.Add(typeof(DateTime?), new NullableDateTimeBinder());
        }

        //protected void Application_BeginRequest(object sender, EventArgs e)
        //{
        //    var culture = new CultureInfo("en-GB");
        //    Thread.CurrentThread.CurrentCulture = culture;
        //    Thread.CurrentThread.CurrentUICulture = culture;
        //}
    }
}
