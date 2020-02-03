using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace VITV.WebApi.Entities
{
   
    public class ReporterGroupEntity
    {
        
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public List<ReporterEntity> Reporters { get; set; }
        
    }
}