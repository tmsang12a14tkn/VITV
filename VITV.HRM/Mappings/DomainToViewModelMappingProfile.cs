using AutoMapper;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Linq;
using VITV.HRM.Models;
using VITV.HRM.ViewModels;
namespace VITV.HRM.Mappings
{
    public class DomainToViewModelMappingProfile: Profile
    {
        public override string ProfileName
        {
            get { return "DomainToViewModelMappings"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<Dayoff, HolidayModel>();
            Mapper.CreateMap<Employee, EmployeeModel>();
            Mapper.CreateMap<ApplicationUser, EmployeeModel>();
            Mapper.CreateMap<EmployeeWorkInfo, EmployeeModel>();
        }
    }
}