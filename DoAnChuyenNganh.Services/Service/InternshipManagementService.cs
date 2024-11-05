using AutoMapper;
using DoAnChuyenNganh.Contract.Repositories.Entity;
using DoAnChuyenNganh.Contract.Repositories.Interface;
using DoAnChuyenNganh.Contract.Services.Interface;
using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.Core.Store;
using DoAnChuyenNganh.Core.Utils;
using DoAnChuyenNganh.ModelViews.InternshipMangamentModelViews;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DoAnChuyenNganh.Services.Service
{
    public class InternshipManagementService : IInternshipManagementService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public InternshipManagementService(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task CreateInternshipManagement(InternshipManagementModelView internshipManagementModelView)
        {
            string? UserId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            InternshipManagement internshipManagement = _mapper.Map<InternshipManagement>(internshipManagementModelView);
            internshipManagement.CreatedBy = UserId;
            internshipManagement.DeletedTime = null;
            internshipManagement.CreatedTime = CoreHelper.SystemTimeNow;
            await _unitOfWork.GetRepository<InternshipManagement>().InsertAsync(internshipManagement);
            await _unitOfWork.SaveAsync();

        }
        public async Task DeleteInternshipManagement(string id, string studentId, string businessId)
        {
            string? UserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã thực tập");
            }
            if (string.IsNullOrEmpty(studentId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã sinh viên!");
            }
            if (string.IsNullOrEmpty(businessId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã doanh nghiệp!");
            }
            InternshipManagement? internshipManagement = await _unitOfWork.GetRepository<InternshipManagement>()
                .Entities.FirstOrDefaultAsync(i => i.Id == id && i.StudentId == studentId && i.BusinessId == businessId)
                ?? throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, $"Không tìm thấy thông tin thực tập nào với mã {id}!");
            if (internshipManagement.DeletedTime != null)
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, "Thông tin thực tập đã bị xóa!");
            }
            internshipManagement.DeletedBy = UserId;
            internshipManagement.DeletedTime = CoreHelper.SystemTimeNow;
            await _unitOfWork.GetRepository<InternshipManagement>().UpdateAsync(internshipManagement);
            await _unitOfWork.SaveAsync();
        }
        public async Task UpdateInternshipManagement(string id, string studentId, string businessId, InternshipManagementModelView internshipManagementModelView)
        {
            string? UserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã thực tập");
            }
            if (string.IsNullOrEmpty(studentId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã sinh viên!");
            }
            if (string.IsNullOrEmpty(businessId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã doanh nghiệp!");
            }
            InternshipManagement? oldInternshipManagement = await _unitOfWork.GetRepository<InternshipManagement>()
                .Entities.FirstOrDefaultAsync(i => i.Id == id && i.StudentId == studentId && i.BusinessId == businessId)
                ?? throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, $"Không tìm thấy thông tin thực tập nào với mã {id}!");
            await _unitOfWork.GetRepository<InternshipManagement>().DeleteAsync(id, studentId, businessId);
            InternshipManagement newInternshipManagement = new InternshipManagement
            {
                StudentId = internshipManagementModelView.StudentId,
                BusinessId = internshipManagementModelView.StudentId,
                StartDate = internshipManagementModelView.StartDate,
                EndDate = internshipManagementModelView.EndDate,
                Remark = internshipManagementModelView.Remark,
                Rating = internshipManagementModelView.Rating,
                CreatedBy = UserId,
                CreatedTime = CoreHelper.SystemTimeNow,
                LastUpdatedBy = UserId,
                LastUpdatedTime = CoreHelper.SystemTimeNow
            };
            await _unitOfWork.GetRepository<InternshipManagement> ().InsertAsync(newInternshipManagement);
            await _unitOfWork.SaveAsync();
        }
        private async Task<BasePaginatedList<InternshipManagementResponseDTO>> PaginatedInternshipManagement(
        IQueryable<InternshipManagement> query,
        int? pageIndex,
        int? pageSize)
        {
            int currentPage = pageIndex ?? 1;
            int currentPageSize = pageSize ?? 10;
            int totalItems = await query.CountAsync();

            List<InternshipManagementResponseDTO>? internshipManagements = await query
                .Skip((currentPage - 1) * currentPageSize)
                .Take(currentPageSize)
                .Select(internship => new InternshipManagementResponseDTO
                {
                    Id = internship.Id,
                    StudentId = internship.StudentId,
                    BusinessId = internship.BusinessId,
                    StartDate = internship.StartDate,
                    EndDate = internship.EndDate,
                    Remark = internship.Remark,
                    Rating = internship.Rating,
                    //CreatedBy = internship.CreatedBy,
                    //CreatedTime = internship.CreatedTime,
                    //LastUpdatedBy = internship.LastUpdatedBy,
                    //LastUpdatedTime = internship.LastUpdatedTime,
                })
                .ToListAsync();

            return new BasePaginatedList<InternshipManagementResponseDTO>(internshipManagements, currentPage, currentPageSize, totalItems);    
        }
        public async Task<BasePaginatedList<InternshipManagementResponseDTO>> GetInternshipManagements(string? id, string? studentId, string? businessId, int pageIndex, int pageSize)
        {
            IQueryable<InternshipManagement>? query = _unitOfWork.GetRepository<InternshipManagement>().Entities.Where(i => i.DeletedTime == null);
            if (!string.IsNullOrWhiteSpace(id))
            {
                query = query.Where(i => i.Id == id);
            }
            if (!string.IsNullOrWhiteSpace(businessId))
            {
                query = query.Where(i => i.BusinessId == businessId);
            }
            if (!string.IsNullOrEmpty(studentId))
            {
                query = query.Where(i => i.StudentId == studentId);
            }
            return await PaginatedInternshipManagement(query, pageIndex, pageSize);
        }
    }
}
