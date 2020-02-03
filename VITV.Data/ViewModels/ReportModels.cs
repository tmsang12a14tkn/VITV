using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VITV.Data.Models;

namespace VITV.Data.ViewModels
{
    public class VideoPeriodAccessReport
    {
        public DateTime Date { get; set; }
        public PeriodType PeriodType { get; set; }
        public int IPViewCount {get;set;}
        public int PageViewCount { get; set; }
        
    }
    public class PageAccessDate
    {
        public DateTime Date { get; set; }
        public int IPCount { get; set; }
        public int IPViewCount { get; set; }
        public int PageViewCount { get; set; }
        public int HomePageCount { get; set; }
    }

    public class PageViewerShip
    {
        public string Name { get; set; }
        public int Week { get; set; }
        public double IPViewCount { get; set; }
        public double AvgIPViewCount { get; set; }
        public double ChangeIPViewCount { get; set; }
        public double PercentIPViewCount { get; set; }
        public double PerAvgIPViewCount { get; set; }
        public double ChangeAvgIPViewCount { get; set; }
        public double PerChangeAvgIPViewCount { get; set; }
        public double PageViewCount { get; set; }
        public double AvgPageViewCount { get; set; }
        public double ChangePageViewCount { get; set; }
        public double PercentPageViewCount { get; set; }
        public double PerAvgPageViewCount { get; set; }
        public double ChangeAvgPageViewCount { get; set; }
        public double PerChangeAvgPageViewCount { get; set; }
        public double ReviewCount { get; set; }
        public double PercentReviewCount { get; set; }
        public int VideoCount { get; set; }
        public double VideoTime { get; set; }
    }

    public class PeriodVideoAccess
    {
        public int? ParentId { get; set; }
        public int IPViewCount { get; set; }
        public int PageViewCount { get; set; }
        public string Url { get; set; }
        public string Thumbnail {get;set;}
        public string Title { get; set; }
        public string VideoCatName { get; set; }
        public int? VideoCatId { get; set; }
        public int? TypeId { get; set; }
        public string DeviceType { get; set; }
        public string DeviceModel { get; set; }
        public string DeviceVendor { get; set; }
        public string OSName { get; set; }
        public string Type { get; set; } //Trực tiếp/Gián tiếp/Chính luận/Giải trí
        public Video Video { get; set; }
        public double PercentIPViewCount { get; set; }
        public double PercentPageViewCount { get; set; }
        public double PercentReViewCount { get; set; }
        public bool ViewDetail { get; set; }
    }

    public class VideoAccessReport
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Month { get; set; }
        public int Week { get; set; }
        public int IPViewCount{ get; set; }
        public int PageViewCount{ get; set; }
        public int ReviewCount { get; set; }
        public int VideoCatId { get; set; }
        public string VideoCatName { get; set; }
        public Page Page{ get; set; }
        public double PercentIPViewCount { get; set; }
        public double PercentPageViewCount { get; set; }
        public double PercentPageReviewCount { get; set; }

        public double IPViewChange { get; set; }
        public double PageViewChange { get; set; }

        public double PercentIPViewChange { get; set; }
        public double PercentPageViewChange { get; set; }


    }

    public class CatVideosReport
    {
        public List<VideoAccessReport> Videos { get; set; }
        public int TotalIPViewCount { get; set; }
        public int TotalPageViewCount { get; set; }
        public int TotalPageReviewCount { get; set; }
        public double AvgIPViewCount { get; set; }
        public double AvgPageViewCount { get; set; }

    }
    public enum PeriodType
    {
        Daily,
        Weekly,
        Monthly,
        Quarter,
        Year
    }
    public enum GroupByType
    {
        Video, 
        VideoCat
    }
    public class DeviceReport
    {
        public string Name { get; set; }
        public int IPViewCount { get; set; }
        public double PercentIPViewCount { get; set; }
        public int PageViewCount { get; set; }
        public double PercentPageViewCount { get; set; }
        public int DuplicateViewCount { get; set; }
        public double PercentDuplicateViewCount { get; set; }

    }
    public class VideoAccessDateView
    {
        public List<PeriodVideoAccess> AccessData { get; set; }
        public DateTime Date { get; set; }
        public PeriodType PeriodType { get; set; }
        public GroupByType GroupByType { get; set; }
    }

    public class CatWeekDate
    {
        public DayOfWeek DOW { get; set; }
        public int IPViewCount { get; set; }
        public int PageViewCount { get; set; }
        public double PercentIPViewCount { get; set; }
    }
    public class CatWeekChange
    {
        public DayOfWeek DOW { get; set; }
        public int IPViewCount { get; set; }
        //public int PageViewCount { get; set; }
        public int LastIPViewCount { get; set; }
        public int Change { get; set; }
        public double PercentDayChange { get; set; }
    }
    public class CatWeekAvgChange
    {
        public DayOfWeek DOW { get; set; }
        public int IPViewCount { get; set; }

        public double Change { get; set; }
        public double PercentDayChange { get; set; }
    }
   
    public class CatWeekDateReport
    {
        public int CatId { get; set; }
        public string CatName { get; set; }
        public IEnumerable<CatWeekDate> Data { get; set; }
    }

    public class CatWeekChangeReport
    {
        public int CatId { get; set; }
        public string CatName { get; set; }
        public int Total { get; set; }
        //public double AvgWeek { get; set; }
        
        //public double PercentWeek { get; set; }
        public int LastTotal { get; set; }
        public double PercentTotalChange { get; set; }

        public IEnumerable<CatWeekChange> Data { get; set; }
    }

    public class CatWeekAvgChangeReport
    {
        public int CatId { get; set; }
        public string CatName { get; set; }
        public int Total { get; set; }
        //public double AvgWeek { get; set; }
        public double TotalChange { get; set; }
        //public double PercentWeek { get; set; }
        public double PercentTotalChange { get; set; }

        public IEnumerable<CatWeekAvgChange> Data { get; set; }
    }

    public class CatWeek
    {
        public int Week { get; set; }
        public int IPViewCount { get; set; }
        public int PageViewCount { get; set; }
        public string WeekName { get; set; }
    }
    public class CatWeekReport
    {
        public int Id { get; set; }
        public string CatName { get;set;}
        public IEnumerable<CatWeek> Data { get; set; }
    }
    public class ParentCatWeekReport
    {
        public int Id { get; set; }
        public string CatName { get; set; }
        public List<string> Weeks { get; set; }
        public IEnumerable<CatWeekReport> Children { get; set; }
    }

    public class AllCatReport
    {
        public int? ParentId { get; set; }
        public int IPViewCount { get; set; }
        public int PageViewCount { get; set; }
        public string Title { get; set; }
        public int? VideoCatId { get; set; }
        public int? TypeId { get; set; }
        public string Type { get; set; } //Trực tiếp/Gián tiếp/Chính luận/Giải trí
        public double PercentIPViewCount { get; set; }
        public double PercentPageViewCount { get; set; }
        public int TotalVideos { get; set; }
        public int TotalDays { get; set; }
        public double TotalHourDuration { get; set; }
        public bool ViewDetail { get; set; }
        public int MinDayView { get; set; }
        public int MaxDayView { get; set; }
        public double Stability { get; set; }
        public int HighestVideoCount { get; set; }
        public int HighestVideoId { get; set; }
    }
}