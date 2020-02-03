using System;
using System.Collections.Generic;
using System.Web.Http;
using VITV.Data.Repositories;
using VITV.Data.ViewModels;

namespace VITV.Portal.Controllers.api
{
    public class VideoAccessController : ApiController
    {
        private readonly IVideoRepository _videoRepository = new VideoRepository();

        //public IEnumerable<CatAccessDataChart> GetAllCatAccessDataGroupBy(string groupBy, DateTime startDate, DateTime endDate)
        //{
        //    IEnumerable<CatAccessDataChart> data = new List<CatAccessDataChart>();
        //    if (groupBy == "day")
        //        data = _videoRepository.GetAllCatAccessDataByDate(startDate, endDate);
        //    else if (groupBy == "week")
        //        data = _videoRepository.GetAllCatAccessDataByWeek(startDate, endDate);
        //    else if (groupBy == "month")
        //        data = _videoRepository.GetAllCatAccessDataByMonth(startDate, endDate);
        //    else if (groupBy == "quarter")
        //        data = _videoRepository.GetAllCatAccessDataByQuarter(startDate, endDate);
        //    else if (groupBy == "year")
        //        data = _videoRepository.GetAllCatAccessDataByYear(startDate, endDate);
        //    return data;
        //}
    }
}
