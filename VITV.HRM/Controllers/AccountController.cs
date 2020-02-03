using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using VITV.HRM.Models;
using VITV.HRM.Models.View;

namespace VITV.HRM.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private IAuthenticationManager _authnManager;

        // Modified this from private to public and add the setter
        public IAuthenticationManager AuthenticationManager
        {
            get { return _authnManager ?? (_authnManager = HttpContext.GetOwinContext().Authentication); }
            set { _authnManager = value; }
        }

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
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

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                case SignInStatus.RequiresVerification:
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Đăng nhập không thành công.");
                    return View(model);
            }
        }

        [HttpPost]
        public ActionResult IsAuth(string un, string pw, string id, string dt)
        {
            var context = new VITVSecondContext();
            un = un.Trim();
            un = Regex.Replace(un, @"\s", "");
            ApplicationUser user = UserManager.FindByName(un);
           
            if (user != null)
            {
                bool checkPass = UserManager.CheckPassword(user, pw);

                if (checkPass)
                {
                    var loginToken = Guid.NewGuid().ToString();

                    var deviceUser = context.DeviceUsers.Find(user.Id, id, 0);
                    if (deviceUser == null)
                    {
                        deviceUser = new DeviceUser
                        {
                            DeviceId = id,
                        };
                        context.DeviceUsers.Add(deviceUser);
                    }
                    deviceUser.DeviceToken = dt;
                    deviceUser.UserId = user.Id;
                    deviceUser.LoginToken = loginToken;
                    deviceUser.ExpiredTime = DateTime.Now.AddDays(7);
                    deviceUser.Logged = true;

                    context.SaveChanges();
                    return Json(new { Success = true, Token = loginToken, Id = user.Id });
                }
                return Json(new { Success = false, Token = "" });
            }
            return Json(new { Success = false, Token = "" });
        }
        
        [HttpPost]
        [AllowAnonymous]
        public ActionResult CheckToken(CheckTokenModel model) //deviceId and tokenId
        {
            var context = new VITVSecondContext();
            DeviceUser deviceUser = context.DeviceUsers.FirstOrDefault(du => du.DeviceId == model.Id && du.LoginToken == model.Token);
            try
            {
                if (deviceUser != null)
                {
                    if (DateTime.Now < deviceUser.ExpiredTime)
                        return Json(new { Success = true });
                    else
                        return Json(new { Success = false, error = 1 });

                }
                else
                {
                    return Json(new { Success = false, error = 0 }, JsonRequestBehavior.AllowGet);
                }
            }
            catch(Exception e)
            {
                return Json(new { Success = false, error = e.Message });
            }
            
        }

        //[HttpPost]
        //public int CheckToken(string id, string token) //deviceId and tokenId
        //{
        //    var context = new VITVSecondContext();
        //    token = token.Trim();
        //    DeviceUser deviceUser = context.DeviceUsers.Where(du => du.DeviceId == id).FirstOrDefault();
        //    if (deviceUser != null || deviceUser.DeviceToken != token)
        //    {
        //        if (DateTime.Now < deviceUser.ExpiredTime)
        //            return 2;
        //        else
        //            return 1;
        //    }
        //    return 0;
        //}

        [HttpPost]
        public ActionResult MobileLogout(string id)
        {
            var context = new VITVSecondContext();
            DeviceUser deviceUser = context.DeviceUsers.Where(du => du.DeviceId == id).FirstOrDefault();
            if(deviceUser!=null)
            {
                deviceUser.Logged = false;
                context.SaveChanges();
                return Json(new { Success = true});
            }
            return Json(new { Success = false});

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(RegisterViewModel regModel)
        {
            ApplicationUser user = UserManager.FindById(regModel.UserId);
            user.PasswordHash = UserManager.PasswordHasher.HashPassword(regModel.Password);
            UserManager.UpdateSecurityStamp(regModel.UserId);
            IdentityResult rs = UserManager.Update(user);
            if (rs.Succeeded)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                return RedirectToAction("ProfilePage", "Employee");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

    }
}