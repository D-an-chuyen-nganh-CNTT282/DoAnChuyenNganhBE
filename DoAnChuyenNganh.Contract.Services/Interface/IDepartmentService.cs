using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.ModelViews.ResponseDTO;

namespace DoAnChuyenNganh.Contract.Services.Interface
{
    public interface IDepartmentService
    {
        Task<BasePaginatedList<DepartmentResponseDTO>> GetDepartments(string? id, string? name, int pageIndex, int pageSize);
    }
}
