using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace VITV_Web.Helpers
{
    public static class Extensions
    {
        public static string DayOfWeekVN(this DateTime value)
        {
            int valInt = (int)value.DayOfWeek;
            if (valInt > 0)
                return string.Format("T {0}", valInt + 1);
            else
                return "CN";

        }

        public static string DayOfWeekVN2(this DateTime value)
        {
            int valInt = (int)value.DayOfWeek;
            if (valInt > 0)
                return string.Format("Thứ {0}", valInt + 1);
            else
                return "Chủ Nhật";

        }

        public static string DayOfWeekVN3(DayOfWeek dow)
        {
            int valInt = (int)dow;
            if (valInt > 0)
                return string.Format("Thứ {0}", valInt + 1);
            else
                return "Chủ Nhật";
        }
    }
}