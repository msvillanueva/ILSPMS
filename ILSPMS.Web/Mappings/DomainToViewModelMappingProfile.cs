using AutoMapper;
using ILSPMS.Entities;
using ILSPMS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ILSPMS.Web.Mappings
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<User, UserViewModel>()
                .ForMember(d => d.FullName, v => v.MapFrom(s => s.FirstName + " " + s.LastName))
                .ForMember(d => d.DivisionName, v => v.MapFrom(s => s.Division.Name));
            CreateMap<Division, DivisionViewModel>();
            CreateMap<Role, RoleViewModel>();
            CreateMap<Project, ProjectViewModel>()
                .ForMember(d => d.DivisionName, v => v.MapFrom(s => s.Division.Name))
                .ForMember(d => d.ProjectManager, v => v.MapFrom(s => s.ProjectManagerID != null ? s.ProjectManager.FirstName + " " + s.ProjectManager.LastName : "Not assigned"))
                .ForMember(d => d.Year, v => v.MapFrom(s => s.DateCreated.Year.ToString()));
        }
    }
}