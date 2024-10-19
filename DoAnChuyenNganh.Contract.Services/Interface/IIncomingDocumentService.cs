using DoAnChuyenNganh.Contract.Repositories.Entity;
using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.ModelViews.IncomingDocumentModelViews;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DoAnChuyenNganh.Contract.Repositories.Entity.IncomingDocument;

namespace DoAnChuyenNganh.Contract.Services.Interface
{
    public interface IIncomingDocumentService
    {
        Task<BasePaginatedList<IncomingDocumentResponseDTO>> Get(string? id, string? Title, Guid userid, DateTime duedate, int pageSize, int pageIndex);
        Task Create(IncomingDocumentModelViews incomingdocumentView);
        Task Update(string id, IncomingDocumentModelViews incomingdocumentView, IncomingDocumentProcessingStatus status);
        Task Delete(string id);
    }
}
