using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VITV.Data.Models;

namespace VITV.Data.ViewModels
{
    public enum RangeType: int
    {
        All = 1,
        Day = 2,
        Week = 3,
        OneMonth = 4,
        ThreeMonth = 5,
        SixMonth = 6,
        Custom = 7
    }

    public enum FilterTypeReporter : int
    {
        All = 0,
        Active = 1,
        Inactive = 2,
    }

    public enum OrderTypeReporter : int
    {
        Name = 0,
        PositionId = 1,
    }

    public class VideoFilterView
    {
        public string Category { get; set; }
        //public string Type { get; set; }
        public RangeType Range { get; set; }
        public string Reporter { get; set; }
        public DateTime DateSelected { get; set; }
        public int PageCount { get; set; }
        public int Page { get; set; }
        public int? EventId { get; set; }
        public string Title { get; set; }
        public DateTime? End { get; set; }
        public DateTime? Begin { get; set; }
        public DateTime? Lastdate { get; set; }
        public List<Video> Videos { get; set; }
        public List<VideoTabFilterView> VideosTab { get; set; }
        
    }

    public class ReporterFilterView
    {
        public FilterTypeReporter Filter { get; set; }
        public OrderTypeReporter Order { get; set; }
        public int PageCount { get; set; }
        public int Page { get; set; }
        public string Name { get; set; }
        public List<Employee> Employees { get; set; }

    }

}