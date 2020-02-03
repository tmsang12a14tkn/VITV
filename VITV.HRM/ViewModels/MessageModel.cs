using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VITV.HRM.ViewModels
{
    public class MessageModel
    {
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public DateTime CreatedTime { get; set; }
        public string MessageContent { get; set; }
    }
}