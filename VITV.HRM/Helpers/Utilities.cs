using PushSharp;
using PushSharp.Apple;
using PushSharp.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using VITV.HRM.Models;

namespace VITV.HRM.Helpers
{
    public static class Utilities
    {
        public static string RenderRazorViewToString(this Controller controller, string viewName, object model)
        {

            controller.ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(controller.ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        public static bool DoesIncludeSundaySaturday(DateTime startDate, DateTime endDate)
        {
            bool r = false;
            TimeSpan testSpan = new TimeSpan(6, 0, 0, 0);
            TimeSpan actualSpan = endDate - startDate;

            if (actualSpan >= testSpan) { r = true; }
            else
            {
                DateTime checkDate = endDate;
                while (checkDate >= startDate)
                {
                    r = (checkDate.DayOfWeek == DayOfWeek.Sunday || checkDate.DayOfWeek == DayOfWeek.Saturday);
                    if (r) { break; }
                    checkDate = checkDate.AddDays(-1);
                }
            }

            return r;
        }

        public static string URLFriendly(string title)
        {
            if (title == null) return "";

            const int maxlen = 80;
            int len = title.Length;
            bool prevdash = false;
            var sb = new StringBuilder(len);
            char c;

            for (int i = 0; i < len; i++)
            {
                c = title[i];
                if ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9'))
                {
                    sb.Append(c);
                    prevdash = false;
                }
                else if (c >= 'A' && c <= 'Z')
                {
                    // tricky way to convert to lowercase
                    sb.Append((char)(c | 32));
                    prevdash = false;
                }
                else if (c == ' ' || c == ',' || c == '.' || c == '/' ||
                    c == '\\' || c == '-' || c == '_' || c == '=')
                {
                    if (!prevdash && sb.Length > 0)
                    {
                        sb.Append('-');
                        prevdash = true;
                    }
                }
                else if ((int)c >= 128)
                {
                    int prevlen = sb.Length;
                    sb.Append(RemapInternationalCharToAscii(c));
                    if (prevlen != sb.Length) prevdash = false;
                }
                if (i == maxlen) break;
            }

            if (prevdash)
                return sb.ToString().Substring(0, sb.Length - 1);
            else
                return sb.ToString();
        }

        private static string RemapInternationalCharToAscii(char c)
        {
            string s = c.ToString().ToLowerInvariant();
            if ("àåáäãåąảạăằắẳẳẵặâấầẩẫậ".Contains(s))
            {
                return "a";
            }
            else if ("èéëęẻẽẹêếềểễệ".Contains(s))
            {
                return "e";
            }
            else if ("ìíîïıỉĩị".Contains(s))
            {
                return "i";
            }
            else if ("òóôõöøőðốồổỗộơớờởỡợ".Contains(s))
            {
                return "o";
            }
            else if ("ùúûüŭůủũụưứừửữự".Contains(s))
            {
                return "u";
            }
            else if ("çćčĉ".Contains(s))
            {
                return "c";
            }
            else if ("żźž".Contains(s))
            {
                return "z";
            }
            else if ("śşšŝ".Contains(s))
            {
                return "s";
            }
            else if ("ñń".Contains(s))
            {
                return "n";
            }
            else if ("ýÿỳỷỹ".Contains(s))
            {
                return "y";
            }
            else if ("ğĝ".Contains(s))
            {
                return "g";
            }
            else if (c == 'ř')
            {
                return "r";
            }
            else if (c == 'ł')
            {
                return "l";
            }
            else if (c == 'đ')
            {
                return "d";
            }
            else if (c == 'ß')
            {
                return "ss";
            }
            else if (c == 'Þ')
            {
                return "th";
            }
            else if (c == 'ĥ')
            {
                return "h";
            }
            else if (c == 'ĵ')
            {
                return "j";
            }
            else
            {
                return "";
            }
        }

        public static void SendDayoffPush(int dayoffId)
        {
            var context = new VITVSecondContext();
            var dayoff = context.PersonalDayoffs.Find(dayoffId);
            var deviceTokens = context.DeviceUsers.Where(du => du.Logged == true).Select(du => du.DeviceToken).ToList();
            foreach(var dt in deviceTokens)
            {
                SendPush(dt, dayoff.Employee.Name+"-"+dayoff.Start.ToShortDateString()+"-"+ (string.IsNullOrEmpty(dayoff.Title)?"Không lý do":dayoff.Title), 0);
            }
        }

        public static void SendDayoffPushAndroid(PersonalDayoff dayoff)
        {
            var context = new VITVSecondContext();
            var deviceTokens = context.DeviceUsers.Where(du => du.Logged == true).Select(du => du.DeviceToken).ToList();
            foreach (var dt in deviceTokens)
            {
                SendPushAndroid(dt, string.IsNullOrEmpty(dayoff.Title) ? "Không lý do" : dayoff.Title);
            }
        }

        public static void SendPushAndroid(string deviceId, string message)
        {
            //Server API key
            var GoogleAppID = "AIzaSyBGUEYU-ctzKQZ9fXNjYA6I2fZa9Igovx4";
            //Project Number ( google api console )
            var SENDER_ID = "148182821402";
            var value = message;
            WebRequest webRequest;
            webRequest = WebRequest.Create("https://android.googleapis.com/gcm/send");
            webRequest.Method = "post";
            webRequest.ContentType = " application/x-www-form-urlencoded;charset=UTF-8";
            webRequest.Headers.Add(string.Format("Authorization: key={0}", GoogleAppID));

            webRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));

            var postData = "collapse_key=score_update&time_to_live=108&delay_while_idle=1&data.message=" + value + "&registration_id=" + deviceId + "";
            Console.WriteLine(postData);
            Byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            webRequest.ContentLength = byteArray.Length;

            Stream dataStream = webRequest.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse webResponse = webRequest.GetResponse();

            dataStream = webResponse.GetResponseStream();

            StreamReader tReader = new StreamReader(dataStream);

            String sResponseFromServer = tReader.ReadToEnd();


            tReader.Close();
            dataStream.Close();
            webResponse.Close();
        }

        public static void SendPush(string deviceToken, string message, int badge)
        {
            var push = new PushBroker();

            //Wire up the events for all the services that the broker registers
            push.OnNotificationSent += NotificationSent;
            push.OnChannelException += ChannelException;
            push.OnServiceException += ServiceException;
            push.OnNotificationFailed += NotificationFailed;
            push.OnDeviceSubscriptionExpired += DeviceSubscriptionExpired;
            push.OnDeviceSubscriptionChanged += DeviceSubscriptionChanged;
            push.OnChannelCreated += ChannelCreated;
            push.OnChannelDestroyed += ChannelDestroyed;

            var appleCert = System.IO.File.ReadAllBytes(System.Web.HttpContext.Current.Server.MapPath("/PushCert.Development.p12"));
            push.RegisterAppleService(new ApplePushChannelSettings(appleCert, "Abc@@123")); //Extension method
            push.QueueNotification(new AppleNotification()
                                       .ForDeviceToken(deviceToken)
                                       .WithAlert(message)
                                       .WithBadge(badge));
        }

        static void DeviceSubscriptionChanged(object sender, string oldSubscriptionId, string newSubscriptionId, INotification notification)
        {
            //Currently this event will only ever happen for Android GCM
            Console.WriteLine("Device Registration Changed:  Old-> " + oldSubscriptionId + "  New-> " + newSubscriptionId + " -> " + notification);
        }

        private static void NotificationSent(object sender, INotification notification)
        {
            Console.WriteLine("Sent: " + sender + " -> " + notification);
        }

        static void NotificationFailed(object sender, INotification notification, Exception notificationFailureException)
        {
            Console.WriteLine("Failure: " + sender + " -> " + notificationFailureException.Message + " -> " + notification);
        }

        static void ChannelException(object sender, IPushChannel channel, Exception exception)
        {
            Console.WriteLine("Channel Exception: " + sender + " -> " + exception);
        }

        static void ServiceException(object sender, Exception exception)
        {
            Console.WriteLine("Channel Exception: " + sender + " -> " + exception);
        }

        static void DeviceSubscriptionExpired(object sender, string expiredDeviceSubscriptionId, DateTime timestamp, INotification notification)
        {
            Console.WriteLine("Device Subscription Expired: " + sender + " -> " + expiredDeviceSubscriptionId);
        }

        static void ChannelDestroyed(object sender)
        {
            Console.WriteLine("Channel Destroyed for: " + sender);
        }

        static void ChannelCreated(object sender, IPushChannel pushChannel)
        {
            Console.WriteLine("Channel Created for: " + sender);
        }
    }
}