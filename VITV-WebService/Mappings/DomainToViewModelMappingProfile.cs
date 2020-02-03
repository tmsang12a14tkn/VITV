using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using VITV.Data.Models;
using VITV.Data.ViewModels;
using VITV.DataService.Entity;
namespace VITV.DataService.Mappings
{
    public class DomainToViewModelMappingProfile: Profile
    {
        public override string ProfileName
        {
            get { return "DomainToViewModelMappings"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<VideoCategory, VideoCategoryEntity>();
            Mapper.CreateMap<Video, VideoEntity>();
            Mapper.CreateMap<Video, VideoToListEntity>();
            Mapper.CreateMap<Employee, ReporterEntity>();
            Mapper.CreateMap<Employee, ReporterToListEntity>()
                .ForMember(v => v.Videos, opt => opt.MapFrom(src => src.Videos.Take(8).ToList()));
            Mapper.CreateMap<VITV.Data.Models.StoreInfo.ExchangeRate, ExchangeRateEntity>()
                .ForMember(e=>e.y, opt=>opt.MapFrom(src=>src.Value))
                .ForMember(e=>e.x, opt=>opt.MapFrom(src=> ToUnixTimestamp(src.VCreateDate.Date)));
            Mapper.CreateMap<VITV.Data.Models.StoreInfo.Stock_Day, StockChartEntity>()
                .ForMember(e => e.y, opt => opt.MapFrom(src => src.Close))
                .ForMember(e => e.x, opt => opt.MapFrom(src => ToUnixTimestamp(src.Date)));
        }
        private double ToUnixTimestamp(DateTime target)
        {
            var date = new DateTime(1970, 1, 1, 0, 0, 0, target.Kind);
            var unixTimestamp = (target - date).TotalMilliseconds;
            return unixTimestamp;
        }
    }
}