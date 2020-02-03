using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using VITV.HRM.ChatModels;
using VITV.HRM.Helpers;
using VITV.HRM.Models;
using VITV.HRM.ViewModels;

namespace VITV.HRM.Controllers
{
    public class MessageController : Controller
    {
        private VITVSecondContext context;

        public MessageController()
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
        public ActionResult Get()
        {
            var listMessage = _GetListNotiMessage();
            ViewBag.count = listMessage.Where(m => !m.Own && !m.IsView).ToList().Count;
            return PartialView(listMessage);
        }

        public ActionResult GetListNotiMessage()
        {
            if (!User.Identity.IsAuthenticated)
                return Json(new { success = -1, content = Url.Action("Login", "Account") });
            var listMessage =  _GetListNotiMessage();
            if(listMessage != null)
            {
                string view = this.RenderRazorViewToString("~/Views/Message/GetListNotiMessage.cshtml",listMessage);
                return Json(new { success = 1, content = view, count = listMessage.Where(m => !m.Own && !m.IsView).ToList().Count });
            }
            return Json(new { success = 1, content = "Không có tin nhắn" });
        }

        public JsonResult SetAllMessageViewed(string receiver)
        {
            var sender = User.Identity.GetUserId();
            try
            {
                List<Message> lstMessage = context.Messages.Where(c => (c.SenderId == sender && c.ReceiverId == receiver || c.SenderId == receiver && c.ReceiverId == sender) && (c.IsView == false)).ToList();
                foreach (Message co in lstMessage)
                {
                    co.IsView = true;
                }
                context.SaveChanges();
                return Json(new { success = 1 });
            }catch(Exception e)
            {

            }
            
            return Json(new { success = 0 });
        }

        public JsonResult GetCountReceiveMessage()
        {
            var listMessage = _GetListNotiMessage();
            int count = listMessage.Where(m => !m.Own && !m.IsView).ToList().Count;
            return Json(new { count = count });
        }

        

        [Authorize]
        public PartialViewResult Get10Last(string userId)
        {
            var context = new VITVSecondContext();
            var myId = User.Identity.GetUserId();
            var messages = context.Messages.Where(m => (m.SenderId == myId && m.ReceiverId == userId) || (m.SenderId == userId && m.ReceiverId == myId)).OrderByDescending(m => m.CreatedTime).Take(10).ToList();
            messages.Reverse();
            ViewBag.MyId = myId;
            return PartialView("_Last10", messages);
        }

        
        public ActionResult GetChatWindow(string userId)
        {
            var context = new VITVSecondContext();
            var myId = User.Identity.GetUserId();
            var sender = context.Employees.Find(myId);
            var recv = context.Employees.Find(userId);
            var messages = context.Messages.Where(m => (m.SenderId == myId && m.ReceiverId == userId) || (m.SenderId == userId && m.ReceiverId == myId))
                .AsEnumerable().Select(m => new MessageModel
                {
                    Avatar = myId == m.SenderId ? sender.ProfilePicture : recv.ProfilePicture,
                    Name = myId == m.SenderId ? sender.Name : recv.Name,
                    CreatedTime = m.CreatedTime,
                    MessageContent = m.MessageContent,
                }).OrderByDescending(m => m.CreatedTime).Take(10).ToList();
            messages.Reverse();
            var sendEmp = context.Employees.Find(myId);
            ViewBag.MyId = myId;
            ViewBag.receiver = recv;
            ViewBag.sender = sendEmp;
            return PartialView("_ChatWindow", messages);
        }

        public ActionResult GetListUser()
        {
            var context = new VITVSecondContext();
            var chatClients = context.Employees.Select(u => new ChatClient() { UserId = u.Id, UserName = u.Name, DisplayName = u.Name, Avatar = u.ProfilePicture }).ToList();
            ViewBag.userlogin = User.Identity.GetUserId();
            return PartialView("_ListUser", chatClients);
        }

        [HttpPost]
        public ActionResult GetChatListWhenScrolling(string userid, DateTime createdatetime)
        {
            if (!User.Identity.IsAuthenticated)
                return Json(new { success = -1, content = Url.Action("Login", "User") });
            List<MessageModel> chats = _GetChatListWhenScrolling(userid, createdatetime, 10);
            string content = this.RenderRazorViewToString("_ChatList", chats);
            if (chats.Count == 0)
                return Json(new { success = 2, content = "" });
            return Json(new { success = 1, content = content, time = Utilities.URLFriendly(createdatetime.ToString()) });
        }



        private List<MessageModel> _GetChatListWhenScrolling(string userid, DateTime createdatetime, int noRecord)
        {
            var db = new VITVSecondContext();
            var myId = User.Identity.GetUserId();
            var sender = db.Employees.Find(myId);
            var recv = db.Employees.Find(userid);
            var sentMessages = db.Messages.Where(m => ((m.SenderId == myId && m.ReceiverId == userid) || (m.SenderId == userid && m.ReceiverId == myId))
                                                     && m.CreatedTime < createdatetime).AsEnumerable().Select(m => new MessageModel
                                                     {
                                                         Avatar = myId == m.SenderId ? sender.ProfilePicture : recv.ProfilePicture,
                                                         Name = myId == m.SenderId ? sender.Name : recv.Name,
                                                         CreatedTime = m.CreatedTime,
                                                         MessageContent = m.MessageContent,
                                                     }).OrderByDescending(m => m.CreatedTime).Take(noRecord).ToList();
            sentMessages.Reverse();
            return sentMessages;
        }
        //Own = false thi message nay la do nguoi khac nhan tin den
        private List<ListNotiChatModel> _GetListNotiMessage()
        {
            string curUserId = User.Identity.GetUserId();
            var db = new VITVSecondContext();
            var messages = db.Messages.ToList();
            List<ListNotiChatModel> myMessages = (from chatObj in db.Messages
                                                  where (chatObj.SenderId == curUserId || chatObj.ReceiverId == curUserId)
                                                  select new ListNotiChatModel
                                                  {
                                                      Avatar = (chatObj.SenderId == curUserId) ? chatObj.Receiver.ProfilePicture : chatObj.Sender.ProfilePicture,
                                                      Name = (chatObj.SenderId == curUserId) ? chatObj.Receiver.Name : chatObj.Sender.Name,
                                                      id = (chatObj.SenderId == curUserId) ? chatObj.ReceiverId : chatObj.SenderId,
                                                      Own = chatObj.SenderId == curUserId,
                                                      MessageContent = chatObj.MessageContent,
                                                      CreatedTime = chatObj.CreatedTime,
                                                      IsView = chatObj.IsView != null ? chatObj.IsView : false
                                                  }).ToList();
            List<ListNotiChatModel> messageGroups = (from myMes in myMessages
                                                     group myMes by new { refId = myMes.id } into mesGroup
                                                     select mesGroup.OrderByDescending(gr => gr.CreatedTime).FirstOrDefault()).Take(10).ToList();
            return messageGroups;
        }
    }
}