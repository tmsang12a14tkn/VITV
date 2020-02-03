using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace VITV.DataService.Entity
{
    [DataContract]
    public class ReporterToListEntity
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string ProfilePicture { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Biography { get; set; }
        [DataMember]
        public string Introduction { get; set; }
        [DataMember]
        public List<VideoToListEntity> Videos { get; set; }
        
    }
}