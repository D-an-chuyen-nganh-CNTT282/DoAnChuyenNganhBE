using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.ModelViews.AlumniActivitiesModelViews;
using DoAnChuyenNganh.ModelViews.ResponseDTO;

namespace DoAnChuyenNganh.Contract.Services.Interface
{
    public interface IAlumniActivitiesService
    {
        Task<BasePaginatedList<AlumniActivitiesResponseDTO>> GetAlumniActivities(string? id, string? alumniId, string? ActivitiesId, int pageIndex, int pageSize);
        Task CreateAlumniActivities(AlumniActivitiesModelView alumniActivitiesModelView);
        Task UpdateAlumniActivities(string id, string alumiId, string activitiesId, AlumniActivitiesModelView alumniActivitiesModelView);
        Task DeleteAlumniActivities(string id, string alumniId, string activitiesId);
    }
}
