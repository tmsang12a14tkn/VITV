using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace VITV.DataService.Entity
{
    [DataContract]
    public class VideoToListEntity
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Thumbnail { get; set; }
        [DataMember]
        public string MobileThumbnail { get; set; }
        [DataMember]
        public string Url { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Content { get; set; }

        [DataMember]
        public DateTime PublishedTime { get; set; }
    }
}