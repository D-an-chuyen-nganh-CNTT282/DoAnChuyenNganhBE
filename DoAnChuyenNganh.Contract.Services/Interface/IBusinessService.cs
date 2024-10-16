using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.ModelViews.BusinessModelViews;
using DoAnChuyenNganh.ModelViews.ResponseDTO;

namespace DoAnChuyenNganh.Contract.Services.Interface
{
    public interface IBusinessService
    {
        Task<BasePaginatedList<BusinessResponseDTO>> GetBusiness(string? id, string? name, int pageIndex, int pageSize);
        Task CreateBusiness(BusinessModelView businessModelView);
        Task UpdateBusiness(string id, BusinessModelView businessModelView);
        Task DeleteBusiness(string id);
    }
}
