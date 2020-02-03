using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Mvc;
using VITV.Data.Helpers;
using VITV.Data.Models;
using VITV.Data.Repositories;

namespace VITV.Web.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactRepository _contactRepository;
        // GET: /Contact/

        public ContactController()
        {
            _contactRepository = new ContactRepository();
        }

        public ActionResult Index()
        {
            return View();
        }

        [RoleAuthorize(Site="admin")]
        public ActionResult Management()
        {
            ViewBag.ContactStatuses = _contactRepository.AllStatus.ToList();
            return View(_contactRepository.All.ToList());
        }

        [RoleAuthorize(Site = "admin")]
        // GET: /Contact/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = _contactRepository.Find(id.Value);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // GET: /Contact/Create
       

        // POST: /Contact/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index([Bind(Include="Id,Name,Email,Title,ContactContent")] Contact contact)
        {
            contact.CreatedDatetime = DateTime.Now;
            contact.StatusId = 2;
            if (ModelState.IsValid)
            {
                _contactRepository.InsertOrUpdate(contact);
                _contactRepository.Save();

                //SendMail(contact);

                return RedirectToAction("Index","Home");
            }

            return View(contact);
        }
        private async void SendMail(Contact contact)
        {
            var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
            var message = new MailMessage();
            message.To.Add(new MailAddress("eritnguyen.ad@gmail.com"));  // replace with valid value 
            message.From = new MailAddress("trungtien.tb@gmail.com");  // replace with valid value
            message.Subject = "Your email subject";
            message.Body = string.Format(body, contact.Name, contact.Email, contact.ContactContent);
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "trungtien.tb@gmail.com",  // replace with valid value
                    Password = "pw"  // replace with valid value
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);

            }
        }
      [RoleAuthorize(Site = "admin")]
        // GET: /Contact/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = _contactRepository.Find(id.Value);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        [RoleAuthorize(Site = "admin")]
        // POST: /Contact/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _contactRepository.Delete(id);
            _contactRepository.Save();
            return Json(new { success = true, id = id });
            //return RedirectToAction("Management");
        }

        [RoleAuthorize(Site = "admin")]
        [HttpPost]
        public ActionResult ChangeStatus(int id, int value)
        {
            _contactRepository.UpdateStatus(id, value);
            _contactRepository.Save();
            return Json(new { success= true});
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _contactRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
