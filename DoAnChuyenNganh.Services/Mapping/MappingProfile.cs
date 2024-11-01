using AutoMapper;
using DoAnChuyenNganh.Contract.Repositories.Entity;
using DoAnChuyenNganh.ModelViews.ActivitiesModelViews;
using DoAnChuyenNganh.ModelViews.BusinessActivitiesModelViews;
using DoAnChuyenNganh.ModelViews.BusinessCollaborationModelViews;
using DoAnChuyenNganh.ModelViews.BusinessModelViews;
using DoAnChuyenNganh.ModelViews.ExtracurricularActivitiesModelViews;
using DoAnChuyenNganh.ModelViews.InternshipMangamentModelViews;
using DoAnChuyenNganh.ModelViews.LecturerActivitiesModelViews;
using DoAnChuyenNganh.ModelViews.LecturerModelViews;
using DoAnChuyenNganh.ModelViews.LecturerPlanModelViews;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using DoAnChuyenNganh.ModelViews.RoleViewModel;
using DoAnChuyenNganh.ModelViews.StudentExpectationsModelViews;
using DoAnChuyenNganh.ModelViews.StudentModelViews;
using DoAnChuyenNganh.ModelViews.TeachingScheduleModelViews;
using DoAnChuyenNganh.ModelViews.UserModelViews;
using DoAnChuyenNganh.Repositories.Entity;
using DoAnChuyenNganh.ModelViews.OutgoingDocumentModelViews;
using DoAnChuyenNganh.ModelViews.AlumniActivitiesModelViews;
using DoAnChuyenNganh.ModelViews.AlumniCompanyModelViews;
using DoAnChuyenNganh.ModelViews.AlumniModelViews;
using DoAnChuyenNganh.ModelViews.CompanyModelViews;
using DoAnChuyenNganh.ModelViews.IncomingDocumentModelViews;

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
            CreateMap<Lecturer, LecturerResponseDTO>();

            CreateMap<LecturerPlan, LecturerPlanModelView>().ReverseMap();
            CreateMap<LecturerPlan, LecturerPlanResponseDTO>();


            CreateMap<LecturerActivities, LecturerActivitiesModelView>().ReverseMap();
            CreateMap<LecturerActivities, LecturerActivitiesResponseDTO>();

            CreateMap<Activities, ActivitiesModelView>().ReverseMap();
            CreateMap<Activities, ActivitiesResponseDTO>();

            CreateMap<TeachingSchedule, TeachingScheduleModelView>().ReverseMap();
            CreateMap<TeachingSchedule, TeachingScheduleResponseDTO>();

            CreateMap<Business, BusinessModelView>().ReverseMap();
            CreateMap<Business, BusinessResponseDTO>();

            CreateMap<BusinessCollaboration, BusinessCollaborationModelView>().ReverseMap();
            CreateMap<BusinessCollaboration, BusinessCollaborationResponseDTO>();

            CreateMap<BusinessActivities, BusinessActivitiesModelView>().ReverseMap();
            CreateMap<BusinessActivities, BusinessActivitiesResponseDTO>();

            CreateMap<InternshipManagement, InternshipManagementModelView>().ReverseMap();
            CreateMap<InternshipManagement, InternshipManagementResponseDTO>();

            CreateMap<Student, StudentModelView>().ReverseMap();
            CreateMap<Student, StudentResponseDTO>();

            CreateMap<ExtracurricularActivities, ExtracurricularActivitiesModelView>().ReverseMap();
            CreateMap<ExtracurricularActivities, ExtracurricularActivitiesReponseDTO>();

            CreateMap<StudentExpectations, StudentExpectationsModelView>().ReverseMap();
            CreateMap<StudentExpectations, StudentExpectationsResponseDTO>();

            CreateMap<OutgoingDocument, OutgoingDocumentModelView>().ReverseMap();
            CreateMap<OutgoingDocument, OutgoingDocumentResponseDTO>();

            CreateMap<Company, CompanyResponseDTO>();
            CreateMap<CompanyModelViews, Company>().ReverseMap();

            CreateMap<Alumni, AlumniResponseDTO>();
            CreateMap<AlumniModelView, Alumni>().ReverseMap();
            CreateMap<IncomingDocument, IncomingDocumentResponseDTO>();
            CreateMap<IncomingDocumentModelViews, IncomingDocument>().ReverseMap();
            CreateMap<AlumniCompany, AlumniCompanyResponseDTO>();
            CreateMap<AlumniCompanyModelView, AlumniCompany>().ReverseMap();

            CreateMap<AlumniActivities, AlumniActivitiesResponseDTO>();
            CreateMap<AlumniActivitiesModelView, AlumniActivities>().ReverseMap();

            CreateMap<Department, DepartmentResponseDTO>();

        }
    }
}
