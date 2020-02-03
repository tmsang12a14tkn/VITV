using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VITV.Data.ViewModels
{
    public class VideoView
    {
        public int Id { get; set; }

        public string UniqueTitle { get; set; }
        public string Thumbnail { get; set; }

        public string Title { get; set; }

        public DateTime PublishedTime { get; set; }
        public DateTime DisplayTime { get; set; }

        public string CategoryName { get; set; }

        public bool IsHot { get; set; }

    }
}