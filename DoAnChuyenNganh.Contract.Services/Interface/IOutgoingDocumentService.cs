using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using DoAnChuyenNganh.ModelViews.OutgoingDocumentModelViews;
using DoAnChuyenNganh.ModelViews.IncomingDocumentModelViews;

namespace DoAnChuyenNganh.Contract.Services.Interface
{
    public interface IOutgoingDocumentService
    {
        Task<BasePaginatedList<OutgoingDocumentResponseDTO>> GetOutgoingDocuments(string? title, string? departmentId, Guid? userId, int pageIndex, int pageSize);
        Task CreateOutgoingDocument(OutgoingDocumentModelView outgoingDocumentModelView);
        Task UpdateOutgoingDocument(string id, OutgoingDocumentModelView outgoingDocumentModelView);
    }
}
