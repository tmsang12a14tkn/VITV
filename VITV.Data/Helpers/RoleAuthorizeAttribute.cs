using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;

namespace VITV.Data.Helpers
{
    public class RoleAuthorizeAttribute : AuthorizeAttribute
    {
        public string Site { get; set; }
        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {

            bool isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized) return false;
            var actionName = httpContext.Request.RequestContext.RouteData.GetRequiredString("action");
            var controllerName = httpContext.Request.RequestContext.RouteData.GetRequiredString("controller");


            var context = new DAL.VITVContext();
            var controllerAction = context.ControllerActions.FirstOrDefault(ca => ca.ActionName == actionName && ca.ControllerName == controllerName && ca.Site == Site);
            if(controllerAction == null)
            {
                context.ControllerActions.Add(new Models.ControllerAction
                    {
                        Site = Site,
                        ActionName = actionName,
                        ControllerName = controllerName
                    });
                context.SaveChanges();
                return false;
            }

            var controllerActionPermission = context.ControllerActionPermissions.Where(p => p.ControllerActionId == controllerAction.Id).ToList().FirstOrDefault(p=>httpContext.User.IsInRole(p.Role.Name));
            if (controllerActionPermission != null)
                return true;
            else
                return false;
        }
    }
}
