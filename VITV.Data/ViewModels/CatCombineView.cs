using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VITV.Data.ViewModels
{
    public class CatCombineView
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int IPViewCount { get; set; }
        public int PageViewCount { get; set; }
        public double PercentIPViewCount { get; set; }
        public double PercentPageViewCount { get; set; }
    }
}
