using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.ModelViews.BusinessActivitiesModelViews;
using DoAnChuyenNganh.ModelViews.ResponseDTO;

namespace DoAnChuyenNganh.Contract.Services.Interface
{
    public interface IBusinessActivitesService
    {
        Task<BasePaginatedList<BusinessActivitiesResponseDTO>> GetBusinessActivities(string? id, string? businessId, string? activitiesId, int pageIndex, int pageSize);
        Task CreateBusinessActivities(BusinessActivitiesModelView businessActivitiesModelView);
        Task UpdateBusinessActivities(string id, string businessId, string activitiesId, BusinessActivitiesModelView businessActivitiesModelView);
        Task DeleteBusinessActivities(string id, string businessId, string activitiesId);
    }
}
