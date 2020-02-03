using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VITV.Data.Models
{
    public class ControllerAction
    {
        [Key]
        public int Id { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string Site { get; set; }    //admin or portal
        public string Description { get; set; }

        public virtual ICollection<ControllerActionPermission> Permissions { get; set; }
    }
    public class ControllerActionPermission
    {
        [Key, Column(Order=0)]
        [ForeignKey("ControllerAction")]
        public int ControllerActionId { get; set; }
        [Key, Column(Order = 1)]
        [ForeignKey("Role")]
        public string RoleId { get; set; }

        public virtual IdentityRole Role { get; set; }
        public virtual ControllerAction ControllerAction { get; set; }

    }
}
