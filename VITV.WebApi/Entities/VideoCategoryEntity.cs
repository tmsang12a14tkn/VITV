using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace VITV.WebApi.Entities
{
   
    public class VideoCategoryEntity
    {
         public int Id { get; set; }
        
        public string Name { get; set; }
        
        public int? GroupId { get; set; }
        
        public string Description { get; set; }
        
        public string Introduction { get; set; }
        
        public string IntroductionBonus { get; set; }
        
        public string ProfilePhoto { get; set; }
        
        public string MobileProfilePhoto { get; set; }
    }
}