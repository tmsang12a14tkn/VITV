using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VITV.HRM.Models
{
    public class UserNotification
    {
        [Key, Column(Order = 0)]
        public string EmployeeId { get; set; }

        [Key, Column(Order = 1)]
        public int NotificationId { get; set; }

        public bool IsRead { get; set; }
        public DateTime CreatedTime { get; set; } 

        public virtual Notification Notification { get; set; }

        public virtual Employee Employee { get; set; }

        public UserNotification()
        {
            IsRead = false;
            CreatedTime = DateTime.Now;

        }

    }
}