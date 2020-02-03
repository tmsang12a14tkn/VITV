using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VITV.Data.DAL;
using VITV.Data.Helpers;
using VITV.Data.Models;
using VITV.Data.Repositories;
using VITV.Data.ViewModels;

namespace VITV.Web.Controllers
{
    
    public class ReporterController : Controller
    {
        private readonly VITVContext context;
        private readonly IReporterRepository _reporterRepository;
        private readonly IPositionRepository _positionRepository;
        private readonly IPositionLevelRepository _positionLevelRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IVideoRepository _videoRepository;

        // If you are using Dependency Injection, you can delete the following constructor
        public ReporterController()
        {
            context = new VITVContext();
            _reporterRepository = new ReporterRepository(context);
            _positionRepository = new PositionRepository(context);
            _positionLevelRepository = new PositionLevelRepository(context);
            _roleRepository = new RoleRepository(context);
            _videoRepository = new VideoRepository(context);
        }

        public ReporterController(IReporterRepository reporterRepository, IPositionLevelRepository positionLevelRepository, IVideoRepository videoRepository)
        {
            context = new VITVContext();
            _reporterRepository = reporterRepository;
            _positionLevelRepository = positionLevelRepository;
            _videoRepository = videoRepository;
        }

        public ViewResult Index()
        {
            List<ReporterGroupView> lstReporterGroups = new List<ReporterGroupView>();
            foreach (var p in _positionLevelRepository.All)
            {
                var reporters = _reporterRepository.GetMany(r => r.Position.PositionLevelId == p.Id && r.IsShow == true).ToList();
                var rep = new ReporterGroupView
                {
                    Id = p.Id,
                    Name = p.Name,
                    Reporters = reporters
                };
                lstReporterGroups.Add(rep);
            }
            return View(lstReporterGroups);
            //return View(_reporterRepository.AllIncluding(reporter => reporter.Videos, reporter => reporter.Articles).ToList());
        }

        public PartialViewResult Search(string searchText)
        {
            List<ReporterGroupView> lstReporterGroups = new List<ReporterGroupView>();
            foreach (var p in _positionLevelRepository.All)
            {
                var reporters = _reporterRepository.GetMany(r => r.Position.PositionLevelId == p.Id && (r.Name.Contains(searchText) || r.Position.Name.Contains(searchText)) && r.IsShow == true).ToList();
                var rep = new ReporterGroupView
                {
                    Id = p.Id,
                    Name = p.Name,
                    Reporters = reporters
                };
                lstReporterGroups.Add(rep);
            }
            return PartialView("_ListSearch", lstReporterGroups);
        }

        public PartialViewResult Filter(string type)
        {
            List<Employee> reporters;
            if (type == "name")
            {
                reporters = _reporterRepository.All.Where(r => r.IsShow == true).OrderBy(r => r.Name).ToList();
            }
            else if (type == "position")
            {
                reporters = _reporterRepository.All.Where(r => r.IsShow == true).OrderBy(r => r.PositionId).ToList();
            }
            else
            {
                reporters = _reporterRepository.All.Where(r => r.IsShow == true).ToList();
            }
            return PartialView("_ListSearch", reporters);
        }
        //
        // GET: /Reporter/Details/5

        public ViewResult Details(string title)
        {
            return View(_reporterRepository.Find(title));
        }

        [RoleAuthorize(Site="admin")]
        public ViewResult Management(FilterTypeReporter filter = FilterTypeReporter.All, OrderTypeReporter order = OrderTypeReporter.PositionId, string name = "nguyen", string page = "1")
        {
            List<Employee> reporters = null;

            if (!string.IsNullOrEmpty(name))
            {
                reporters = _reporterRepository.SP_GetEmployees(name);
            }else
            {
                reporters = _reporterRepository.All;
            }

            if (filter == FilterTypeReporter.Active)
            {
                reporters = reporters.Where(r => r.IsShow).ToList();
            }
            else if (filter == FilterTypeReporter.Inactive)
            {
                reporters = reporters.Where(r => !r.IsShow).ToList();
            }

            if (order == OrderTypeReporter.Name)
            {
                reporters = reporters.OrderBy(r => r.Name).ToList();
            }
            else if (order == OrderTypeReporter.PositionId)
            {
                reporters = reporters.OrderBy(r => r.PositionId).ToList();
            }

            //dung cho version cu mau xanh la
            const int noResult = 10;
            var pageCount = (int)Math.Ceiling((double)reporters.Count() / noResult);
            int iPage;
            if (int.TryParse(page, out iPage))
            {
                reporters = reporters.Skip((iPage - 1) * noResult).Take(noResult).ToList();
            }


            var filterView = new ReporterFilterView
            {
                Page = iPage,
                PageCount = pageCount,
                Employees = reporters,
                Name = name,
                Filter = filter,
                Order = order
            };

            ViewBag.positions = _positionRepository.All.ToList();

            return View(filterView);
        }


        [RoleAuthorize(Site="admin")]
        public JsonResult GetReporters()
        {
            var reporters = _reporterRepository.AllIncluding(reporter => reporter.Videos, reporter => reporter.Articles)
                                                .Select(reporter => new
                                                {
                                                    ActiveNumber = reporter.IsShow ? 1: 0,
                                                    IsShow = reporter.IsShow,
                                                    Active = reporter.IsShow ? "emerald-bg" : "gray-bg",
                                                    Id = reporter.Id,
                                                    Name = reporter.Name,
                                                    VideoCount = reporter.Videos.Count,
                                                    ArticleCount = reporter.Articles.Count,
                                                    UniqueTitle = reporter.UniqueTitle,
                                                    Email = reporter.EmployeePersonalInfo.Email,
                                                    Phone = reporter.EmployeePersonalInfo.Phone,
                                                    Position = reporter.Position!= null? reporter.Position.Name:"Biên tập viên",
                                                    PositionId = reporter.PositionId != null ? reporter.PositionId : 100,
                                                    ProfilePicture = reporter.ProfilePicture
                                                }).OrderByDescending(r => r.ActiveNumber).ToList();
            var total = reporters.Count;
            return Json(new { reporters, total }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult Videos(int repId, int? month, string videocat = "all", string page = "1")
        {
            var reporter = _reporterRepository.Find(repId);
            List<Video> videosQuery = reporter.Videos.ToList();

            int catId;
            if (videocat != "all" && int.TryParse(videocat, out catId))
            {
                videosQuery = videosQuery.Where(v => v.CategoryId == catId).ToList();
            }

            if (month.HasValue && month > 0)
            {
                videosQuery = videosQuery.Where(v => v.DisplayTime.Month == month).ToList();
            }

            const int noResult = 8;
            var pageCount = (int)Math.Ceiling((double)videosQuery.Count() / noResult);
            int iPage;
            if (int.TryParse(page, out iPage))
            {
                videosQuery = videosQuery.Skip((iPage - 1) * noResult).Take(noResult).ToList();
            }

            var videos = videosQuery.OrderByDescending(v => v.DisplayTime).ToList();

            ViewBag.repId = repId;
            ViewBag.Page = iPage;
            ViewBag.PageCount = pageCount;

            return PartialView("_Videos", videos);
        }

        [RoleAuthorize(Site="admin")]
        public ActionResult Create()
        {
            ViewBag.PossiblePositions = context.Positions.ToList();
            ViewBag.PossibleRoles = _roleRepository.All.ToList();
            return View();
        }

        //
        // POST: /Reporter/Create
        [RoleAuthorize(Site="admin")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(HttpPostedFileBase profileFile, EmployeeModel reporter)
        {
            reporter.UniqueTitle = Helpers.UrlHelper.URLFriendly(reporter.Name);
            if (profileFile != null && profileFile.ContentLength > 0)
            {
                string fileName = Convert.ToInt32((DateTime.Now - new DateTime(2010, 01, 01)).TotalSeconds) + "_" + profileFile.FileName;
                string currentDomain = System.Configuration.ConfigurationManager.AppSettings["CurrentDomain"];
                string folder = "UploadedImages/profilepicture";
                string filePath = System.Configuration.ConfigurationManager.AppSettings[currentDomain] + @"\" + folder+ @"\"+fileName;

                using (new Impersonator("uploaduser", "", "Upload@@123"))
                {
                    profileFile.SaveAs(filePath);
                }
                reporter.ProfilePicture = currentDomain + "/" + folder + "/" + fileName;
            }
            else
            {
                reporter.ProfilePicture = "/Images/default-avatar.png";
            }
            ModelState.Clear();
            TryValidateModel(reporter);

            if (ModelState.IsValid)
            {
                var reporterEn = AutoMapper.Mapper.Map<EmployeeModel, Employee>(reporter);
                reporterEn.EmployeePersonalInfo = new EmployeePersonalInfo
                {
                    Biography = reporter.Biography,
                    Email = reporter.Email,
                    Introduction = reporter.Introduction,
                    Phone = reporter.Phone
                };
                _reporterRepository.InsertOrUpdate(reporterEn);
                _reporterRepository.Save();
                var reporterTemp = _reporterRepository.GetMany(r => r.Id == reporterEn.Id).FirstOrDefault();
                if (reporter.Roles != null)
                {
                    foreach (int i in reporter.Roles)
                    {
                        Role r = _roleRepository.Find(i);
                        reporterTemp.Roles.Add(r);
                    }
                }
                _roleRepository.Save();
                return RedirectToAction("Management");
            }
            return View(reporter);
        }

        //
        // GET: /Reporter/Edit/5
        [RoleAuthorize(Site="admin")]
        public ActionResult Edit(int id)
        {
            ViewBag.PossiblePositions = context.Positions.ToList();
            ViewBag.PossibleRoles = _roleRepository.All;
            var reporter = _reporterRepository.Find(id);
            var model = AutoMapper.Mapper.Map<Employee, EmployeeModel>(reporter);
          
            return View(model);
        }

        //
        // POST: /Reporter/Edit/5
        [RoleAuthorize(Site="admin")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(HttpPostedFileBase profileFile, EmployeeModel reporter)
        {
            reporter.UniqueTitle = VITV.Web.Helpers.UrlHelper.URLFriendly(reporter.Name);
            if (profileFile != null && profileFile.ContentLength > 0)
            {
                string fileName = Convert.ToInt32((DateTime.Now - new DateTime(2010, 01, 01)).TotalSeconds) + "_" + profileFile.FileName;
                string currentDomain = System.Configuration.ConfigurationManager.AppSettings["CurrentDomain"];
                string folder = "UploadedImages/profilepicture";
                string filePath = System.Configuration.ConfigurationManager.AppSettings[currentDomain] + @"\" + folder + @"\"+fileName;

                using (new Impersonator("uploaduser", "", "Upload@@123"))
                {
                    profileFile.SaveAs(filePath);
                }
                reporter.ProfilePicture = currentDomain + "/" + folder + "/" + fileName;
            }
            else if (String.IsNullOrEmpty(reporter.ProfilePicture))
            {
                reporter.ProfilePicture = "/Images/default-avatar.png";
            }

            ModelState.Clear();
            TryValidateModel(reporter);

            if (ModelState.IsValid)
            {
                var reporterEn = AutoMapper.Mapper.Map<EmployeeModel, Employee>(reporter);
                _reporterRepository.Load(reporterEn, a => a.Roles);
                if (reporter.Roles != null)
                {
                    List<Role> lstTemp = reporterEn.Roles.ToList();
                    foreach (Role r in lstTemp)
                        reporterEn.Roles.Remove(r);

                    foreach (int i in reporter.Roles)
                    {
                        Role r = _roleRepository.Find(i);
                        reporterEn.Roles.Add(r);
                    }
                }
                else
                {
                    List<Role> lstTemp = reporterEn.Roles.ToList();
                    foreach (Role r in lstTemp)
                    {
                        reporterEn.Roles.Remove(r);
                    }
                }
                var personalInfo = context.EmployeePersonalInfos.Find(reporter.Id);
                if(personalInfo == null)
                {
                    personalInfo = new EmployeePersonalInfo
                    {
                        Id = reporter.Id
                };
                }
                personalInfo.Biography = reporter.Biography;
                personalInfo.Email = reporter.Email;
                personalInfo.Introduction = reporter.Introduction;
                personalInfo.Phone = reporter.Phone;

                
                _reporterRepository.InsertOrUpdate(reporterEn);
                _reporterRepository.InsertOrUpdate(personalInfo);
                _reporterRepository.Save();
                return RedirectToAction("Management");
            }
            return View(reporter);
        }

        //
        // GET: /Reporter/Delete/5
        [RoleAuthorize(Site="admin")]
        public ActionResult Delete(int id)
        {
            return View(_reporterRepository.Find(id));
        }

        //
        // POST: /Reporter/Delete/5
        [RoleAuthorize(Site="admin")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var reporter = _reporterRepository.Find(id);
            if (reporter != null)
            {
                if (reporter.ProfilePicture != "/Images/default-avatar.png")
                {
                    using (new Impersonator("uploaduser", "", "Upload@@123"))
                    {
                        string filePath = Helpers.UrlHelper.GetPhysicalPath(this, reporter.ProfilePicture);
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                }
                _reporterRepository.Delete(id);
                _reporterRepository.Save();
            }


            return RedirectToAction("Management");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _reporterRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

