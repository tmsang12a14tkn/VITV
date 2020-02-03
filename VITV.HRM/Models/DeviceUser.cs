using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VITV.HRM.Models
{
    public class DeviceUser
    {
        [Key, Column(Order=0)]
        [ForeignKey("User")]
        public string UserId { get; set; }
        [Key, Column(Order = 1)]
        public string DeviceId { get; set; }
        public string DeviceToken { get; set; }
        [Key, Column(Order = 2)]
        public int DeviceType { get; set; } //0: ios, 1:android
        public string LoginToken { get; set; }
        public DateTime ExpiredTime { get; set; }
        public bool Logged { get;set;}

        public virtual ApplicationUser User { get; set; }
    }
}