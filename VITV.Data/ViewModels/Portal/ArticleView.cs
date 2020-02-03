namespace VITV.Data.ViewModels.Portal
{
    public class ArticleFirstWeekReport
    {
        public int PageView { get; set; }
        public double PercentPageView { get; set; }
        public int Highest { get; set; }
        public int Lowest { get; set; }
        public double AverageChange { get; set; }
        public int ViewDateCount { get; set; }
    }

    public class ArticleAllTimeReport
    {
        public int PageView { get; set; }
        public double PercentPageView { get; set; }
        public int Highest { get; set; }
        public int Lowest { get; set; }
        public double AverageChange { get; set; }
        public int ViewDateCount { get; set; }
    }
}
