using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.ModelViews.LecturerActivitiesModelViews;
using DoAnChuyenNganh.ModelViews.ResponseDTO;

namespace DoAnChuyenNganh.Contract.Services.Interface
{
    public interface ILecturerActivitiesService
    {
        Task<BasePaginatedList<LecturerActivitiesResponseDTO>> GetLecturerActivities(string? id, string? lecturerId, string? activitiesId, int pageIndex, int pageSize);
        Task CreateLecturerActivities(LecturerActivitiesModelView lecturerActivitiesModelView);
        Task UpdateLecturerActivities(string id, string lecturerId, string activitiesId, LecturerActivitiesModelView lecturerActivitiesModelView);
        Task DeleteLecturerActivities(string id, string lecturerId, string activitiesId);
    }
}
