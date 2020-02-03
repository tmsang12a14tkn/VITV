using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace VITV.DataService.Entity
{
    public class BroadcastScheduleModel
    {
        [DataMember]
        public ICollection<ProgramScheduleDetailModel> Programs { get; set; }
    }
}
