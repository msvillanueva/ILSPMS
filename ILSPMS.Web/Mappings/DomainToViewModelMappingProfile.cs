using AutoMapper;
using ILSPMS.Common;
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
                .ForMember(d => d.AddedByName, v => v.MapFrom(s => s.AddedBy.FirstName + " " + s.AddedBy.LastName))
                .ForMember(d => d.Milestone, v => v.MapFrom(s => s.ProjectMovements.Count() > 0 ? s.ProjectMovements.OrderByDescending(pm => pm.ID).FirstOrDefault().Milestone.Name : ""))
                .ForMember(d => d.Activity, v => v.MapFrom(s => s.ProjectMovements.Count() > 0 ? s.ProjectMovements.OrderByDescending(pm => pm.ID).FirstOrDefault().ProjectMovementType.Name : ""))
                .ForMember(d => d.BudgetUtilized, v => v.MapFrom(s => s.ProjectActivities.Where(pa => !pa.Deleted).Count() > 0 ? s.ProjectActivities.Where(pa => !pa.Deleted).Sum(pa => pa.BudgetUtilized) : 0))
                .ForMember(d => d.MilestoneOrder, v => v.MapFrom(s => s.ProjectMovements.Count() > 0 ? s.ProjectMovements.OrderByDescending(pm => pm.ID).FirstOrDefault().Milestone.Order : 0))
                .ForMember(d => d.ApproverRoleID, v => v.MapFrom(s => s.ProjectMovements.Count() > 0 ? s.ProjectMovements.OrderByDescending(pm => pm.ID).FirstOrDefault().ApproverRoleID : 0))
                .ForMember(d => d.ApproverRoleName, v => v.MapFrom(s => s.ProjectMovements.Count() > 0 ? (s.ProjectMovements.OrderByDescending(pm => pm.ID).FirstOrDefault().ApproverRoleID != null ? s.ProjectMovements.OrderByDescending(pm => pm.ID).FirstOrDefault().ApproverRole.Name : "" ) : ""))
                .ForMember(d => d.Year, v => v.MapFrom(s => s.DateCreated.Year.ToString()))
                .ForMember(d => d.LockSubmit, v => v.MapFrom(s => s.ProjectMovements.Count() > 0 ? s.ProjectMovements.OrderByDescending(pm => pm.ID).FirstOrDefault().ProjectMovementTypeID != (int)Enumerations.ProjectMovementType.Init && s.ProjectMovements.OrderByDescending(pm => pm.ID).FirstOrDefault().ProjectMovementTypeID != (int)Enumerations.ProjectMovementType.Rejected : false));
            CreateMap<Project, NewProjectMovementViewModel>()
                .ForMember(d => d.DivisionName, v => v.MapFrom(s => s.Division.Name))
                .ForMember(d => d.ProjectManager, v => v.MapFrom(s => s.ProjectManagerID != null ? s.ProjectManager.FirstName + " " + s.ProjectManager.LastName : "Not assigned"))
                .ForMember(d => d.AddedByName, v => v.MapFrom(s => s.AddedBy.FirstName + " " + s.AddedBy.LastName))
                .ForMember(d => d.Year, v => v.MapFrom(s => s.DateCreated.Year.ToString()));
            CreateMap<ProjectMovement, ProjectMovementViewModel>()
                .ForMember(d => d.ApproverName, v => v.MapFrom(s => s.ApproverUserID != null ? s.ApproverUser.FirstName + " " + s.ApproverUser.LastName : ""))
                .ForMember(d => d.ApproverRoleName, v => v.MapFrom(s => s.ApproverRoleID != null ? s.ApproverRole.Name : ""))
                .ForMember(d => d.ProjectManagerName, v => v.MapFrom(s => s.ProjectManager.FirstName + " " + s.ProjectManager.LastName))
                .ForMember(d => d.ProjectMovementTypeName, v => v.MapFrom(s => s.ProjectMovementType.Name));
            CreateMap<ProjectActivity, ProjectActivityViewModel>()
                .ForMember(d => d.ProjectManagerName, v => v.MapFrom(s => s.User.FirstName + " " + s.User.LastName));
            CreateMap<ProjectActivityFile, ProjectActivityFileViewModel>();
        }
    }
}