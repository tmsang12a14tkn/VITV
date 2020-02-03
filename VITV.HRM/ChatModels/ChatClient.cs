using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VITV.HRM.Models;

namespace VITV.HRM.ChatModels
{
    public class ChatClient
    {
        public string DisplayName { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Avatar { get; set; }
        public List<string> ConnectionIds { get; set; }
        public ChatClient()
        { }
        public ChatClient(Employee user)
        {
            DisplayName = user.Name;
            UserId = user.Id;
        }
    }
}