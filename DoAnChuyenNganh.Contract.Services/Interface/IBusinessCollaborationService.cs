using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.ModelViews.BusinessCollaborationModelViews;
using DoAnChuyenNganh.ModelViews.ResponseDTO;

namespace DoAnChuyenNganh.Contract.Services.Interface
{
    public interface IBusinessCollaborationService
    {
        Task<BasePaginatedList<BusinessCollaborationResponseDTO>> GetBusinessCollaborations(string? id, string? businessId, string? projectName, int pageIndex, int pageSize);
        Task CreateBusinessCollaboration(BusinessCollaborationModelView businessCollaborationModelView);
        Task UpdateBusinessCollaboration(string id, BusinessCollaborationModelView businessCollaborationModelView);
        Task DeleteBusinessCollaboration(string id);
    }
}
