using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.ModelViews.InternshipMangamentModelViews;
using DoAnChuyenNganh.ModelViews.ResponseDTO;

namespace DoAnChuyenNganh.Contract.Services.Interface
{
    public interface IInternshipManagementService
    {
        Task<BasePaginatedList<InternshipManagementResponseDTO>> GetInternshipManagements(string? id, string? studentId, string? businessId, int pageIndex, int pageSize);
        Task CreateInternshipManagement(InternshipManagementModelView internshipManagementModelView);
        Task UpdateInternshipManagement(string id, string studentId, string businessId, InternshipManagementModelView internshipManagementModelView);
        Task DeleteInternshipManagement(string id, string studentId, string businessId);
    }
}
