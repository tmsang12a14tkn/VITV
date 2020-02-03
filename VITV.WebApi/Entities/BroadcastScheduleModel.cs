using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace VITV.WebApi.Entities
{
    public class BroadcastScheduleModel
    {
        
        public ICollection<ProgramScheduleDetailModel> Programs { get; set; }
    }
}
