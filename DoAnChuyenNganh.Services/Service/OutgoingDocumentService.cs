using AutoMapper;
using DoAnChuyenNganh.Contract.Repositories.Entity;
using DoAnChuyenNganh.Contract.Repositories.Interface;
using DoAnChuyenNganh.Contract.Services.Interface;
using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.Core.Store;
using DoAnChuyenNganh.Core.Utils;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using DoAnChuyenNganh.ModelViews.OutgoingDocumentModelViews;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DoAnChuyenNganh.Services.Service
{
    public class OutgoingDocumentService : IOutgoingDocumentService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OutgoingDocumentService(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateOutgoingDocument(OutgoingDocumentModelView outgoingDocumentModelView)
        {
            string? userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }

            OutgoingDocument outgoingDocument = _mapper.Map<OutgoingDocument>(outgoingDocumentModelView);

            outgoingDocument.UserId = Guid.Parse(userId);
            outgoingDocument.CreatedBy = userId;
            outgoingDocument.CreatedTime = CoreHelper.SystemTimeNow.DateTime;
            outgoingDocument.LastUpdatedBy = userId;
            outgoingDocument.LastUpdatedTime = CoreHelper.SystemTimeNow.DateTime;

            await _unitOfWork.GetRepository<OutgoingDocument>().InsertAsync(outgoingDocument);
            await _unitOfWork.SaveAsync();
        }

        public async Task<BasePaginatedList<OutgoingDocumentResponseDTO>> GetOutgoingDocuments(string? title, string? departmentId, Guid? userId, int pageIndex, int pageSize)
        {
            IQueryable<OutgoingDocument>? query = _unitOfWork.GetRepository<OutgoingDocument>().Entities.Where(doc => doc.DeletedTime == null);

            if (!string.IsNullOrWhiteSpace(title))
            {
                query = query.Where(doc => doc.OutgoingDocumentTitle.Contains(title));
            }
            if (!string.IsNullOrWhiteSpace(departmentId))
            {
                query = query.Where(doc => doc.DepartmentId == departmentId);
            }
            if (userId.HasValue)
            {
                query = query.Where(doc => doc.UserId == userId);
            }

            int totalItems = await query.CountAsync();

            List<OutgoingDocumentResponseDTO>? outgoingDocuments = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(doc => new OutgoingDocumentResponseDTO
                {
                    Id = doc.Id,
                    OutgoingDocumentTitle = doc.OutgoingDocumentTitle,
                    OutgoingDocumentContent = doc.OutgoingDocumentContent,
                    DepartmentId = doc.DepartmentId,
                    RecipientEmail = doc.RecipientEmail,
                    UserId = doc.UserId,
                    OutgoingDocumentProcessingStatuss = doc.OutgoingDocumentProcessingStatuss.ToString(),
                    CreatedBy = doc.CreatedBy,
                    LastUpdatedBy = doc.LastUpdatedBy,
                    CreatedTime = doc.CreatedTime,
                    LastUpdatedTime = doc.LastUpdatedTime
                })
                .ToListAsync();

            return new BasePaginatedList<OutgoingDocumentResponseDTO>(outgoingDocuments, totalItems, pageIndex, pageSize);
        }
    }
}
