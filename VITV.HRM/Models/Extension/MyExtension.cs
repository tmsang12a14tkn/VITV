using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VITV.HRM.Models.Extension
{
    public static class MyExtension
    {
        public static string PassTimeString(this DateTime date)
        {
            TimeSpan deltaTime = DateTime.Now - date;
            string passTimeString;
            if (deltaTime.TotalSeconds < 60)
            {
                passTimeString = "gần đây";
            }
            else if (deltaTime.TotalMinutes < 60)
            {
                passTimeString = string.Format("{0} phút trước", Math.Floor(deltaTime.TotalMinutes));
            }
            else if (deltaTime.TotalHours < 24)
            {
                passTimeString = string.Format("{0} giờ trước", Math.Floor(deltaTime.TotalHours));
            }
            else if (deltaTime.TotalDays < 30)
            {
                passTimeString = string.Format("{0} ngày trước", Math.Floor(deltaTime.TotalDays));
            }
            else
            {
                passTimeString = date.ToString();
            }
            return passTimeString;
        }    
    }
}