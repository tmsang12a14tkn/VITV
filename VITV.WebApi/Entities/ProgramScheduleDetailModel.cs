using System;
using System.Runtime.Serialization;

namespace VITV.WebApi.Entities
{
    public class ProgramScheduleDetailModel
    {
        
        public string VideoCategory { get; set; }
        
        
        public int? VideoId { get; set; }
        
        
       public DateTime DateTime { get; set; }

        
        public string Name { get; set; }

        //
        //public bool IsNew { get; set; }

    }
}