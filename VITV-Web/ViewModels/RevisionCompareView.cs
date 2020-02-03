using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VITV_Web.ViewModels
{
    public class RevisionView{
        public string ChangedDate { get; set; }
        public string UserName { get; set; }
        public string Title { get; set; }
        public string ShortBrief { get; set; }
        public string ArticleContent { get; set; }
        public int Order { get; set; }
    }
    public class RevisionCompareView
    {
        public RevisionView Revision1 { get; set; }
        public RevisionView Revision2 { get; set; }
        public string DiffOutput { get; set; }
        public string Title { get; set; }
        public int Count { get; set; }
        public int ArticleId { get; set; }
    }
}