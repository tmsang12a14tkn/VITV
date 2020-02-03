using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VITV.HRM.Models
{
    public class GroupNotification
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int GroupId { get; set; }
        public bool isAvailable { get; set; }
        public virtual Group Group { get; set; }

    }
}