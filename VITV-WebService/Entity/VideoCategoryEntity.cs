using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace VITV.DataService.Entity
{
    [DataContract]
    public class VideoCategoryEntity
    {
        [DataMember] public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int? GroupId { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string Introduction { get; set; }
        [DataMember]
        public string IntroductionBonus { get; set; }
        [DataMember]
        public string ProfilePhoto { get; set; }
        [DataMember]
        public string MobileProfilePhoto { get; set; }
    }
}