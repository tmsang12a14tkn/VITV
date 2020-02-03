using System;
using System.Web.Mvc;
using VITV.Data.Models;
using VITV.Data.ViewModels;
using Microsoft.AspNet.Identity;
using VITV.Data.DAL;
using System.Linq;
using System.Collections.Generic;

namespace VITV_Web.Controllers
{
    public class ArticleCommentController : Controller
    {
        [HttpPost]
        public ActionResult Comment(CommentModel model)
        {
            var db = new VITVContext();
            var comment = new ArticleComment
            {
                ArticleId = model.ArticleId,
                Content = model.Content,
                UserId = User.Identity.GetUserId(),
                CreatedTime = DateTime.Now,
                Id = model.Id
            };
            db.ArticleComments.Add(comment);
            db.SaveChanges();
            return Json(new { success = true});
        }
        
        [HttpPost]
        public ActionResult AddComments(List<CommentModel> comments)
        {
            if (comments != null)
            {
                var db = new VITVContext();
                foreach (var cmt in comments)
                {
                    var comment = new ArticleComment
                    {
                        ArticleId = cmt.ArticleId,
                        Content = cmt.Content,
                        UserId = User.Identity.GetUserId(),
                        CreatedTime = DateTime.Now,
                        Id = cmt.Id
                    };
                    db.ArticleComments.Add(comment);
                }
                db.SaveChanges();
            }
            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult DeleteComments(List<string> comments)
        {
            if (comments != null)
            {
                var db = new VITVContext();
                foreach (var cmtId in comments)
                {
                    var comment = db.ArticleComments.Find(cmtId);
                    if (comment != null)
                    {
                        db.ArticleComments.Remove(comment);
                    }
                }
                db.SaveChanges();
            }
        
            return Json(new { success = true });
        }
        public ActionResult AddReplies(List<ReplyModel> replies)
        {
            if (replies != null)
            {
                var db = new VITVContext();
                foreach (var replyModel in replies)
                {
                    var reply = new ArticleCommentReply
                    {
                        ArticleCommentId = replyModel.CommentId,
                        Content = replyModel.Content,
                        UserId = User.Identity.GetUserId(),
                        CreatedTime = DateTime.Now
                    };
                    db.ArticleCommentReplies.Add(reply);
                }
                db.SaveChanges();
            }
            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult DeleteReplies(List<string> replies)
        {
            return Json(new { success = true });
        }
        public JsonResult GetList(int articleId)
        {
            var db = new VITVContext();
            var comments = db.ArticleComments.Where(cmt=>cmt.ArticleId == articleId)
                .Select(cmt => new
                {
                    Id = cmt.Id,
                    Content = cmt.Content,
                    UserName = cmt.User.Employee != null? cmt.User.Employee.Name: cmt.User.UserName,
                    Avatar = cmt.User.Employee!=null?cmt.User.Employee.ProfilePicture: "/images/default-avatar.png",
                    UserId = cmt.UserId,
                    Replies = cmt.Replies.Select(r => new {
                       Content = r.Content,
                       UserName = r.User.Employee != null ? r.User.Employee.Name : r.User.UserName,
                       Avatar = r.User.Employee != null ? r.User.Employee.ProfilePicture : "/images/default-avatar.png",
                        Id =  r.Id
                    })
                });
            return Json(comments, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Reply()
        {
            return View();
        }
    }
}