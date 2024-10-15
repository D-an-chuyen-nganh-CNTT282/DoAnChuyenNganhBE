using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.ModelViews.ActivitiesModelViews;
using DoAnChuyenNganh.ModelViews.ResponseDTO;

namespace DoAnChuyenNganh.Contract.Services.Interface
{
    public interface IActivitiesService
    {
        Task<BasePaginatedList<ActivitiesResponseDTO>> GetActivities(string? id, string? name, int pageIndex, int pageSize);
        Task CreateActivities(ActivitiesModelView activitiesModelView);
        Task UpdateActivities(string id, ActivitiesModelView activitiesModelView);
        Task DeleteActivities(string id);
    }
}
