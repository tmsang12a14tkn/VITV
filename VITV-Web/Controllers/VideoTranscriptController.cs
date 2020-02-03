using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VITV.Data.Helpers;
using VITV.Data.Models;
using VITV.Data.Repositories;
using VITV.Data.ViewModels;
using VITV.Data.DAL;
using VITV.Web.Helpers;
using Microsoft.AspNet.Identity;

namespace VITV_Web.Controllers
{
    public class VideoTranscriptController : Controller
    {
        private readonly IVideoRepository _videoRepository;
        private readonly IVideoTranscriptRepository _videoTranscriptRepository;
        private readonly IReporterRepository _reporterRepository;

        public VideoTranscriptController()
        {
            var context = new VITVContext();
            _videoRepository = new VideoRepository(context);
            _videoTranscriptRepository = new VideoTranscriptRepository(context);
            _reporterRepository = new ReporterRepository(context);
        }

        public VideoTranscriptController(IVideoRepository videoRepository, IReporterRepository reporterRepository, IVideoTranscriptRepository videoTranscriptRepository)
        {
            _videoRepository = videoRepository;
            _videoTranscriptRepository = videoTranscriptRepository;
            _reporterRepository = reporterRepository;
        }

        public ActionResult GetById(int vId)
        {
            var video = _videoRepository.Find(vId);
            var transcripts = _videoTranscriptRepository.GetMany(vt => vt.VideoId == vId).ToList();
            ViewBag.hasContent = !string.IsNullOrEmpty(video.Content);
            return PartialView("_ListFull", transcripts);
        }
        
        [Authorize(Roles = "Admin, Mod")]
        public ActionResult Create(int vId, double? t)
        {
            double hours = 0;
            double minutes = 0;
            double seconds = 0;

            if (t.HasValue)
            {
                hours = Math.Floor(Convert.ToDouble(t / 60 / 60));
                minutes = Math.Floor(Convert.ToDouble((t - (hours * 60 * 60)) / 60));
                seconds = Math.Floor(Convert.ToDouble(t - (hours * 60 * 60) - (minutes * 60)));
            }

            var model = new VideoTranscriptModel
            {
                UserEditedId = User.Identity.GetUserId(),
                VideoId = vId,
                HourSeek = Convert.ToInt32(hours),
                MinuteSeek = Convert.ToInt32(minutes),
                SecondSeek = Convert.ToInt32(seconds)
            };

            ViewBag.Reporters = _reporterRepository.All.ToList();
            return PartialView("_CreateForm", model);
        }

        [Authorize(Roles = "Admin, Mod")]
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(VideoTranscriptModel model)
        {
            if (ModelState.IsValid)
            {
                var videoTranscript = AutoMapper.Mapper.Map<VideoTranscriptModel, VideoTranscript>(model);

                if (!string.IsNullOrWhiteSpace(model.ReporterIds))
                {
                    var reporters = model.ReporterIds.Split(',');
                    foreach (string rpId in reporters)
                    {
                        if (!string.IsNullOrWhiteSpace(rpId))
                        {
                            var rpIdNormalize = rpId.Trim();
                            var reporter = _reporterRepository.Find(Convert.ToInt32(rpIdNormalize));
                            if (reporter != null)
                                videoTranscript.Reporters.Add(reporter);
                        }
                    }
                }
                videoTranscript.ConvertedToSeconds = model.HourSeek * 60 * 60 + model.MinuteSeek * 60 + model.SecondSeek;

                _videoTranscriptRepository.InsertOrUpdate(videoTranscript);
                _videoTranscriptRepository.Save();
            }

            var transcripts = _videoTranscriptRepository.GetMany(vt => vt.VideoId == model.VideoId).ToList();
            var transcriptModels = AutoMapper.Mapper.Map<List<VideoTranscript>, List<VideoTranscriptModel>>(transcripts);
            return PartialView("_ListTranscripts", transcriptModels);
        }

        [Authorize(Roles = "Admin, Mod")]
        public ActionResult Edit(int id)
        {
            var transcript = _videoTranscriptRepository.Find(id);
            var model = AutoMapper.Mapper.Map<VideoTranscript, VideoTranscriptModel>(transcript);
            ViewBag.Reporters = _reporterRepository.All.ToList();
            return PartialView("_EditForm", model);
        }

        [Authorize(Roles = "Admin, Mod")]
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(VideoTranscriptModel model)
        {
            if (ModelState.IsValid)
            {
                var transcript = AutoMapper.Mapper.Map<VideoTranscriptModel, VideoTranscript>(model);

                _videoTranscriptRepository.Load(transcript, v => v.Reporters);
                transcript.Reporters.Clear();

                if (!string.IsNullOrWhiteSpace(model.ReporterIds))
                {
                    var reporters = model.ReporterIds.Split(',');
                    foreach (string rpId in reporters)
                    {
                        if (!string.IsNullOrWhiteSpace(rpId))
                        {
                            var rpIdNormalize = rpId.Trim();
                            var reporter = _reporterRepository.Find(Convert.ToInt32(rpIdNormalize));
                            if (reporter != null)
                                transcript.Reporters.Add(reporter);
                        }
                    }
                }
                transcript.ConvertedToSeconds = model.HourSeek * 60 * 60 + model.MinuteSeek * 60 + model.SecondSeek;

                _videoTranscriptRepository.InsertOrUpdate(transcript);
                _videoRepository.Save();

                var transcripts = _videoTranscriptRepository.GetMany(vt => vt.VideoId == model.VideoId).ToList();
                var transcriptModels = AutoMapper.Mapper.Map<List<VideoTranscript>, List<VideoTranscriptModel>>(transcripts);
                return PartialView("_ListTranscripts", transcriptModels);
            }

            ViewBag.Reporters = _reporterRepository.All;
            return PartialView("_EditForm", model);
        }

        [Authorize(Roles = "Admin, Mod")]
        public ActionResult Details(int vId)
        {
            var video = _videoRepository.Find(vId);
            if (video == null)
                return new HttpNotFoundResult("Không tìm thấy video");

            var videoTranscripts = _videoTranscriptRepository.GetMany(vt => vt.VideoId == video.Id).ToList();
            var model = new VideoTranscriptView
            {
                Video = video,
                VideoTranscriptModels = AutoMapper.Mapper.Map<List<VideoTranscript>, List<VideoTranscriptModel>>(videoTranscripts)
            };

            return View(model);
        }

        [Authorize(Roles = "Admin, Mod")]
        public ActionResult Delete(int id)
        {
            return PartialView("_Delete", _videoTranscriptRepository.Find(id));
        }

        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Mod")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var transcript = _videoTranscriptRepository.Find(id);
            _videoTranscriptRepository.Delete(id);
            _videoTranscriptRepository.Save();

            var transcripts = _videoTranscriptRepository.GetMany(vt => vt.VideoId == transcript.VideoId).ToList();
            var transcriptModels = AutoMapper.Mapper.Map<List<VideoTranscript>, List<VideoTranscriptModel>>(transcripts);

            return PartialView("_ListTranscripts", transcriptModels);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _videoTranscriptRepository.Dispose();
                _videoRepository.Dispose();
                _reporterRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}