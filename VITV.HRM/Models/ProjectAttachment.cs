using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VITV.HRM.Models
{
    public class ProjectAttachment
    {
        [Key, Column(Order=0)]
        public int ProjectId { get; set; }
        [Key, Column(Order = 1)]
        public string AttachmentLink { get; set; }
        public virtual Project Project { get; set; }
        
    }
}