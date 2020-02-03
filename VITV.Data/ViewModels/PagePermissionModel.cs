using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VITV.Data.Models;

namespace VITV.Data.ViewModels
{
    public class PagePermissionModel
    {
        public int ControllerActionId { get; set; }

        public string RoleId { get; set; }
        public bool CanAccess { get; set; }
    }
    public class PagePermissionSiteModel
    {
        public string Site { get; set; }
        public List<ControllerAction> ControllerActions { get; set; }
    }
}
