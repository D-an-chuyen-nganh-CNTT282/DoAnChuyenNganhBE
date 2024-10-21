using AutoMapper;
using DoAnChuyenNganh.Contract.Repositories.Entity;
using DoAnChuyenNganh.Contract.Repositories.Interface;
using DoAnChuyenNganh.Contract.Services.Interface;
using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.Core.Store;
using DoAnChuyenNganh.ModelViews.IncomingDocumentModelViews;
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
            string? UserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            // Map từ View Model sang Model
            var IncomingDocument = _mapper.Map<IncomingDocument>(incomingdocumentView);
            IncomingDocument.IncomingDocumentProcessingStatuss = Status.InProcess;
            // Thêm tài liệu vào repository
            IncomingDocument.UserId = Guid.Parse(UserId);
            await _unitOfWork.GetRepository<IncomingDocument>().InsertAsync(IncomingDocument);

            // Lưu thay đổi vào cơ sở dữ liệu
            await _unitOfWork.SaveAsync();
        }

        public async Task Delete(string id)
        {

            if (string.IsNullOrWhiteSpace(id))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Lỗi!!! Dữ liệu không tồn tại ");
            }
            // Tìm tài liệu theo id
            var document = await _unitOfWork.GetRepository<IncomingDocument>().GetByIdAsync(id);

            // Xóa tài liệu
            await _unitOfWork.GetRepository<IncomingDocument>().DeleteAsync(id);

            // Lưu thay đổi vào cơ sở dữ liệu
            await _unitOfWork.SaveAsync();
        }

        public async Task<BasePaginatedList<IncomingDocumentResponseDTO>> Get(string? id, string? Title, Guid userid, DateTime duedate, int pageSize, int pageIndex)
        {
            if (pageIndex == 0 && pageSize == 0)
            {
                pageSize = 5;
                pageIndex = 1;
            }

            IQueryable<IncomingDocument> icm = _unitOfWork.GetRepository<IncomingDocument>().Entities;

            // Không có bộ lọc nào được áp dụng
            if (string.IsNullOrEmpty(id) && string.IsNullOrEmpty(Title) && userid == Guid.Empty && duedate == DateTime.MinValue)
            {
                var totalCount = await icm.CountAsync();

                var incomingDocs = await icm
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var incomingDocDTOs = _mapper.Map<List<IncomingDocumentResponseDTO>>(incomingDocs);

                return new BasePaginatedList<IncomingDocumentResponseDTO>(incomingDocDTOs, totalCount, pageIndex, pageSize);
            }

            // Lọc theo id
            if (!string.IsNullOrEmpty(id) && string.IsNullOrEmpty(Title) && userid == Guid.Empty && duedate == DateTime.MinValue)
            {
                var filteredDocs = await icm
                    .Where(doc => doc.Id == id)
                    .ToListAsync();

                var totalCount = filteredDocs.Count;
                var incomingDocDTOs = _mapper.Map<List<IncomingDocumentResponseDTO>>(filteredDocs);

                return new BasePaginatedList<IncomingDocumentResponseDTO>(incomingDocDTOs, totalCount, pageIndex, pageSize);
            }

            // Lọc theo Title
            if (string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(Title) && userid == Guid.Empty && duedate == DateTime.MinValue)
            {
                var filteredDocs = await icm
                    .Where(doc => doc.IncomingDocumentTitle.Contains(Title))
                    .ToListAsync();

                var totalCount = filteredDocs.Count;
                var incomingDocDTOs = _mapper.Map<List<IncomingDocumentResponseDTO>>(filteredDocs);

                return new BasePaginatedList<IncomingDocumentResponseDTO>(incomingDocDTOs, totalCount, pageIndex, pageSize);
            }

            // Lọc theo UserId
            if (string.IsNullOrEmpty(id) && string.IsNullOrEmpty(Title) && userid != Guid.Empty && duedate == DateTime.MinValue)
            {
                var filteredDocs = await icm
                    .Where(doc => doc.UserId == userid)
                    .ToListAsync();

                var totalCount = filteredDocs.Count;
                var incomingDocDTOs = _mapper.Map<List<IncomingDocumentResponseDTO>>(filteredDocs);

                return new BasePaginatedList<IncomingDocumentResponseDTO>(incomingDocDTOs, totalCount, pageIndex, pageSize);
            }

            // Lọc theo DueDate
            if (string.IsNullOrEmpty(id) && string.IsNullOrEmpty(Title) && userid == Guid.Empty && duedate != DateTime.MinValue)
            {
                var filteredDocs = await icm
                    .Where(doc => doc.DueDate.Date == duedate.Date)
                    .ToListAsync();

                var totalCount = filteredDocs.Count;
                var incomingDocDTOs = _mapper.Map<List<IncomingDocumentResponseDTO>>(filteredDocs);

                return new BasePaginatedList<IncomingDocumentResponseDTO>(incomingDocDTOs, totalCount, pageIndex, pageSize);
            }

            // Xử lý trường hợp có nhiều bộ lọc có thể được áp dụng
            var filteredQuery = icm;

            if (!string.IsNullOrEmpty(id))
            {
                filteredQuery = filteredQuery.Where(doc => doc.Id == id);
            }

            if (!string.IsNullOrEmpty(Title))
            {
                filteredQuery = filteredQuery.Where(doc => doc.IncomingDocumentTitle.Contains(Title));
            }

            if (userid != Guid.Empty)
            {
                filteredQuery = filteredQuery.Where(doc => doc.UserId == userid);
            }

            if (duedate != DateTime.MinValue)
            {
                filteredQuery = filteredQuery.Where(doc => doc.DueDate.Date == duedate.Date);
            }

            var totalFilteredCount = await filteredQuery.CountAsync();
            var filteredDocsPaged = await filteredQuery
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var filteredDocDTOs = _mapper.Map<List<IncomingDocumentResponseDTO>>(filteredDocsPaged);

            return new BasePaginatedList<IncomingDocumentResponseDTO>(filteredDocDTOs, totalFilteredCount, pageIndex, pageSize);
        }


        public async Task Update(string? id, IncomingDocumentModelViews incomingdocumentView)
        {
            var existingDocument = await _unitOfWork.GetRepository<IncomingDocument>().GetByIdAsync(id);

            if (string.IsNullOrWhiteSpace(id))
            {

                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Lỗi!!! id sai ");
            }
            _mapper.Map(incomingdocumentView, existingDocument);

            _unitOfWork.GetRepository<IncomingDocument>().Update(existingDocument);

            await _unitOfWork.SaveAsync();
        }
    }
}
