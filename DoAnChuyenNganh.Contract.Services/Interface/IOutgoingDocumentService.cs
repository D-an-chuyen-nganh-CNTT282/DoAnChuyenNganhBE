using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using DoAnChuyenNganh.ModelViews.OutgoingDocumentModelViews;

namespace DoAnChuyenNganh.Contract.Services.Interface
{
    public interface IOutgoingDocumentService
    {
        Task<BasePaginatedList<OutgoingDocumentResponseDTO>> GetOutgoingDocuments(string? title, string? departmentId, Guid? userId, int pageIndex, int pageSize);
        Task CreateOutgoingDocument(OutgoingDocumentModelView outgoingDocumentModelView);
    }
}
