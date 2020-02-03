using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VITV.HRM.Models;
using VITV.HRM.Models.View;
using Microsoft.AspNet.Identity;
using VITV.HRM.Helpers;
using VITV.HRM.ViewModels;

namespace VITV.HRM.Controllers
{
    public class PersonalDayoffController : Controller
    {
        
        // GET: PersonalDayoff/Create
        private List<DropDownListsModel> lstTimeOption = new List<DropDownListsModel>();

        public PersonalDayoffController()
        {
            lstTimeOption.Add(new DropDownListsModel { Text = "Cả ngày", Value = "all" });
            lstTimeOption.Add(new DropDownListsModel { Text = "Buổi sáng", Value = "morning" });
            lstTimeOption.Add(new DropDownListsModel { Text = "Buổi chiều", Value = "afternoon" });
            lstTimeOption.Add(new DropDownListsModel { Text = "Tùy chọn", Value = "custom" });
        }
        [Authorize]
        public ActionResult Create(DateTime date, string userid)
        {
            
            if (userid == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                //var date = DateTime.Parse(date,)
                var model = new PersonalDayoffModel()
                {
                    Date = date,
                    UserId = userid
                };
                ViewBag.lstTimeOption = lstTimeOption;
                return PartialView(model);
            }
            else
            {
                return Content("Bạn không có quyền!!!");
            }
        }

        [HttpPost]
        public ActionResult Create(PersonalDayoffModel dayoffModel)
        {
            var userId = User.Identity.GetUserId();
            if (dayoffModel.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                var context = new VITVSecondContext();
                if (ModelState.IsValid)
                {
                    PersonalDayoff dayoff = null;
                    if (dayoffModel.TimeOption == "all")
                    {
                        dayoff = new PersonalDayoff
                        {
                            AllDay = true,
                            Title = dayoffModel.Title,
                            Start = dayoffModel.Date.AddHours(9),
                            End = dayoffModel.Date.AddHours(18),
                            EmployeeId = dayoffModel.UserId
                        };
                    }
                    else if (dayoffModel.TimeOption == "morning")
                    {
                        dayoff = new PersonalDayoff
                        {
                            AllDay = false,
                            Title = dayoffModel.Title,
                            Start = dayoffModel.Date.AddHours(9),
                            End = dayoffModel.Date.AddHours(12),
                            EmployeeId = dayoffModel.UserId
                        };
                    }
                    else if (dayoffModel.TimeOption == "afternoon")
                    {
                        dayoff = new PersonalDayoff
                        {
                            AllDay = false,
                            Title = dayoffModel.Title,
                            Start = dayoffModel.Date.AddHours(14),
                            End = dayoffModel.Date.AddHours(18),
                            EmployeeId = dayoffModel.UserId
                        };
                    }
                    else if (dayoffModel.TimeOption == "custom" && dayoffModel.StartTime.HasValue && dayoffModel.EndTime.HasValue)//custom
                    {
                        dayoff = new PersonalDayoff
                        {
                            AllDay = false,
                            Title = dayoffModel.Title,
                            Start = dayoffModel.Date.AddHours(dayoffModel.StartTime.Value.Hour).AddMinutes(dayoffModel.StartTime.Value.Minute),
                            End = dayoffModel.Date.AddHours(dayoffModel.EndTime.Value.Hour).AddMinutes(dayoffModel.EndTime.Value.Minute),
                            EmployeeId = dayoffModel.UserId
                        };
                        
                    }
                    else
                    {
                        return Json(new { success = false, error = "Có lỗi..." });
                    }
                    
                    context.PersonalDayoffs.Add(dayoff);
                    context.SaveChanges();
                    //Utilities.SendDayoffPush(dayoff.Id);
                    //Utilities.SendDayoffPushAndroid(dayoff);
                    return Json(new { success = true });
                }
                return Json(new { success = false, error = "Chưa nhập đủ dữ liệu" });
            }
            else
                return Json(new { success = false, error = "Không có quyền!!!" });
        }

        [Authorize]
        // GET: PersonalDayoff/Create
        public ActionResult Edit(DateTime date, string userid, int dayoffid)
        {
            if (userid == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                var context = new VITVSecondContext();
                PersonalDayoff dayoff = context.PersonalDayoffs.FirstOrDefault(d => d.Id == dayoffid);
                var model = new PersonalDayoffModel() {
                    Date = date,
                    UserId = userid
                };
                if(dayoff != null)
                {
                    model.StartTime = dayoff.Start;
                    model.EndTime = dayoff.End;
                    model.Title = dayoff.Title;
                    model.Id = dayoff.Id;
                    if(dayoff.AllDay)
                    {
                        model.TimeOption = "all";
                    }else
                    {
                        if(dayoff.Start.Date.Hour >= 9 && dayoff.Start.Date.Hour <= 12)
                        {
                            model.TimeOption = "morning";
                        }else if(dayoff.Start.Date.Hour >= 14 && dayoff.Start.Date.Hour <= 18)
                        {
                            model.TimeOption = "afternoon";
                        }else
                        {
                            model.TimeOption = "custom";
                        }
                    }
                }
                ViewBag.lstTimeOption = lstTimeOption;
                return PartialView(model);
            }
            else
            {
                return Content("Bạn không có quyền!!!");
            }
        }

        [HttpPost]
        public ActionResult Edit(PersonalDayoffModel dayoffModel)
        {
            var userId = User.Identity.GetUserId();
            if (dayoffModel.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                var context = new VITVSecondContext();
                if (ModelState.IsValid)
                {
                    PersonalDayoff dayoff = context.PersonalDayoffs.Find(dayoffModel.Id);

                    if (dayoffModel.TimeOption == "all")
                    {
                        dayoff.AllDay = true;
                        dayoff.Title = dayoffModel.Title;
                        dayoff.Start = dayoffModel.Date.AddHours(9);
                        dayoff.End = dayoffModel.Date.AddHours(18);
                        dayoff.EmployeeId = dayoffModel.UserId;
                    }
                    else if (dayoffModel.TimeOption == "morning")
                    {
                        dayoff.AllDay = false;
                        dayoff.Title = dayoffModel.Title;
                        dayoff.Start = dayoffModel.Date.AddHours(9);
                        dayoff.End = dayoffModel.Date.AddHours(12);
                        dayoff.EmployeeId = dayoffModel.UserId;
                    }
                    else if (dayoffModel.TimeOption == "afternoon")
                    {
                        dayoff.AllDay = false;
                        dayoff.Title = dayoffModel.Title;
                        dayoff.Start = dayoffModel.Date.AddHours(14);
                        dayoff.End = dayoffModel.Date.AddHours(18);
                        dayoff.EmployeeId = dayoffModel.UserId;
                    }
                    else if (dayoffModel.TimeOption == "custom" && dayoffModel.StartTime.HasValue && dayoffModel.EndTime.HasValue)//custom
                    {
                        dayoff.AllDay = false;
                        dayoff.Title = dayoffModel.Title;
                        dayoff.Start = dayoffModel.Date.AddHours(dayoffModel.StartTime.Value.Hour).AddMinutes(dayoffModel.StartTime.Value.Minute);
                        dayoff.End = dayoffModel.Date.AddHours(dayoffModel.EndTime.Value.Hour).AddMinutes(dayoffModel.EndTime.Value.Minute);
                        dayoff.EmployeeId = dayoffModel.UserId;
                    }
                    else
                    {
                        return Json(new { success = false, error = "Có lỗi..." });
                    }
                    context.SaveChanges();
                    //Utilities.SendDayoffPush(dayoff);
                    //Utilities.SendDayoffPushAndroid(dayoff);
                    return Json(new { success = true });
                }
                return Json(new { success = false, error = "Chưa nhập đủ dữ liệu" });
            }
            else
                return Json(new { success = false, error = "Không có quyền!!!" });
        }

        public ActionResult Delete(int? id)
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var context = new VITVSecondContext();
            var dayoff = context.PersonalDayoffs.Find(id);
            if (dayoff != null)
            {
                context.PersonalDayoffs.Remove(dayoff);
                context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false});
        }
    }
}