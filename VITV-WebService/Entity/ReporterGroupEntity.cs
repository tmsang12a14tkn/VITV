using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace VITV.DataService.Entity
{
    [DataContract]
    public class ReporterGroupEntity
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public List<ReporterEntity> Reporters { get; set; }
        
    }
}