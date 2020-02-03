using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace VITV.WebApi.Entities
{
   
    public class VideoToListEntity
    {
        
        public int Id { get; set; }
        
        public string Thumbnail { get; set; }
        
        public string MobileThumbnail { get; set; }
        
        public string Url { get; set; }
        
        public string Title { get; set; }
        
        public string Content { get; set; }

        
        public DateTime PublishedTime { get; set; }
    }
}