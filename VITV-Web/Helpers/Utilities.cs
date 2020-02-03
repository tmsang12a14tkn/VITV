using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VITV.Web.Helpers
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
        public static double ToUnixTimestamp(DateTime target)
        {
            var date = new DateTime(1970, 1, 1, 0, 0, 0, target.Kind);
            var unixTimestamp = (target - date).TotalMilliseconds;
            return unixTimestamp;
        }

        public static DateTime GetFirstDateOfWeek(DateTime dayInWeek, DayOfWeek firstDay)
        {
            DateTime firstDayInWeek = dayInWeek.Date;
            while (firstDayInWeek.DayOfWeek != firstDay)
                firstDayInWeek = firstDayInWeek.AddDays(-1);

            return firstDayInWeek;
        }
        public static DateTime GetLastDateOfWeek(DateTime dayInWeek, DayOfWeek firstDay)
        {
            DateTime lastDayInWeek = dayInWeek.Date;
            while (lastDayInWeek.DayOfWeek != firstDay)
                lastDayInWeek = lastDayInWeek.AddDays(1);

            return lastDayInWeek;
        }
        public static DateTime GetFirstDateOfWeek(int year, int weekOfYear, DayOfWeek firstDay = DayOfWeek.Monday)
        {
            DateTime jan1 = new DateTime(year, 1, 1);

            DateTime firstDayInWeek = jan1.Date;
            while (firstDayInWeek.DayOfWeek != firstDay)
                firstDayInWeek = firstDayInWeek.AddDays(-1);

            return firstDayInWeek.AddDays(7 * weekOfYear);

        }

        public static DateTime GetLastDateOfWeek(int year, int weekOfYear, DayOfWeek lastDay = DayOfWeek.Sunday)
        {
            DateTime jan1 = new DateTime(year, 1, 1);

            DateTime lastDayInWeek = jan1.Date;
            while (lastDayInWeek.DayOfWeek != lastDay)
                lastDayInWeek = lastDayInWeek.AddDays(1);

            return lastDayInWeek.AddDays(7 * weekOfYear);

        }
    }
}