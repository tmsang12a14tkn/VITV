using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VITV.HRM.Models.View
{
    public class ProfilePageModel
    {
        public Employee Employee { get; set; }

        public RegisterViewModel RegisterViewModel { get; set; }

        public IEnumerable<Equipment> Equipments { get; set; }
    }
}