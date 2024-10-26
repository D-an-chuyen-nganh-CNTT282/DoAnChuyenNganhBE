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

            newincomingdocument.UserId = Guid.Parse(UserId);
            newincomingdocument.CreatedTime = CoreHelper.SystemTimeNow;
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
        
        public async Task<BasePaginatedList<IncomingDocumentResponseDTO>> Get(string? id, string? Title, Guid? userid, DateTime? duedate, int pageSize, int pageIndex)
        {
            IQueryable<IncomingDocument>? query = _unitOfWork.GetRepository<IncomingDocument>().Entities.Where(doc => doc.DeletedTime == null);
            if (!string.IsNullOrWhiteSpace(id))
            {
                query = query.Where(incomingDocument => incomingDocument.Id == id);
            }
            if (!string.IsNullOrWhiteSpace(Title))
            {
                query = query.Where(incomingDocument => incomingDocument.IncomingDocumentTitle == Title);
            }
            if (userid != null)
            {
                query = query.Where(incomingDocument => incomingDocument.UserId == userid);
            }
            if (duedate != null)
            {
                query = query.Where(incomingDocument => incomingDocument.DueDate == duedate);
            }
            int totalItems = await query.CountAsync();

            List<IncomingDocumentResponseDTO>? outgoingDocuments = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(doc => new IncomingDocumentResponseDTO
                {
                    Id = doc.Id,
                    IncomingDocumentContent = doc.IncomingDocumentContent,
                    IncomingDocumentTitle = doc.IncomingDocumentTitle,
                    IncomingDocumentProcessingStatuss = doc.IncomingDocumentProcessingStatuss.ToString(),
                    UserId = doc.UserId,
                    DepartmentId = doc.DepartmentId,
                    FileScanUrl = doc.FileScanUrl,
                    ReceivedDate = doc.ReceivedDate,
                    DueDate = doc.DueDate,
                    LastUpdatedBy = doc.LastUpdatedBy,
                    LastUpdatedTime = doc.LastUpdatedTime,
                    CreatedBy = doc.CreatedBy,
                    CreatedTime = doc.CreatedTime,
                })
                .ToListAsync();

            return new BasePaginatedList<IncomingDocumentResponseDTO>(outgoingDocuments, totalItems, pageIndex, pageSize);
        }


        public async Task Update(string id, IncomingDocumentModelViews incomingdocumentView)
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
            incomingDocument.UserId = Guid.Parse(UserId);
            await _unitOfWork.GetRepository<IncomingDocument>().UpdateAsync(incomingDocument);
            await _unitOfWork.SaveAsync();
        }
    }
}
