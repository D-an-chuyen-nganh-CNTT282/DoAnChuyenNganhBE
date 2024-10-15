using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using DoAnChuyenNganh.ModelViews.TeachingScheduleModelViews;

namespace DoAnChuyenNganh.Contract.Services.Interface
{
    public interface ITeachingScheduleService
    {
        Task<BasePaginatedList<TeachingScheduleResponseDTO>> GetTeachingSchedules(string? id, string? subject, int pageIndex, int pageSize);
        Task CreateTeachingSchedule(TeachingScheduleModelView teachingScheduleModelView);
        Task UpdateTeachingSchedule(string id, TeachingScheduleModelView teachingScheduleModelView);
        Task DeleteTeachingSchedule(string id);
    }
}
