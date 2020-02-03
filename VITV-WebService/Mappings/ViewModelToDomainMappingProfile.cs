using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VITV.Data.Models;
using VITV.Data.ViewModels;
using VITV.DataService.Entity;

namespace VITV.DataService.Mappings
{
    public class ViewModelToDomainMappingProfile: Profile
    {
        public override string ProfileName
        {
            get { return "ViewModelToDomainMappings"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<VideoModel, Video>().ForMember(vi => vi.Keywords, opt => opt.Ignore()); ;
            Mapper.CreateMap<ArticleModel, Article>().ForMember(am=>am.Keywords, opt=> opt.Ignore());

            Mapper.CreateMap<VideoToListEntity, Video>();
            Mapper.CreateMap<ReporterEntity, Employee>();
        }
    }
}