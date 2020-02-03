using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VITV.HRM.Models;
using VITV.HRM.Models.View;
using Microsoft.AspNet.Identity;
using VITV.HRM.ViewModels;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using VITV.HRM.Helpers;

namespace VITV.HRM.Controllers
{
    public class EmployeeController : Controller
    {
        private ApplicationUserManager _userManager;
        private VITVSecondContext context;

        public EmployeeController()
        {
            context = new VITVSecondContext();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: Employee
        public ActionResult Index()
        {
            var model = new DepartmentArrangeView()
            {
                //DepartmentLocations = context.Groups.GroupBy(d => d.DepartmentLocation).Select(g => new DepartmentLocationView
                //    {
                //        LocationName = g.Key.Name,
                //        Roles = g.GroupBy(d => d.DepartmentRole).Select(gd => 
                //            new DepartmentRoleView { 
                //                RoleName = gd.Key.Name,
                //                Departments = gd.ToList()
                //            }).ToList()
                //    }).ToList(),
                Departments = context.Departments.ToList(),
                UnassignEmployees = context.Employees.Where(e => !e.WorkInfo.GroupId.HasValue).ToList()
            };
            return View(model);
        }

        public ActionResult List()
        {
            var employees = new VITVSecondContext().Employees.ToList();
            return View(employees);
        }

        public ActionResult Add()
        {
            
            ViewBag.PossibleRoles = context.Roles.OrderByDescending(r => r.Name).ToList();
            return View(new EmployeeModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(HttpPostedFileBase ProfilePicture, EmployeeModel model)
        {
            if (ProfilePicture != null && ProfilePicture.ContentLength > 0)
            {
                string fileName = Convert.ToInt32((DateTime.Now - new DateTime(2010, 01, 01)).TotalSeconds) + "_" + ProfilePicture.FileName;
                string path = System.IO.Path.Combine(Server.MapPath("~/Upload/Avatar"), fileName);
                ProfilePicture.SaveAs(path);
                model.ProfilePicture = "/Upload/Avatar/" + fileName;
            }

            ModelState.Clear();
            TryValidateModel(model);

            if (ModelState.IsValid)
            {
                
                if(context.Users.FirstOrDefault(u => u.UserName == model.UserName) != null)
                {
                    ViewBag.PossibleRoles = context.Roles.OrderByDescending(r => r.Name).ToList();
                    ModelState.AddModelError("", "Đã tồn tại người dùng này.");
                    return View(model);
                }
                var hasher = new PasswordHasher();

                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    PasswordHash = hasher.HashPassword(model.Password),
                    Email = model.Email,
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString()
                };


                user.Roles.Add(new IdentityUserRole { RoleId = model.Role, UserId = user.Id });

                context.Users.Add(user);
                await context.SaveChangesAsync();

                var employee = new Employee { 
                    Id = user.Id,
                    Name = model.Name,
                    ProfilePicture = model.ProfilePicture,
                    BirthDay = model.BirthDay,
                    Address = model.Address,
                    UniqueTitle = model.UniqueTitle,
                };

                context.Employees.Add(employee);

                var empWorkInfo = new EmployeeWorkInfo { 
                    EmployeeId = employee.Id,
                    StartDate = model.StartDate,
                };

                context.EmployeeWorkInfos.Add(empWorkInfo);

                context.SaveChanges();
                return RedirectToAction("List");
            }
            return View(model);
        }

        public ActionResult EditEmp(string id)
        {
            
            ViewBag.PossibleRoles = context.Roles.OrderByDescending(r => r.Name).ToList();
            var user = context.Users.Find(id);
            var emp = context.Employees.Find(id);
            var empWorkinfo = context.EmployeeWorkInfos.Find(id);
            var lstRoles = UserManager.GetRoles(user.Id);
            var role = lstRoles.Count > 0 ? lstRoles[0] : "";
            var model = new EmployeeEditModel
            { 
                Email = user.Email,
                Name = emp.Name,
                ProfilePicture = emp.ProfilePicture,
                BirthDay = emp.BirthDay,
                Address = emp.Address,
                StartDate = empWorkinfo.StartDate.Value,
                Role = role
            };
            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEmp(HttpPostedFileBase ProfilePicture, EmployeeEditModel model)
        {
            if (ProfilePicture != null && ProfilePicture.ContentLength > 0)
            {
                string fileName = Convert.ToInt32((DateTime.Now - new DateTime(2010, 01, 01)).TotalSeconds) + "_" + ProfilePicture.FileName;
                string path = System.IO.Path.Combine(Server.MapPath("~/Upload/Avatar"), fileName);
                ProfilePicture.SaveAs(path);
                model.ProfilePicture = "/Upload/Avatar/" + fileName;
            }

            ModelState.Clear();
            TryValidateModel(model);
            
            if (ModelState.IsValid)
            {
                var user = context.Users.Find(model.Id);
                user.Email = model.Email;

                var emp = context.Employees.Find(model.Id);
                emp.Name = model.Name;
                emp.BirthDay = model.BirthDay;
                emp.Address = model.Address;
                emp.ProfilePicture = model.ProfilePicture;

                var empWork = context.EmployeeWorkInfos.FirstOrDefault(e => e.EmployeeId == model.Id);
                empWork.StartDate = model.StartDate;

                if (UserManager.IsInRole(user.Id, model.Role))
                    UserManager.RemoveFromRole(user.Id, model.Role);
                UserManager.AddToRole(user.Id, model.Role);

                context.SaveChanges();

                return RedirectToAction("List");
            }
            ViewBag.PossibleRoles = context.Roles.OrderByDescending(r => r.Name).ToList();
            return View(model);
        }

        public ActionResult Delete(string id)
        {
            return PartialView(context.Employees.FirstOrDefault(u => u.Id == id));
        }

        [ValidateAntiForgeryToken]
        [HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(string id, string confirmText)
        {
            ApplicationUser user = context.Users.FirstOrDefault(u => u.Id == id);
            var success = false;
            string error = "";
            if (user == null)
            {
                error = "Không tìm thấy người dùng này";
            }
            else if (confirmText.ToLower() != "đồng ý")
            {
                error = "Chuỗi nhập vào chưa đúng";
            }
            else
            {
                context.Users.Attach(user);
                context.Entry(user).Collection(u => u.Roles).Load();
                var empWork = context.EmployeeWorkInfos.FirstOrDefault(e => e.EmployeeId == id);
                if (empWork != null)
                    context.EmployeeWorkInfos.Remove(empWork);

                var emp = context.Employees.Find(id);
                if(emp != null)
                    context.Employees.Remove(emp);

                var roles = UserManager.GetRoles(user.Id);

                if (UserManager.IsInRole(user.Id, roles.Count <= 0 ? "Mod" : roles[0]))
                    UserManager.RemoveFromRole(user.Id, roles[0]);

                context.Users.Remove(user);

                context.SaveChanges();
                success = true;
            }

            return Json(new { success = success, id = id, error = error });
        }

        public ActionResult Statistic(string Id)
        {
            
            var employee = context.Employees.Find(Id);
            if (employee == null)
                return new HttpNotFoundResult();
            //employee.PersonalJobs.Where(t => t.IsAbsent).Sum(t => (t.End - t.Start).TotalDays);

            return View();
        }

        public ActionResult Index2()
        {
            
            var model = new DepartmentArrangeView()
            {
                //DepartmentLocations = context.Groups.GroupBy(d => d.DepartmentLocation).Select(g => new DepartmentLocationView
                //    {
                //        LocationName = g.Key.Name,
                //        Roles = g.GroupBy(d => d.DepartmentRole).Select(gd => 
                //            new DepartmentRoleView { 
                //                RoleName = gd.Key.Name,
                //                Departments = gd.ToList()
                //            }).ToList()
                //    }).ToList(),
                Departments = context.Departments.ToList(),
                UnassignEmployees = context.Employees.Where(e => !e.WorkInfo.GroupId.HasValue).ToList()
            };
            return View(model);
        }

        public ActionResult ProfilePage(string id)
        {
            if (string.IsNullOrEmpty(id))
                id = User.Identity.GetUserId();

            var employee = context.Employees.Find(id);
            var model = new ProfilePageModel
            {
                Employee = employee,
                RegisterViewModel = new RegisterViewModel { UserId = id },
                Equipments = employee.Equipments
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(string id, string val, string valObj)
        {
            var model = context.Employees.Find(id);
            if (valObj == "name")
                model.Name = val;
            else if (valObj == "address")
                model.Address = val;
            else if (valObj == "birthday")
                model.BirthDay = Convert.ToDateTime(val);
            else if (valObj == "starteddate")
                model.WorkInfo.StartDate = DateTime.ParseExact(val, "dd/mm/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            else if (valObj == "endeddate")
            {
                DateTime endDate;
                if (DateTime.TryParseExact(val, "dd/mm/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out endDate))
                    model.WorkInfo.EndDate = endDate;
                else
                    model.WorkInfo.EndDate = null;
            }
            else if (valObj == "about")
                model.About = val;

            context.SaveChanges();

            return Content(null);
        }

        [HttpPost]
        public JsonResult ChangeGroup(string employeeId, int? groupId, int positionId)
        {

            var employee = context.Employees.Find(employeeId);
            if (employee != null)
            {
                employee.WorkInfo.GroupId = groupId;
                employee.WorkInfo.PositionLevelId = positionId;
                context.SaveChanges();
                return Json(new { result = true });
            }
            else
                return Json(new { result = false });
        }

        [HttpPost]
        public JsonResult UnarrangedGroup(string employeeId)
        {
            var employee = context.Employees.Find(employeeId);
            if (employee != null)
            {
                employee.WorkInfo.GroupId = null;
                employee.WorkInfo.PositionLevelId = null;
                context.SaveChanges();
                return Json(new { result = true });
            }
            else
                return Json(new { result = false });
        }

        public ActionResult EditPositionGroup(int id)
        {
            
            var group = context.Groups.Find(id);
            var existPositionLevelIds = group.PositionLevels.Select(pl => pl.Id).ToList();
            var lstPosition = context.PositionLevels.Where(pl => !existPositionLevelIds.Contains(pl.Id)).ToList();
            ViewBag.group = group;
            return PartialView(lstPosition);
        }

        [ValidateAntiForgeryToken]
        [HttpPost, ActionName("EditPositionGroup")]
        public JsonResult EditPositionGroupConfirmed(List<int> lstPosition, int groupid)
        {

            var group = context.Groups.Find(groupid);
            context.Groups.Attach(group);
            context.Entry(group).Collection(g => g.PositionLevels).Load();
            var lstPositionLevelView = new List<PositionLevelView>();

            if (lstPosition != null)
            {
                foreach (int position in lstPosition)
                {
                    if (group.PositionLevels.FirstOrDefault(g => g.Id == position) == null)
                    {
                        PositionLevel p = context.PositionLevels.Find(position);
                        lstPositionLevelView.Add(new PositionLevelView() { Id = p.Id, Name = p.Name });
                        group.PositionLevels.Add(p);
                    }
                }
            }
            context.SaveChanges();
            ViewBag.groupid = groupid;
            string strViewPosition = "";// this.RenderRazorViewToString("/Views/Employee/PositionView.cshtml", lstPositionLevelView);

            return Json(new { viewposition = strViewPosition });
        }

        [HttpPost]
        public ActionResult UploadAvatar(string id, HttpPostedFileBase file)
        {
            file.SaveAs(Server.MapPath("~/Upload/Avatar/" + file.FileName));
            var link = ("/Upload/Avatar/" + file.FileName);
            var context = new VITVSecondContext();
            var employee = context.Employees.Find(id);
            employee.ProfilePicture = link;
            context.SaveChanges();
            return Json(new { link = link });
        }

        public JsonResult GetList()
        {
            return Json(context.Employees.Select(e => new { id = e.Id, text = e.Name}).ToList(), JsonRequestBehavior.AllowGet);
        }
        
    }
}