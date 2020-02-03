using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VITV.HRM.Models
{
    public enum NotificationTypes
    {
        AddedToProject = 1,
        AddedToTask = 2,
        AddedToJob = 3,
        ReceivedAResponse = 4,
    }
    public class NotificationType
    {
        [Key]
        public NotificationTypes Id { get; set; }
        public string Format { get; set; }
    }
}