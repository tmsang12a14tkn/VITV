using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VITV.HRM.Models
{
    public class DepartmentRoleView
    {
        public string RoleName { get; set; }
        public List<Group> Groups { get; set; }
    }
    public class DepartmentLocationView
    {
        public string LocationName { get; set; }
        public List<DepartmentRoleView> Roles { get; set; }
    }
    public class DepartmentArrangeView
    {
        public List<Department> Departments { get; set; }
        public List<Employee> UnassignEmployees { get; set; }
    }
}