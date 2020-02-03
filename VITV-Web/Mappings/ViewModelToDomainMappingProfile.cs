using AutoMapper;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VITV.Data.Helpers;
using VITV.Data.Models;
using VITV.Data.ViewModels;
using VITV.Web.Helpers;

namespace VITV.Web.Mappings
{
    public class ViewModelToDomainMappingProfile: Profile
    {
        public override string ProfileName
        {
            get { return "ViewModelToDomainMappings"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<VideoModel, Video>().ForMember(vi => vi.Keywords, opt => opt.Ignore())
                .ForMember(vi=>vi.UniqueTitle, opt=> opt.MapFrom(src=> UrlHelper.URLFriendly(src.Title)));
            Mapper.CreateMap<ArticleModel, Article>().ForMember(am=>am.Keywords, opt=> opt.Ignore())
                .ForMember(a=>a.UniqueTitle, opt=> opt.MapFrom(src=> UrlHelper.URLFriendly(src.Title)));
            Mapper.CreateMap<EmployeeModel, Employee>().ForMember(am => am.Roles, opt => opt.Ignore());
            Mapper.CreateMap<SpecialEventModel, SpecialEvent>();
            Mapper.CreateMap<HolidayModel, Holiday>();
            Mapper.CreateMap<BankModel, Bank>();
            Mapper.CreateMap<CreateRoleModel, IdentityRole>();
            Mapper.CreateMap<EditUserModel, IdentityUser>();
            Mapper.CreateMap<VideoTranscriptModel, VideoTranscript>();
        }
    }
}