using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using VITV.Data.Repositories;
using VITV.DataService.Contract;
using VITV.DataService.Entity;

namespace VITV.DataService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ScheduleService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ScheduleService.svc or ScheduleService.svc.cs at the Solution Explorer and start debugging.
    public class ScheduleService : IScheduleService
    {
        private readonly IProgramScheduleDetailRepository _scheduleDetailRepository;
        private readonly IProgramScheduleRepository _scheduleRepository;
        public ScheduleService()
        {
            //var context = new VITVContext();
            _scheduleDetailRepository = new ProgramScheduleDetailRepository();
            _scheduleRepository = new ProgramScheduleRepository();
        }
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
