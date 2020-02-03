using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VITV.Data.Models;
namespace VITV.Data.ViewModels
{
    public enum PageType: int
    {
        Startup = 1
    }

    public class PageGroupModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public string UniqueTitle { get; set; }
    }
}
