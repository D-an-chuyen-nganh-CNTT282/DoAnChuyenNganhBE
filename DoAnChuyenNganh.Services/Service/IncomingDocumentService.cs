using AutoMapper;
using DoAnChuyenNganh.Contract.Repositories.Entity;
using DoAnChuyenNganh.Contract.Repositories.Interface;
using DoAnChuyenNganh.Contract.Services.Interface;
using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.Core.Store;
using DoAnChuyenNganh.Core.Utils;
using DoAnChuyenNganh.ModelViews.IncomingDocumentModelViews;
using DoAnChuyenNganh.ModelViews.LecturerModelViews;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Status = DoAnChuyenNganh.Contract.Repositories.Entity.IncomingDocument.IncomingDocumentProcessingStatus;
namespace DoAnChuyenNganh.Services.Service
{
    public class IncomingDocumentService : IIncomingDocumentService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public IncomingDocumentService(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task Create(IncomingDocumentModelViews incomingdocumentView)
        {
            string? UserId =  _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            IncomingDocument newincomingdocument = _mapper.Map<IncomingDocument>(incomingdocumentView);
            newincomingdocument.IncomingDocumentProcessingStatuss = Status.InProcess;
            newincomingdocument.UserId = Guid.Parse(UserId);
            newincomingdocument.CreatedTime = CoreHelper.SystemTimeNow;
            newincomingdocument.DeletedTime = null;
            newincomingdocument.CreatedBy = UserId;
            await _unitOfWork.GetRepository<IncomingDocument>().InsertAsync(newincomingdocument);
            await _unitOfWork.SaveAsync();
        }

        public async Task Delete(string id)
        {
            string? UserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã công văn đến!");
            }
            IncomingDocument? incomingDocument = await _unitOfWork.GetRepository<IncomingDocument>().GetByIdAsync(id)
                ?? throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, $"Không tìm công văn đến nào với mã {id}!");
            if (incomingDocument.DeletedTime != null)
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, "Thông tin công văn đến đã bị xóa!");
            }
            incomingDocument.DeletedBy = UserId;
            incomingDocument.DeletedTime = CoreHelper.SystemTimeNow;
            await _unitOfWork.GetRepository<IncomingDocument>().UpdateAsync(incomingDocument);
            await _unitOfWork.SaveAsync();
        }
        private async Task<BasePaginatedList<IncomingDocumentResponseDTO>> PaginateIncomingDocument(IQueryable<IncomingDocument> query,int? pageIndex,int? pageSize)
        {
            int currentPage = pageIndex ?? 1;
            int currentPageSize = pageSize ?? 10;
            int totalItems = await query.CountAsync();

            List<IncomingDocumentResponseDTO>? incomingDocuments = await query
                .Skip((currentPage - 1) * currentPageSize)
                .Take(currentPageSize)
                .Select(icomingDocument => new IncomingDocumentResponseDTO
                {
                    Id = icomingDocument.Id,
                    IncomingDocumentTitle = icomingDocument.IncomingDocumentTitle,
                    IncomingDocumentContent = icomingDocument.IncomingDocumentContent,
                    IncomingDocumentProcessingStatuss = icomingDocument.IncomingDocumentProcessingStatuss.ToString(),
                    UserId = icomingDocument.UserId,
                    CreatedBy = icomingDocument.CreatedBy,
                    LastUpdatedBy = icomingDocument.LastUpdatedBy,
                    CreatedTime = icomingDocument.CreatedTime,
                    LastUpdatedTime = icomingDocument.LastUpdatedTime,
                })
                .ToListAsync();

            return new BasePaginatedList<IncomingDocumentResponseDTO>(incomingDocuments, totalItems, currentPage, currentPageSize);
        }
        public async Task<BasePaginatedList<IncomingDocumentResponseDTO>> Get(string? id, string? Title, Guid userid, DateTime duedate, int pageSize, int pageIndex)
        {
            IQueryable<IncomingDocument>? query = _unitOfWork.GetRepository<IncomingDocument>().Entities.Where(l => l.DeletedTime == null);
            if (!string.IsNullOrWhiteSpace(id))
            {
                query = query.Where(incomingDocument => incomingDocument.Id == id);
            }
            if (!string.IsNullOrWhiteSpace(Title))
            {
                query = query.Where(incomingDocument => incomingDocument.IncomingDocumentTitle == Title);
            }
            if (userid != Guid.Empty)
            {
                query = query.Where(incomingDocument => incomingDocument.UserId == userid);
            }
            if (duedate != null)
            {
                query = query.Where(incomingDocument => incomingDocument.DueDate == duedate);
            }
            return await PaginateIncomingDocument(query, pageIndex, pageSize);
        }


        public async Task Update(string? id, IncomingDocumentModelViews incomingdocumentView)
        {
            string? UserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã công văn đến!");
            }
            IncomingDocument? incomingDocument = await _unitOfWork.GetRepository<IncomingDocument>().GetByIdAsync(id)
                ?? throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, $"Không tìm thấy công văn đến nào với mã {id}!");
            _mapper.Map(incomingdocumentView, incomingDocument);
            incomingDocument.LastUpdatedTime = CoreHelper.SystemTimeNow;
            incomingDocument.LastUpdatedBy = UserId;
            await _unitOfWork.GetRepository<IncomingDocument>().UpdateAsync(incomingDocument);
            await _unitOfWork.SaveAsync();
        }
    }
}
