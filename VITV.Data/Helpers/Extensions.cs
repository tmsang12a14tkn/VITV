using System;
using System.Linq;
using System.Security.Principal;
using VITV.Data.DAL;
using VITV.Data.Models;

namespace VITV.Data.Helpers
{
    public static class Extensions
    {
        public static bool CanAccess(this IPrincipal principal, string actionName, string controllerName, string site)
        {
            var context = new VITVContext();
            var controllerAction = context.ControllerActions.FirstOrDefault(ca => ca.ActionName == actionName && ca.ControllerName == controllerName && ca.Site == site);
            if (controllerAction == null)
            {
                context.ControllerActions.Add(new ControllerAction
                {
                    Site = site,
                    ActionName = actionName,
                    ControllerName = controllerName
                });
                context.SaveChanges();
                return false;
            }

            var controllerActionPermission = context.ControllerActionPermissions.Where(p => p.ControllerActionId == controllerAction.Id).ToList().FirstOrDefault(p => principal.IsInRole(p.Role.Name));
            if (controllerActionPermission != null)
                return true;
            else
                return false;
        }
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

        public static Tuple<DateTime, DateTime> SetLastQuarterDates(int quarter, int year)
        {
            int startMonth = 0;
            int startDay = 1;
            int endMonth = 0;
            int endDay = 31;

            switch (quarter)
            {
                case 1:
                    startMonth = 1;
                    endMonth = 3;
                    endDay = 31;
                    break;

                case 2:
                    startMonth = 4;
                    endMonth = 6;
                    endDay = 30;
                    break;

                case 3:
                    startMonth = 7;
                    endMonth = 9;
                    endDay = 30;
                    break;

                case 4:
                    startMonth = 10;
                    endMonth = 12;
                    break;
            }

            DateTime startDate = new DateTime(year, startMonth, startDay);
            DateTime endDate = new DateTime(year, endMonth, endDay);
            return new Tuple<DateTime, DateTime>(startDate, endDate);
        }
    }
}