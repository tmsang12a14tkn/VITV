using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VITV.HRM.Models
{
    public class Message
    {
        [Key, Column(Order = 0)]
        public string SenderId { get; set; }
        [Key, Column(Order = 1)]
        public string ReceiverId { get; set; }
        [Key, Column(Order = 2)]
        public DateTime CreatedTime { get; set; }
        public string MessageContent { get; set; }
        [DefaultValue("false")]
        public bool IsView { get; set; }
        [ForeignKey("SenderId")]
        public virtual Employee Sender { get; set; }
        [ForeignKey("ReceiverId")]
        public virtual Employee Receiver { get; set; }

    }
}