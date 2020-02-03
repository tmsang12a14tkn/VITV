using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VITV.Data.Models;

namespace VITV.Data.ViewModels
{
    public class ReporterGroupView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Employee> Reporters { get; set; }
    }

    public class EmployeePositionModel
    {
        public string NameEmp { get; set; }

        public string NamePosi { get; set; }

    }
}