using System;
using System.Runtime.Serialization;

namespace VITV.DataService.Entity
{
    public class ProgramScheduleDetailModel
    {
        [DataMember]
        public string VideoCategory { get; set; }
        
        [DataMember]
        public int? VideoId { get; set; }
        
        [DataMember]
       public DateTime DateTime { get; set; }

        [DataMember]
        public string Name { get; set; }

        //[DataMember]
        //public bool IsNew { get; set; }

    }
}