using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using VITV.Data.DAL;
using VITV.Data.Models;
using VITV.Data.Repositories;
using VITV.Data.ViewModels;

namespace VITV.Web.Controllers
{
    
    public class InterestRateController : Controller
    {
        private readonly VITVContext context;
        private readonly IBankRepository _bankRepository;
        private readonly ITermRepository _termRepository;
        private readonly IInterestRateRepository _interestRateRepository;

        // If you are using Dependency Injection, you can delete the following constructor
        public InterestRateController()
        {
            context = new VITVContext();
            _bankRepository = new BankRepository(context);
            _termRepository = new TermRepository(context);
            _interestRateRepository = new InterestRateRepository(context);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Management()
        {
            var model = GetIRList(TargetType.Personal);
            return View(model);
        }

        public ActionResult Index()
        {
            var model = GetIRList(TargetType.Personal);
            return View(model);
        }

        private InterestRateModel GetIRList(TargetType target)
        {
            var model = new InterestRateModel
            {
                TargetValue = target,
                Banks = _bankRepository.All.ToList(),
                Terms = _termRepository.All.OrderBy(t => t.MonthValue).ToList(),
                InterestRates = _interestRateRepository.All.Where(i => i.TargetValue == (int)target).OrderBy(i => i.Term.MonthValue).ToList()
            };

            return model;
        }

        public ViewResult IndexFilter(TargetType _target)
        {
            var model = GetIRList(_target);
            return View("~/Views/InterestRate/Index.cshtml", model);
        }

        public ViewResult Filter(TargetType TargetValue)
        {
            var model = GetIRList(TargetValue);
            return View("~/Views/InterestRate/Management.cshtml", model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult CreateOrEdit(string irId, string percentVal, TargetType targetVal)
        {
            var TargetValue = targetVal;
            var PercentValue = !string.IsNullOrEmpty(percentVal) ? double.Parse(percentVal.Replace(",", ".").Trim(), CultureInfo.InvariantCulture) : 0;

            if (!irId.StartsWith("0"))
            {
                var interestRate = _interestRateRepository.Find(Convert.ToInt32(irId));
                if (interestRate != null)
                {
                    interestRate.PercentValue = PercentValue;
                    _interestRateRepository.InsertOrUpdate(interestRate);
                    _interestRateRepository.Save();
                }
            }
            else
            {
                var datas = irId.Split('_');
                var interestRate = new InterestRate { 
                    BankId = Convert.ToInt32(datas[1]),
                    TermId = Convert.ToInt32(datas[2]),
                    TypeValue = Convert.ToInt32(datas[3]),
                    TargetValue = Convert.ToInt32(datas[4]),
                    PercentValue = PercentValue
                };
                _interestRateRepository.InsertOrUpdate(interestRate);
                _interestRateRepository.Save();
            }

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _interestRateRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}