using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace VITV.WebApi.Entities
{
   
    public class ReporterEntity
    {
        
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string ProfilePicture { get; set; }
        
        public string Phone { get; set; }
        
        public string Email { get; set; }
        
        public string Biography { get; set; }
        
        public string Introduction { get; set; }
    }
}