using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace VITV.HRM.Models
{
    public class NotificationHub: Hub
    {

        public void SendNotificationTo(string userId)
        {
            Clients.User(userId).receiveNotification();
        }

        public string ReceiveNotification(string userId)
        {
            var db = new VITVSecondContext();
            var user = db.Users.Find(int.Parse(userId));

            //Tính số thông báo
            int notifiCount = (from notifi in db.Notifications
                               join userNotifi in db.UserNotifications on notifi.Id equals userNotifi.NotificationId
                               where userNotifi.EmployeeId == user.Id && userNotifi.IsRead == false
                               select notifi).Count();

            return notifiCount.ToString(CultureInfo.InvariantCulture);
        }
    }
}