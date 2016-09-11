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
            //CreateMap<PendingStore, PendingStoreViewModel>()
            //    .ForMember(d => d.ContactName, v => v.MapFrom(s => s.FirstName + " " + s.LastName))
            //    .ForMember(d => d.Company, v => v.MapFrom(s => s.Store))
            //    .ForMember(d => d.Email, v => v.MapFrom(s => s.ContactEmail))
            //    .ForMember(d => d.ContactNo, v => v.MapFrom(s => s.PhoneNo))
            //    .ForMember(d => d.ShortAdd, v => v.MapFrom(s => s.Address.Length > 20 ? s.Address.Substring(0, 20) : s.Address));
            //CreateMap<Store, StoreViewModel>()
            //    .ForMember(d => d.PackageName, v => v.MapFrom(s => s.StorePackages.OrderByDescending(p => p.DateCreated).FirstOrDefault() != null ? s.StorePackages.OrderByDescending(p => p.DateCreated).FirstOrDefault().Package.Name : "No package"))
            //    .ForMember(d => d.PackageID, v => v.MapFrom(s => s.StorePackages.OrderByDescending(p => p.DateCreated).FirstOrDefault() != null ? s.StorePackages.OrderByDescending(p => p.DateCreated).FirstOrDefault().PackageID : 0))
            //    .ForMember(d => d.HasPackage, v => v.MapFrom(s => s.StorePackages.OrderByDescending(p => p.DateCreated).FirstOrDefault() != null))
            //    .ForMember(d => d.PackageValidUntil, v => v.MapFrom(s => s.StorePackages.OrderByDescending(p => p.DateCreated).FirstOrDefault() != null ? s.StorePackages.OrderByDescending(p => p.DateCreated).FirstOrDefault().DateValidUntil : new DateTime()))
            //    .ForMember(d => d.CreatedByName, v => v.MapFrom(s => s.CreatedBy.FirstName + " " + s.CreatedBy.LastName));
            //CreateMap<Package, PackageViewModel>();
            //CreateMap<AdBanner, AdBannerViewModel>()
            //    .ForMember(d => d.StoreName, v => v.MapFrom(s => s.Store.Name));
            //CreateMap<TextMessage, TextMessageViewModel>()
            //    .ForMember(d => d.ShortMessage, v => v.MapFrom(s => (s.Message.Length > 40 ? s.Message.Substring(0, 40) + "..." : s.Message)))
            //    .ForMember(d => d.SenderName, v => v.MapFrom(s => s.User.FirstName + " " + s.User.LastName));
            //CreateMap<Customer, CustomerViewModel>()
            //    .ForMember(d => d.FullName, v => v.MapFrom(s => s.FirstName + " " + s.LastName));

        }
    }
}