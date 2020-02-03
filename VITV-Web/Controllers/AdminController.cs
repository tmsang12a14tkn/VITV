using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VITV.Data.DAL;
using VITV.Data.Helpers;
using VITV.Data.Models;
using VITV.Data.ViewModels;

namespace VITV.Data.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationUserManager _userManager;
        private VITVContext context;

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

        public AdminController()
        {
            context = new VITVContext();
        }
        public AdminController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }
        //
        // GET: /Admin/
        //[Authorize(Roles="Admin, Mod")]
        [RoleAuthorize(Site="admin")]
        public ActionResult Index()
        {
            //string currentDomain = System.Configuration.ConfigurationManager.AppSettings["CurrentDomain"];
            //string storagePath = System.Configuration.ConfigurationManager.AppSettings[currentDomain];
            
            var errorVideos = new List<Video>();
            using (new Impersonator("uploaduser", "", "Upload@@123"))
            {
                foreach (Video video in context.Videos.ToList())
                {
                    var path = GetPhysicalPath(video.Url);
                    if (!System.IO.File.Exists(path))
                    {
                        errorVideos.Add(video);
                    }
                }
            }
            var now = DateTime.Now.Date;
            bool hasSchedule = context.ProgramScheduleDetails.Any(p => DbFunctions.TruncateTime(p.DateTime) == now);
            var modNotification = context.Infoes.Find("modnotification");
            if(modNotification == null)
            {
                modNotification = new Info
                {
                    Id = "modnotification",
                    Title = "Thông báo tới quản lý",
                    Description = ""
                };
                context.Infoes.Add(modNotification);
                context.SaveChanges();
            }
            DashboardView viewModel = new DashboardView()
            {
                ReporterCount = context.Employees.Count(),
                VideoCount = context.Videos.Count(),
                //VideoStorage = GetDirectorySize(storagePath),
                ArticleCount = context.Articles.Count(),
                ErrorVideos = errorVideos,
                HasSchedule = hasSchedule,
                ModNotification = modNotification.Description,
                HLAlls = context.ArticleHighlightAlls.ToList(),
                HLCats = context.ArticleHighlightCats.ToList()
            };

            return View(viewModel);
            //return RedirectToAction("Management", "Video");
        }

        [Authorize(Roles="Admin")]
        public ActionResult Management()
        {
            //var context = new VITV.Data.DAL.VITVContext();
            //var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            //var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            //// Create Admin Role
            //string roleName = "Mod";
            //IdentityResult roleResult;

            //// Check to see if Role Exists, if not create it
            //if (!RoleManager.RoleExists(roleName))
            //{
            //    roleResult = RoleManager.Create(new IdentityRole(roleName));
            //}
            return View();
        }

        public ViewResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(model.Email, model.Password);
                
                if (user != null)
                {
                    if (user.Locked)
                    {
                        ModelState.AddModelError("", "Tài khoản đã bị khóa.");
                    }
                    else
                    {
                        await SignInAsync(user, model.RememberMe);
                        if (String.IsNullOrEmpty(returnUrl))
                            return RedirectToAction("Index", "Admin");
                        return RedirectToLocal(returnUrl);
                    }
                }
                else
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Admin");
        }

        [HttpGet, Authorize]
        public ActionResult DeleteTemp() //Xóa  tất cả file rác trong thư mục temp
        {
            return View();
        }

        [HttpPost, Authorize, ActionName("DeleteTemp")]
        public ActionResult DeleteTempConfirmed()
        {
            //var tempPath = Server.MapPath("~/UploadedVideos/Temp");
            //Array.ForEach(System.IO.Directory.GetFiles(tempPath), System.IO.File.Delete);
            return RedirectToAction("Index");
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await user.GenerateUserIdentityAsync(UserManager);
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, identity);
        }

        private IAuthenticationManager _authnManager;

        // Modified this from private to public and add the setter
        public IAuthenticationManager AuthenticationManager
        {
            get { return _authnManager ?? (_authnManager = HttpContext.GetOwinContext().Authentication); }
            set { _authnManager = value; }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult CreateUser()
        {
            ViewBag.PossibleRoles = context.Roles.OrderByDescending(r => r.Name).ToList();
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUser(CreateUserModel model)
        {
            if (ModelState.IsValid)
            {
                //var user = new ApplicationUser { UserName = model.UserName, Email = model.UserName };
                //var result = await UserManager.CreateAsync(user, model.Password);
                //var roleStore = new RoleStore<IdentityRole>(context);
                //var roleManager = new RoleManager<IdentityRole>(roleStore);

                //var userStore = new UserStore<ApplicationUser>(context);
                //var userManager = new UserManager<ApplicationUser>(userStore);
                //UserManager.AddToRole(user.Id, model.Role);

                if (!context.Users.Any(u => u.UserName == model.UserName))
                {
                    var hasher = new PasswordHasher();

                    var user = new ApplicationUser
                    {
                        UserName = model.UserName,
                        PasswordHash = hasher.HashPassword(model.Password),
                        Email = "",
                        EmailConfirmed = true,
                        SecurityStamp = Guid.NewGuid().ToString()
                    };

                    foreach (var roleid in model.Roles)
                    {
                        user.Roles.Add(new IdentityUserRole { RoleId = roleid, UserId = user.Id });
                    }

                    context.Users.Add(user);
                    context.SaveChanges();
                }
                return RedirectToAction("Management", "Admin");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id)
        {
            ApplicationUser user = context.Users.Where(r => r.Id == id).FirstOrDefault();
            var lstRoles = UserManager.GetRoles(user.Id);
            ViewBag.PossibleRoles = context.Roles.OrderByDescending(r => r.Name).ToList();
            if(user != null)
                return View(new EditUserModel { Id = user.Id, UserName = user.UserName, OldRoles = lstRoles.ToList(), Locked = user.Locked });
            return RedirectToAction("Management");
        }

        //
        // POST: /Reporter/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditUserModel model)
        {
            if(ModelState.IsValid)
            {
                // creating the UserStore object
                var userStore = new UserStore<ApplicationUser>(context);

                // ... and the userManager object
                var userManager = new UserManager<ApplicationUser>(userStore);

                ApplicationUser user = context.Users.Where(r => r.Id == model.Id).FirstOrDefault();
                user.Email = "";
                user.UserName = model.UserName;
                user.Locked = model.Locked;
                if (model.OldRoles != null)
                {
                    foreach (var name in model.OldRoles)
                    {
                        if (userManager.IsInRole(user.Id, name))
                            userManager.RemoveFromRole(user.Id, name);
                    }
                }
                if (model.Roles != null)
                {
                    foreach (var name in model.Roles)
                    {
                        userManager.AddToRole(user.Id, name);
                    }
                }
                userManager.Update(user);
                context.SaveChanges();
                return RedirectToAction("Management");
            }
            ViewBag.PossibleRoles = context.Roles.OrderByDescending(r => r.Name).ToList();
            return View(model);
        }

        //[Authorize(Roles = "Admin")]
        //public ActionResult CreateRole()
        //{
        //    return View();
        //}

        ////
        //// POST: /Reporter/Edit/5
        //[Authorize(Roles = "Admin")]
        //[HttpPost]
        //[ValidateInput(false)]
        //[ValidateAntiForgeryToken]
        //public ActionResult CreateRole(string namerole)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        ApplicationUser user = context.Users.Where(r => r.Id == model.Id).FirstOrDefault();
        //        user.Email = "";
        //        user.UserName = model.UserName;
        //        context.SaveChanges();
        //        if (UserManager.IsInRole(user.Id, model.Role))
        //            UserManager.RemoveFromRole(user.Id, model.Role);
        //        UserManager.AddToRole(user.Id, model.Role);
        //        context.SaveChanges();

        //        return RedirectToAction("Management");
        //    }
        //    return View(model);
        //}


        [Authorize(Roles="Admin")]
        public ActionResult Delete(string id)
        {
            return PartialView(context.Users.FirstOrDefault(u => u.Id == id));
        }

        [ValidateAntiForgeryToken]
        [Authorize(Roles="Admin")]
        [HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(string id, string confirmText)
        {
            ApplicationUser user = context.Users.FirstOrDefault(u => u.Id == id);
            context.Users.Attach(user);
            context.Entry(user).Collection(u => u.Roles).Load();
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
                var roles = UserManager.GetRoles(user.Id);
                if (roles.Contains("Admin"))
                {
                    error = "Không có quyền xóa tài khoản Admin";
                }
                else
                {
                    //remove login
                    var logins = user.Logins;
                    foreach (var login in logins.ToList())
                    {
                        UserManager.RemoveLogin(login.UserId, new UserLoginInfo(login.LoginProvider, login.ProviderKey));
                    }
                    //remove role
                    foreach (var role in roles)
                    {
                        UserManager.RemoveFromRole(user.Id, role);
                    }
                    //remove user
                    context.Users.Remove(user);
                    context.SaveChanges();
                    success = true;
                }
            }

            return Json(new { success = success, id = id, error = error });
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ResetPassword(string userId)
        {
            if (User.IsInRole("Admin") || User.Identity.GetUserId() == userId)
            {
                var model = new ResetPasswordModel { UserId = userId };
                return View(model);
            }

            return RedirectToAction("Management", "Admin");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {

                await UserManager.RemovePasswordAsync(model.UserId);

                var result = await UserManager.AddPasswordAsync(model.UserId, model.NewPassword);
                if (result.Succeeded)
                {
                    return RedirectToAction("Management", "Admin");
                }
                else
                {
                    foreach (string e in result.Errors)
                        ModelState.AddModelError("", e);
                }
            }
            return View(model);
        }

        public ActionResult ChangePassword(string userId)
        {
            if( User.IsInRole("Admin") || User.Identity.GetUserId() == userId)
            {
                var model = new ChangePasswordModel { UserId = userId };
                return View(model);
            }

            if (User.IsInRole("Admin"))
                return RedirectToAction("Management", "Admin");
            return RedirectToAction("Index", "Admin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                var result = await UserManager.ChangePasswordAsync(model.UserId, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    if (User.IsInRole("Admin"))
                        return RedirectToAction("Management", "Admin");
                    return RedirectToAction("Index", "Admin");
                }else
                {
                    foreach (string e in result.Errors)
                        ModelState.AddModelError("", e);
                }
            }
            return View(model);
        }

        [Authorize(Roles="Admin")]
        public JsonResult GetUsers()
        {
            var users = context.Users.Include(r => r.Roles).AsEnumerable()
                                                .Select(spec => new
                                                {
                                                    Id = spec.Id,
                                                    UserName = spec.UserName,
                                                    Email = spec.Email,
                                                    //Role = UserManager.GetRoles(spec.Id).Count > 0 ? UserManager.GetRoles(spec.Id)[0] : "",  
                                                    Role = GetStringRoles(spec.Id)
                                                }).ToList();
            var total = users.Count;
            return Json(new { users, total  }, JsonRequestBehavior.AllowGet);
        }


        [Authorize(Roles = "Admin")]
        public ActionResult PagePermission()
        {
            ViewBag.Sites = context.ControllerActions.GroupBy(ca => ca.Site).Select
            (
                g => new PagePermissionSiteModel
                {
                    Site = g.Key,
                    ControllerActions = g.OrderBy(ca=>ca.ControllerName).ToList()
                }
            ).ToList();

            ViewBag.Roles = context.Roles.ToList();
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult PagePermission(List<PagePermissionModel> pagePermissions)
        {
            foreach (var pagePermission in pagePermissions)
            {
                var controllerActionPermission = context.ControllerActionPermissions.Find(pagePermission.ControllerActionId, pagePermission.RoleId);

                if (pagePermission.CanAccess)
                {
                    if (controllerActionPermission == null)
                    {
                        //create
                        controllerActionPermission = new ControllerActionPermission
                        {
                            ControllerActionId = pagePermission.ControllerActionId,
                            RoleId = pagePermission.RoleId
                        };
                        context.ControllerActionPermissions.Add(controllerActionPermission);
                    }
                }
                else
                {
                    if(controllerActionPermission!=null)
                    {
                        //delete
                        context.ControllerActionPermissions.Remove(controllerActionPermission);
                    }
                }
            }
            context.SaveChanges();

            return RedirectToAction("PagePermission");
            //ViewBag.Roles = context.Roles.ToList();
            //ViewBag.ControllerActions = context.ControllerActions.ToList();


            //return View();
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Admin");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                context.Dispose();
                context = null;
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }


        private long GetDirectorySize(string p)
        {
            long b = 0;
            using (new Impersonator("uploaduser", "", "Upload@@123"))
            {
                string[] a = Directory.GetFiles(p, "*.*", SearchOption.AllDirectories);

                
                foreach (string name in a)
                {
                    FileInfo info = new FileInfo(name);
                    b += info.Length;
                }
            }
            
            return b/1048576;
        }
        private string GetPhysicalPath(string url)
        {
            Uri videoUri;
            bool isAbsoluteUrl = Uri.TryCreate(url, UriKind.Absolute, out videoUri);
            if (isAbsoluteUrl)
            {
                string currentDomain = videoUri.GetLeftPart(UriPartial.Authority);
                string filePath = System.Configuration.ConfigurationManager.AppSettings[currentDomain] + System.Net.WebUtility.UrlDecode(videoUri.AbsolutePath);
                filePath = filePath.Replace("/", "\\");
                return filePath;
            }
            else
                return "";
        }

        private string GetStringRoles(string userid)
        {
            List<string> lstRoles = UserManager.GetRoles(userid).ToList();
            string result = "";
            foreach (var role in lstRoles)
                result += role + ", ";
            return result.Trim().TrimEnd(',');
        }
	}
}