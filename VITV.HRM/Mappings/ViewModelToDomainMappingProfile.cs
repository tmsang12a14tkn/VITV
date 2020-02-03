using AutoMapper;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VITV.HRM.Models;
using VITV.HRM.ViewModels;

namespace VITV.HRM.Mappings
{
    public class ViewModelToDomainMappingProfile: Profile
    {
        public override string ProfileName
        {
            get { return "ViewModelToDomainMappings"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<HolidayModel, Dayoff>();

            Mapper.CreateMap<EmployeeModel, Employee>();
            Mapper.CreateMap<EmployeeModel, EmployeeWorkInfo>();
            Mapper.CreateMap<EmployeeModel, ApplicationUser>();
            
        }
    }
}