using AutoMapper;
using DoAnChuyenNganh.Contract.Repositories.Entity;
using DoAnChuyenNganh.ModelViews.AuthModelViews;
using DoAnChuyenNganh.ModelViews.LecturerModelViews;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using DoAnChuyenNganh.ModelViews.RoleViewModel;
using DoAnChuyenNganh.ModelViews.UserModelViews;
using DoAnChuyenNganh.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnChuyenNganh.Services.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserUpdateByAdminModel, ApplicationUser>()
                .ForMember(x => x.PasswordHash, option => option.Ignore());

            CreateMap<ApplicationUser, UserProfileResponseModelView>().ReverseMap();

            CreateMap<ApplicationUser, RegisterModelView>().ReverseMap();

            CreateMap<UserModelView, ApplicationUser>()
            .ForMember(dest => dest.LastUpdatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.LastUpdatedTime, opt => opt.Ignore());

            CreateMap<ApplicationRole, RoleViewModel>()
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Name));
            CreateMap<UserUpdateModelView, ApplicationUser>().ReverseMap();

            CreateMap<Lecturer, LecturerModelView>().ReverseMap();
            CreateMap<LecturerResponseDTO, Lecturer>().ReverseMap();
        }
    }
}
