using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VITV.Data.Models;

namespace VITV.Data.ViewModels
{
    public class ArticleCustomView
    {
        public int Id { get; set; }
        public string ArticleName { get; set; }
        public string SeriesName { get; set; }
        public DateTime? PublishedTime { get; set; }
        public string Thumbnail { get; set; }
        public string CatThumbnail { get; set; }
        public string CatName { get; set; }
        public string UniqueTitle { get; set; }
        public List<Employee> Reporters { get; set; }
        public int SeriesId { get; set; }
        public int ArticleStatus { get; set; }
        public bool IsPublished { get; set; }
        public bool IsHighlightAll { get; set; }
        public bool IsHighlightCat { get; set; }
        public int Order { get; set; }
        public int AllTimeCount { get; set; }
        public int FirstWeekCount { get; set; }

    }

    public class ArticleCustomDateView
    {
        public int Week { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<ArticleCustomView> Articles { get; set; }
    }

    public class ArticleFilterView
    {
        public RangeType Range { get; set; }
        public DateTime? End { get; set; }
        public DateTime? Begin { get; set; }
        public string Reporter { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public List<ArticleCustomDateView> CustomArticles { get; set; }
    }
}
