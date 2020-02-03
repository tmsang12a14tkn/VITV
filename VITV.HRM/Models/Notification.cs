using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VITV.HRM.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public NotificationTypes NotificationTypeId { get; set; }
        public string FromId { get; set; }
        public int? ProjectId { get; set; }
        public int? RequestId { get; set; }
        public int? JobId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Url { get; set; }

        public virtual Employee From { get; set; }
        public virtual NotificationType NotificationType { get; set; }
        public virtual Project Project { get; set; }
        public virtual TaskRequest Request { get; set; }
        public virtual Job Job { get; set; }

        [NotMapped]
        public string Content 
        { 
            get
            {
                var target = NotificationType.Id == NotificationTypes.AddedToProject ? Project.Title : 
                            NotificationType.Id == NotificationTypes.AddedToJob || NotificationType.Id == NotificationTypes.ReceivedAResponse? Job.Title:
                            Request.Title;
                return string.Format(NotificationType.Format, From.Name, target);
            }  
        }
        [NotMapped]
        public string LongContent
        {
            get
            {
                return string.Format(NotificationType.Format, From.Name, NotificationType.Id == NotificationTypes.AddedToProject ? Project.Title : Request.Title);
            }
        }
    }
}