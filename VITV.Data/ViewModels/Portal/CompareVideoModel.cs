using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VITV.Data.ViewModels.Portal
{
    public class VideoFirstWeekReport
    {
        public int PageView { get; set; }
        public double PercentPageView { get; set; }
        public int Highest { get; set; }
        public int Lowest { get; set; }
        public double AverageChange { get; set; }
        public int ViewDateCount { get; set; }
    }
    public class VideoReviewReport
    {
        public int PageView { get; set; }
        public double PercentPageView { get; set; }
        public int ViewDateCount { get; set; }
    }
    public class VideoAllTimeReport
    {
        public int PageView { get; set; }
        public double PercentPageView { get; set; }
        public int Highest { get; set; }
        public int Lowest { get; set; }
        public double AverageChange { get; set; }
        public int ViewDateCount { get; set; }
    }
    public class CompareVideoItemModel
    {
        public int VideoId { get; set; }
        public string Title { get; set; }
        public string CategoryName { get; set; }
        public string CategoryTypeName { get; set; }
        public DateTime DisplayTime { get; set; }
        public VideoFirstWeekReport FirstWeek { get; set; }

        public VideoAllTimeReport AllTime { get; set; }

        public VideoReviewReport ReviewReport { get; set; }

        public int VideoPeriod { get; set; }
        public string VideoDuration { get; set; }

        public int Month { get; set; }
        public int Week { get; set; }

    }
    public class CompareVideoModel
    {
        public string CategoryTypeName { get; set; }
        public string CategoryName { get; set; }
        public int VideoCount { get; set; }
        public int FirstWeekHighestVideoId { get; set; }
        public int FirstWeekLowestVideoId { get; set; }
        public int AllTimeHighestVideoId { get; set; }
        public int AllTimeLowestVideoId { get; set; }
        public int CurrentId { get; set; }
        public int FirstWeekView { get; set; }
        public int AllTimeView { get; set; }
        public double AllTimeAverage {get;set;}
        public double AllTimeAverageHighest { get; set; }
        public double FirstWeekAverageHighest { get; set; }
        public double FirstWeekAverage { get; set; }
        public double Review { get; set; }
        public double FirstWeekMedian { get; set; }
        public double AllTimeMedian { get; set; }
        public int FirstWeekHighest { get; set; }
        public int AllTimeHighest { get; set; }
        public int FirstWeekLowest { get; set; }
        public int AllTimeLowest { get; set; }
        public List<CompareVideoItemModel> Videos { get; set; }
    }

    public class CompareVideoSameWeekModel
    {
        public int CurrentId { get; set; }
        public int FirstWeekView { get; set; }
        public int AllTimeView { get; set; }
        public double AllTimeAverage { get; set; }
        public double FirstWeekAverage { get; set; }
        public List<CompareVideoItemModel> Videos { get; set; }
    }


}
