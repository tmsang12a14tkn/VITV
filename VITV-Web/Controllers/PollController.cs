using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VITV.Data.Helpers;
using VITV.Data.Models;
using VITV.Data.Repositories;
using VITV.Web.Helpers;

namespace VITV_Web.Controllers
{
    public class PollController : Controller
    {
        private readonly IPollRepository _pollRepository;
        public PollController()
        {
            _pollRepository = new PollRepository();
        }
        // FRONTEND
        public ActionResult Question(int questionId)
        {
            //check if user is answered the poll
            var ip = Request.UserHostAddress;
            var userAnswered = _pollRepository.CheckUserAnswered(questionId, ip);
            
            if (userAnswered)
            {
                return Result(questionId);
            }
            else
            {
                var now = DateTime.Now;
                var question = _pollRepository.Find(questionId);
                if(!question.Done && question.FromDate <= now && (question.EndDate == null || question.EndDate >= now))
                    return PartialView("_Question", question);
                else
                    return Result(questionId);
            }
        }
        public ActionResult AdminQuestion(int questionId)
        {

            var question = _pollRepository.QuestionAnswers(questionId);
            return PartialView("_AdminQuestion", question);

        }

        public ActionResult AdminAnswer(int index)
        {
            ViewBag.index = index;
            return PartialView("_AdminAnswer", new PollAnswer());
        }

        public ActionResult Result(int questionId)
        {
            var ip = Request.UserHostAddress;
            var question = _pollRepository.QuestionAnswers(questionId);
            var userAnswered = _pollRepository.CheckUserAnswered(questionId, ip);
            ViewBag.useranswered = userAnswered;
            return PartialView("_Result", question);
        }
        public ActionResult VideoPolls(int videoId)
        {
            var questions = _pollRepository.VideoQuestions(videoId);
            return PartialView("_VideoPolls", questions);
        }
        public ActionResult AdminVideoPolls(int videoId)
        {
            var questions = _pollRepository.VideoQuestions(videoId);
            return PartialView("_AdminVideoPolls", questions);
        }

        [HttpPost]
        public ActionResult AnswerQuestion(int questionId, int[] orders)
        {
            var ip = Request.UserHostAddress;
            var userAnswered = _pollRepository.CheckUserAnswered(questionId, ip);
            var question = _pollRepository.QuestionAnswers(questionId);
            if (!userAnswered)
            {
                foreach (var order in orders)
                {
                    var userAnswer = _pollRepository.FindUserAnswer(questionId, order, ip);
                    if (userAnswer == null)
                    {
                        userAnswer = new PollUserAnswer
                        {
                            IP = ip,
                            QuestionId = questionId,
                            AnswerId = order
                        };
                        _pollRepository.InsertUserAnswer(userAnswer);
                    }
                }
                _pollRepository.Save();
                if (question.Published)
                    return Result(questionId);
                else
                    return Content("<b>Cảm ơn bãn đã bình chọn !</b>");
            }
            else
                if (question.Published)
                    return Result(questionId);
                else
                    return Content("<b>Cảm ơn bãn đã bình chọn !</b>");

        }
        //BACKEND
        public ActionResult Create(int videoId)
        {
            var model = new PollQuestion
            {
                VideoId = videoId,
                Published = true,
                Displayed = true,
                ViewTotal = true,

                FromDate = DateTime.Now.Date,

            };
            return PartialView("_Create", model);
        }
        public ActionResult Clone(int questionid)
        {
            var model = _pollRepository.Find(questionid);
            model.Id = 0;
            var answers = _pollRepository.GetAnswers(questionid).ToList();
            answers.ForEach(a =>  a.QuestionId = 0 );
            model.Answers = answers;
           
            return PartialView("_Create", model);
        }
        [HttpPost]
        public ActionResult Create(PollQuestion model, HttpPostedFileBase questionImage, List<HttpPostedFileBase> answerImages)
        {
            if (ModelState.IsValid)
            {
                if (questionImage != null && questionImage.ContentLength > 0)
                {
                    string fileName = questionImage.FileName;
                    using (new Impersonator("uploaduser", "", "Upload@@123"))
                    {
                        string currentDomain = System.Configuration.ConfigurationManager.AppSettings["CurrentDomain"];

                        string catFolder = "UploadedImages/ArticleCategory";
                        string filePath = System.Configuration.ConfigurationManager.AppSettings[currentDomain] + @"\" + catFolder + @"\" + fileName;
                        if (System.IO.File.Exists(filePath))
                        {
                            if (new FileInfo(filePath).Length != questionImage.ContentLength)
                            {
                                fileName = Guid.NewGuid().ToString() + questionImage.FileName;
                                questionImage.SaveAs(filePath);
                            }
                          
                        }
                        else
                        {
                            questionImage.SaveAs(filePath);
                        }
                        model.Image = currentDomain + "/" + catFolder + "/" + fileName;
                    }
                }
               
                if (model.Answers != null)
                { 
                    for (int i = model.Answers.Count -1 ; i >= 0; i--)
                    {
                        var answer = model.Answers.ElementAt(i);

                        if (string.IsNullOrEmpty(answer.Answer))
                        {
                            model.Answers.Remove(answer);
                            answerImages.RemoveAt(i);
                        }
                    }
                    for (int i = 0; i < model.Answers.Count; i++)
                    {
                        model.Answers.ElementAt(i).AnswerId = i + 1 ;
                    }
                }
                if (answerImages != null)
                {
                    for (int i = 0; i < model.Answers.Count; i++)
                    {
                        if (answerImages.ElementAt(i) != null)
                        {
                            var answerImage = answerImages.ElementAt(i);
                            string fileName = answerImage.FileName;
                            using (new Impersonator("uploaduser", "", "Upload@@123"))
                            {
                                string currentDomain = System.Configuration.ConfigurationManager.AppSettings["CurrentDomain"];

                                string catFolder = "UploadedImages/ArticleCategory";
                                string filePath = System.Configuration.ConfigurationManager.AppSettings[currentDomain] + @"\" + catFolder + @"\" + fileName;
                                if (System.IO.File.Exists(filePath))
                                {
                                    if (new FileInfo(filePath).Length != answerImage.ContentLength)
                                    {
                                        fileName = Guid.NewGuid().ToString() + answerImage.FileName;
                                        answerImage.SaveAs(filePath);
                                    }
                                    //else use old file
                                }
                                else
                                {
                                    answerImage.SaveAs(filePath);
                                }
                                model.Answers.ElementAt(i).Image = currentDomain + "/" + catFolder + "/" + fileName;
                            }
                        }
                    }
                }
                _pollRepository.InsertOrUpdate(model);
                _pollRepository.Save();
                var question = _pollRepository.QuestionAnswers(model.Id);
                string adminquestion = Utilities.RenderRazorViewToString(this, "~/Views/Poll/_AdminQuestion.cshtml", question);
                return Json(new { success = true, data = adminquestion });
            }
            else
                return Json(new { success = false, data = "Lỗi dữ liệu !" });
        }
        public ActionResult Edit(int questionid)
        {
            var question = _pollRepository.Find(questionid);
            return PartialView("_Edit", question);
        }
        [HttpPost]
        public ActionResult Edit(PollQuestion model, HttpPostedFileBase questionImage, List<HttpPostedFileBase> answerImages)
        {

            if (questionImage != null && questionImage.ContentLength > 0)
            {
                string fileName = questionImage.FileName;
                using (new Impersonator("uploaduser", "", "Upload@@123"))
                {
                    string currentDomain = System.Configuration.ConfigurationManager.AppSettings["CurrentDomain"];

                    string catFolder = "UploadedImages/ArticleCategory";
                    string filePath = System.Configuration.ConfigurationManager.AppSettings[currentDomain] + @"\" + catFolder + @"\" + fileName;
                    if (System.IO.File.Exists(filePath))
                    {
                        if (new FileInfo(filePath).Length != questionImage.ContentLength)
                        {
                            fileName = Guid.NewGuid().ToString() + questionImage.FileName;
                            questionImage.SaveAs(filePath);
                        }
                    }
                    else
                    {
                        questionImage.SaveAs(filePath);
                    }
                    model.Image = currentDomain + "/" + catFolder + "/" + fileName;
                }
            }
            var existAnswers = _pollRepository.GetAnswers(model.Id);

            if (model.Answers != null)
            {
                //remove answer which not containt content
                for (int i = model.Answers.Count - 1; i >= 0; i--)
                {
                    var answer = model.Answers.ElementAt(i);
                    
                    if (string.IsNullOrEmpty(answer.Answer))
                    {
                        model.Answers.Remove(answer);
                        answerImages.RemoveAt(i);
                        if(answer.AnswerId != 0) //exist answer
                        {
                            //remove user answers
                            _pollRepository.DeleleAnswer(answer.QuestionId, answer.AnswerId);
                        }
                    }
                }
                
                //set answer id for new answer
                for (int i = 0; i < model.Answers.Count; i++)
                {
                    if (model.Answers.ElementAt(i).AnswerId == 0)
                        model.Answers.ElementAt(i).AnswerId = model.Answers.Max(a => a.AnswerId) + 1;
                }
            }
            if (answerImages != null)
            {
                for(int i = 0;i< model.Answers.Count;i++)
                {
                    if(answerImages.ElementAt(i)!=null)
                    { 
                        var answerImage = answerImages.ElementAt(i);
                        string fileName = answerImage.FileName;
                        //string fileName = Convert.ToInt32((DateTime.Now - new DateTime(2010, 01, 01)).TotalSeconds).ToString() + "_" + profileFile.FileName;
                        using (new Impersonator("uploaduser", "", "Upload@@123"))
                        {
                            string currentDomain = System.Configuration.ConfigurationManager.AppSettings["CurrentDomain"];

                            string catFolder = "UploadedImages/ArticleCategory";
                            string filePath = System.Configuration.ConfigurationManager.AppSettings[currentDomain] + @"\" + catFolder + @"\" + fileName;
                            if (System.IO.File.Exists(filePath))
                            {
                                if (new FileInfo(filePath).Length != answerImage.ContentLength)
                                {
                                    fileName = Guid.NewGuid().ToString() + answerImage.FileName;
                                    answerImage.SaveAs(filePath);
                                }
                            }
                            else
                            {
                                answerImage.SaveAs(filePath);
                            }
                            model.Answers.ElementAt(i).Image = currentDomain + "/" + catFolder + "/" + fileName;
                        }
                    }
                }
            }

            _pollRepository.InsertOrUpdate(model);
            _pollRepository.Save();
            var question = _pollRepository.QuestionAnswers(model.Id);
            string adminquestion = Utilities.RenderRazorViewToString(this, "~/Views/Poll/_AdminQuestion.cshtml", question);
            return Json(new { success = true, data = adminquestion, questionid = model.Id });
        }

        [HttpPost]
        public ActionResult ChangeStatus(int id, bool isDone)
        {
            var question = _pollRepository.Find(id);
            if (question != null)
            {
                question.Done = isDone;
                _pollRepository.Save();
                return Json(new { success = true });
            }
            else
                return Json(new { success = false });
        }

        public ActionResult Delete(int questionid)
        {
            var question = _pollRepository.Find(questionid);
            return PartialView("_Delete", question);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int Id)
        {
            var question = _pollRepository.Find(Id);
            if (question != null)
            {
                _pollRepository.Delete(Id);
                _pollRepository.Save();
                return Json(new { success = true, questionid = Id });
            }
            else
                return Json(new { success = false, content = "Không tồn tại bình chọn này !" });
        }
    }
}