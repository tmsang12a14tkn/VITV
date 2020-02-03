using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Mvc;

namespace VITV.WebApi.Entities
{
   
    public class VideoEntity
    {
        
        public int Id { get; set; }
        
        public string Thumbnail { get; set; }
        
        public string MobileThumbnail { get; set; }
        
        public string Url { get; set; }
         
        public string Title { get; set; }
        [AllowHtml]
        public string Content { get; set; }

        
        public DateTime PublishedTime { get; set; }
        
        public int CategoryId { get; set; }
        
        public List<ReporterEntity> Reporters { get; set; }
        
        public bool IsHot { get; set; }
    }
}