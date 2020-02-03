using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VITV.HRM.ViewModels
{
    public class ListNotiChatModel
    {
        public string id { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string MessageContent { get; set; }
        public bool Own { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool IsView { get; set; }
    }
}