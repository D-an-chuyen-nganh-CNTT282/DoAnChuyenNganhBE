using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.ModelViews.ExtracurricularActivitiesModelViews;
using DoAnChuyenNganh.ModelViews.ResponseDTO;

namespace DoAnChuyenNganh.Contract.Services.Interface
{
    public interface IExtracurricularActivitiesService
    {
        Task<BasePaginatedList<ExtracurricularActivitiesReponseDTO>> GetExtracurricularActivities(string? id, string? studentId, string? activitiesId, int pageIndex, int pageSize);
        Task CreateExtracurricularActivities(ExtracurricularActivitiesModelView extracurricularActivitiesModelView);
        Task UpdateExtracurricularActivities(string id, string studentId, string activitiesId, ExtracurricularActivitiesModelView extracurricularActivitiesModelView);
    }
}
