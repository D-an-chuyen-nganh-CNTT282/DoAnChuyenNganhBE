using AutoMapper;
using DoAnChuyenNganh.Contract.Repositories.Entity;
using DoAnChuyenNganh.ModelViews.ActivitiesModelViews;
using DoAnChuyenNganh.ModelViews.AuthModelViews;
using DoAnChuyenNganh.ModelViews.BusinessActivitiesModelViews;
using DoAnChuyenNganh.ModelViews.BusinessCollaborationModelViews;
using DoAnChuyenNganh.ModelViews.BusinessModelViews;
using DoAnChuyenNganh.ModelViews.InternshipMangamentModelViews;
using DoAnChuyenNganh.ModelViews.LecturerActivitiesModelViews;
using DoAnChuyenNganh.ModelViews.LecturerModelViews;
using DoAnChuyenNganh.ModelViews.LecturerPlanModelViews;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using DoAnChuyenNganh.ModelViews.RoleViewModel;
using DoAnChuyenNganh.ModelViews.TeachingScheduleModelViews;
using DoAnChuyenNganh.ModelViews.UserModelViews;
using DoAnChuyenNganh.Repositories.Entity;

namespace DoAnChuyenNganh.Services.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserUpdateByAdminModel, ApplicationUser>()
                .ForMember(x => x.PasswordHash, option => option.Ignore());

            CreateMap<ApplicationUser, UserProfileResponseModelView>().ReverseMap();

            CreateMap<UserModelView, ApplicationUser>()
            .ForMember(dest => dest.LastUpdatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.LastUpdatedTime, opt => opt.Ignore());

            CreateMap<ApplicationRole, RoleViewModel>()
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Name));
            CreateMap<UserUpdateModelView, ApplicationUser>().ReverseMap();

            CreateMap<Lecturer, LecturerModelView>().ReverseMap();
            CreateMap<LecturerResponseDTO, Lecturer>().ReverseMap();

            CreateMap<LecturerPlan, LecturerPlanModelView>().ReverseMap();
            CreateMap<LecturerPlanResponseDTO, LecturerPlan>().ReverseMap();


            CreateMap<LecturerActivities, LecturerActivitiesModelView>().ReverseMap();
            CreateMap<LecturerActivitiesResponseDTO, LecturerActivities>().ReverseMap();

            CreateMap<Activities, ActivitiesModelView>().ReverseMap();
            CreateMap<ActivitiesResponseDTO, Activities>().ReverseMap();

            CreateMap<TeachingSchedule, TeachingScheduleModelView>().ReverseMap();
            CreateMap<TeachingScheduleResponseDTO, TeachingSchedule>().ReverseMap();

            CreateMap<Business, BusinessModelView>().ReverseMap();
            CreateMap<BusinessResponseDTO, Business>().ReverseMap();

            CreateMap<BusinessCollaboration, BusinessCollaborationModelView>().ReverseMap();
            CreateMap<BusinessCollaborationResponseDTO, BusinessCollaboration>().ReverseMap();

            CreateMap<BusinessActivities, BusinessActivitiesModelView>().ReverseMap();
            CreateMap<BusinessActivitiesResponseDTO, BusinessActivities>().ReverseMap();

            CreateMap<InternshipManagement, InternshipManagementModelView>().ReverseMap();
            CreateMap<InternshipManagementResponseDTO, InternshipManagement>().ReverseMap();
        }
    }
}
