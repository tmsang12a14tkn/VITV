using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VITV.HRM.Models
{
    public class ResponseAttachment
    {
        [Key, Column(Order = 0)]
        public int JobResponseId { get; set; }
        [Key, Column(Order = 1)]
        public string AttachmentLink { get; set; }
        public virtual JobResponse JobResponse { get; set; }
    }
}