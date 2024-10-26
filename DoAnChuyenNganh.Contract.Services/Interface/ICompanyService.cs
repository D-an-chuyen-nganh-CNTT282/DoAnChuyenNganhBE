using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.ModelViews.CompanyModelViews;
using DoAnChuyenNganh.ModelViews.ResponseDTO;

namespace DoAnChuyenNganh.Contract.Services.Interface
{
    public interface ICompanyService
    {
        Task<BasePaginatedList<CompanyResponseDTO>> GetCompany(string? id, string? name, int pageIndex, int pageSize);
        Task CreateCompany(CompanyModelViews companyModelView);
        Task UpdateCompany(string id, CompanyModelViews companyModelView);
        Task DeleteCompany(string id);
    }
}
