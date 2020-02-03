using AutoMapper;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Linq;
using VITV.Data.Models;
using VITV.Data.Models.StoreInfo;
using VITV.Data.ViewModels;
using VITV_Web.ViewModels;
namespace VITV.Web.Mappings
{
    public class DomainToViewModelMappingProfile: Profile
    {
        public override string ProfileName
        {
            get { return "DomainToViewModelMappings"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<Video, VideoModel>()
                .ForMember(v => v.ReporterIds, opt => opt.MapFrom(src => String.Join(", ", src.Reporters.Select(k => k.Id).ToList())))
                .ForMember(v => v.Keywords, opt => opt.MapFrom(src => String.Join(", ", src.Keywords.Select(k => k.Value).ToList())));
            Mapper.CreateMap<VideoTranscript, VideoTranscriptModel>()
                .ForMember(v => v.ReporterIds, opt => opt.MapFrom(src => String.Join(", ", src.Reporters.Select(k => k.Id).ToList())));
            Mapper.CreateMap<Article, ArticleModel>()
                .ForMember(v => v.ReporterIds, opt => opt.MapFrom(src => String.Join(", ", src.Reporters.Select(k => k.Id).ToList())))
                .ForMember(v => v.CategoryIds, opt => opt.MapFrom(src => String.Join(",", src.Categories.Select(k => k.Id).ToList())))
                .ForMember(v => v.Keywords, opt => opt.MapFrom(src => String.Join(", ", src.Keywords.Select(k => k.Value).ToList())));
            Mapper.CreateMap<Employee, EmployeeModel>()
                .ForMember(v => v.Roles, opt => opt.MapFrom(src => src.Roles.Select(r => r.Id).ToList()))
                .ForMember(v => v.Phone, opt => opt.MapFrom(src => src.EmployeePersonalInfo.Phone))
                .ForMember(v => v.Email, opt => opt.MapFrom(src => src.EmployeePersonalInfo.Email))
                .ForMember(v => v.Biography, opt => opt.MapFrom(src => src.EmployeePersonalInfo.Biography))
                .ForMember(v => v.Introduction, opt => opt.MapFrom(src => src.EmployeePersonalInfo.Introduction));

            Mapper.CreateMap<SpecialEvent, SpecialEventView>()
                .ForMember(x => x.Videos, opt => opt.MapFrom(s => s.Videos.OrderByDescending(v => v.PublishedTime).Take(12)));
            Mapper.CreateMap<SpecialEvent, SpecialEventModel>();
            Mapper.CreateMap<Holiday, HolidayModel>();
            Mapper.CreateMap<Bank, BankModel>();
            Mapper.CreateMap<Stock_Day, StockChartEntity>()
                 .ForMember(e => e.y, opt => opt.MapFrom(src => src.Close))
                 .ForMember(e => e.x, opt => opt.MapFrom(src => ToUnixTimestamp(src.Date)));
            Mapper.CreateMap<StockMarket_Day, StockChartEntity>()
                .ForMember(e => e.y, opt => opt.MapFrom(src => src.Close))
                .ForMember(e => e.x, opt => opt.MapFrom(src => ToUnixTimestamp(src.Date)));
            Mapper.CreateMap<IdentityRole, CreateRoleModel>();
            Mapper.CreateMap<IdentityUser, EditUserModel>();
            //Mapper.CreateMap<ICollection<Keyword>, string>()
            //    .ConvertUsing(source => KeywordListToString(source) ?? string.Empty);
            Mapper.CreateMap<ArticleRevision, RevisionView>()
                .ForMember(r => r.ChangedDate, opt => opt.MapFrom(src => src.ChangedDate.ToString("dd/MM/yyyy hh:mm:ss")))
                .ForMember(r=>r.UserName, opt=>opt.MapFrom(src=>src.CreateUser.UserName));

        }
        private double ToUnixTimestamp(DateTime target)
        {
            var date = new DateTime(1970, 1, 1, 0, 0, 0, target.Kind);
            var unixTimestamp = (target - date).TotalMilliseconds;
            return unixTimestamp;
        }

        //private string KeywordListToString(ICollection<Keyword> keywords)
        //{
        //    StringBuilder strBuilder = new StringBuilder();
        //    for (int i = 0;i< keywords.Count ;i++)
        //    {
        //        if (i == 0)
        //            strBuilder.Append(keywords.ElementAt(i).Value);
        //        else
        //            strBuilder.AppendFormat(",{0}", keywords.ElementAt(i).Value);
        //    }
        //    return strBuilder.ToString();
        //}
    }
}