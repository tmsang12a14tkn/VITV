using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VITV.Data.Repositories;
using VITV.WebApi.Entities;

namespace VITV.WebApi.Controllers
{
    public class ScheduleController : ApiController
    {
        private readonly IProgramScheduleDetailRepository _scheduleDetailRepository;
        private readonly IProgramScheduleRepository _scheduleRepository;
        public ScheduleController()
        {
            //var context = new VITVContext();
            _scheduleDetailRepository = new ProgramScheduleDetailRepository();
            _scheduleRepository = new ProgramScheduleRepository();
        }
        [HttpGet]
        public List<ProgramScheduleDetailModel> GetBroadcastScheduleByDate(string date)
        {

            var dateTime = DateTime.ParseExact(date, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);

            List<ProgramScheduleDetailModel> scheduleDetails =
               _scheduleDetailRepository.GetMany(p => DbFunctions.TruncateTime(p.DateTime) == dateTime,
                   p => p.DateTime).
                   Select(s => new ProgramScheduleDetailModel
                   {
                       DateTime = s.DateTime,
                       //IsNew = s.IsNew,
                       Name = string.IsNullOrEmpty(s.Name) ? "" : s.Name,
                       VideoCategory = s.VideoCategory.Name,
                       VideoId = s.VideoId == null ? 0 : s.VideoId
                   }).ToList();
            if (scheduleDetails.Count == 0)
            {
                var dayOfWeek = dateTime.DayOfWeek;

                scheduleDetails = _scheduleRepository.GetMany(s => s.DayOfWeek == dayOfWeek, s => s.Time)
                    .Select(s => new ProgramScheduleDetailModel
                    {
                        VideoCategory = s.VideoCategory.Name,
                        DateTime = dateTime.Date.Add(s.Time),
                        //IsNew = s.IsNew,
                        Name = "",
                        VideoId = 0
                    }).ToList();

            }
            return scheduleDetails;
        }
    }
}
