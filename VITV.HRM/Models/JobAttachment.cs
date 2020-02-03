using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VITV.HRM.Models
{
    public class JobAttachment
    {
        [Key, Column(Order=0)]
        public int JobId { get; set; }
        [Key, Column(Order = 1)]
        public string AttachmentLink { get; set; }
        public virtual Job Job { get; set; }
        
    }
}