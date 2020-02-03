using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VITV.Data.Models;

namespace VITV.Data.ViewModels
{
    public class CategoryIntroManage
    {
        public VideoCatGroup Group { get; set; }
        public List<IntroByCategory> CategoryIntroGroups { get; set; }
    }

    public class IntroByCategory
    {
        public VideoCategory Category { get; set; }
        public List<CategoryIntro> Intros { get; set; }
    }
}
