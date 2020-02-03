using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VITV.Data.Models;
using Microsoft.AspNet.Identity.Owin;
using VITV.Data.DAL;
using VITV.Data.ViewModels;

namespace VITV.Web.Controllers
{
    public class RoleController : Controller
    {
        private VITVContext context;
        private ApplicationUserManager _userManager;

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
        public RoleController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public RoleController()
        {
            context = new VITVContext();
        }


        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateRoleModel model)
        {
            if (ModelState.IsValid)
            {
                var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                // Create Admin Role
                IdentityResult roleResult;

                // Check to see if Role Exists, if not create it
                if (!RoleManager.RoleExists(model.Name))
                {
                    roleResult = RoleManager.Create(new IdentityRole(model.Name));
                    if (roleResult.Succeeded)
                        return RedirectToAction("Management");
                }
            }
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id)
        {
            IdentityRole role = context.Roles.Where(r => r.Id == id).FirstOrDefault();
            return View(AutoMapper.Mapper.Map<IdentityRole, CreateRoleModel>(role));
        }

        //
        // POST: /Reporter/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(CreateRoleModel model)
        {
            if (!String.IsNullOrEmpty(model.Name))
            {
                var role = AutoMapper.Mapper.Map<CreateRoleModel, IdentityRole>(model);
                context.Entry(role).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();

                return RedirectToAction("Management");
            }
            return View(model);
        }


        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id)
        {
            IdentityRole role = context.Roles.Where(r => r.Id == id).FirstOrDefault();
            return PartialView(AutoMapper.Mapper.Map<IdentityRole, CreateRoleModel>(role));
        }

        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(string id, string confirmText)
        {
            IdentityRole role = context.Roles.FirstOrDefault(u => u.Id == id);
            var success = false;
            string error = "";
            if (role == null)
            {
                error = "Không tìm thấy vai trò này";
            }
            else if (confirmText.ToLower() != "đồng ý")
            {
                error = "Chuỗi nhập vào chưa đúng";
            }
            else
            {
                var users = UserManager.Users.ToList();
                foreach(ApplicationUser u in users)
                {
                    if (UserManager.IsInRole(u.Id, role.Name))
                        UserManager.RemoveFromRole(u.Id, role.Name);
                }

                context.Roles.Remove(role);
                context.SaveChanges();
                success = true;
            }

            return Json(new { success = success, id = id, error = error });
        }

        public JsonResult GetRoles()
        {
            var Roles = context.Roles.AsEnumerable()
                                                .Select(spec => new
                                                {
                                                    Id = spec.Id,
                                                    Name = spec.Name,
                                                }).ToList();
            var total = Roles.Count;
            return Json(new { Roles, total }, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
                context = null;
            }
            base.Dispose(disposing);
        }
    }
}