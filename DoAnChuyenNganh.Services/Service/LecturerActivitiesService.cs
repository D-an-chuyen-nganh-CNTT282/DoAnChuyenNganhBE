using AutoMapper;
using DoAnChuyenNganh.Contract.Repositories.Entity;
using DoAnChuyenNganh.Contract.Repositories.Interface;
using DoAnChuyenNganh.Contract.Services.Interface;
using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.Core.Store;
using DoAnChuyenNganh.Core.Utils;
using DoAnChuyenNganh.ModelViews.LecturerActivitiesModelViews;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DoAnChuyenNganh.Services.Service
{
    public class LecturerActivitiesService : ILecturerActivitiesService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public LecturerActivitiesService(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task CreateLecturerActivities(LecturerActivitiesModelView lecturerActivitiesModelView)
        {
            string? UserId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            LecturerActivities lecturerActivities = _mapper.Map<LecturerActivities>(lecturerActivitiesModelView);
            lecturerActivities.CreatedBy = UserId;
            lecturerActivities.CreatedTime = CoreHelper.SystemTimeNow;
            await _unitOfWork.GetRepository<LecturerActivities>().InsertAsync(lecturerActivities);
            await _unitOfWork.SaveAsync();
        }
        public async Task UpdateLecturerActivities(string id, string lecturerId, string activitiesId, LecturerActivitiesModelView lecturerActivitiesModelView)
        {
            string? UserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã hoạt động giảng viên!");
            }
            if (string.IsNullOrEmpty(activitiesId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã hoạt động!");
            }
            if (string.IsNullOrEmpty(lecturerId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã giảng viên!");
            }
            LecturerActivities? oldLecturerActivities = await _unitOfWork.GetRepository<LecturerActivities>()
                .Entities.FirstOrDefaultAsync(a => a.Id == id && a.LecturerId == lecturerId && a.ActivitiesId == activitiesId)
                ?? throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, $"Không tìm thấy hoạt động giảng viên nào với mã {id}");
            await _unitOfWork.GetRepository<LecturerActivities>().DeleteAsync(id, lecturerId, activitiesId);
            LecturerActivities newLecturerActivities = new LecturerActivities
            {
                LecturerId = lecturerActivitiesModelView.LecturerId,
                ActivitiesId = lecturerActivitiesModelView.ActivitiesId,
                CreatedBy = UserId,
                LastUpdatedBy = UserId,
                CreatedTime = CoreHelper.SystemTimeNow,
                LastUpdatedTime = CoreHelper.SystemTimeNow
            };
            await _unitOfWork.GetRepository<LecturerActivities>().InsertAsync(newLecturerActivities);
            await _unitOfWork.SaveAsync();
        }
        public async Task DeleteLecturerActivities(string id, string lecturerId, string activitiesId)
        {
            string? UserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã hoạt động giảng viên!");
            }
            if (string.IsNullOrEmpty(activitiesId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã hoạt động!");
            }
            if (string.IsNullOrEmpty(lecturerId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã giảng viên!");
            }
            LecturerActivities? lecturerActivities = await _unitOfWork.GetRepository<LecturerActivities>()
                .Entities.FirstOrDefaultAsync(a => a.Id == id && a.LecturerId == lecturerId && a.ActivitiesId == activitiesId)
                ?? throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, $"Không tìm thấy hoạt động giảng viên nào với mã {id}");
            if (lecturerActivities.DeletedTime != null)
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, "Hoạt động giảng viên đã bị xóa!");
            }
            lecturerActivities.DeletedBy = UserId;
            lecturerActivities.DeletedTime = CoreHelper.SystemTimeNow;
            await _unitOfWork.GetRepository<LecturerActivities>().UpdateAsync(lecturerActivities);
            await _unitOfWork.SaveAsync();
        }
        private async Task<BasePaginatedList<LecturerActivitiesResponseDTO>> PaginateLecturerActivities(
        IQueryable<LecturerActivities> query,
        int? pageIndex,
        int? pageSize)
        {
            int currentPage = pageIndex ?? 1;
            int currentPageSize = pageSize ?? 10;
            int totalItems = await query.CountAsync();

            List<LecturerActivitiesResponseDTO>? lecturerActivities = await query
                .Skip((currentPage - 1) * currentPageSize)
                .Take(currentPageSize)
                .Select(activities => new LecturerActivitiesResponseDTO
                {
                    Id = activities.Id,
                    LecturerId = activities.LecturerId,
                    ActivitiesId = activities.ActivitiesId,
                    //CreatedBy = activities.CreatedBy,
                    //LastUpdatedBy = activities.LastUpdatedBy,
                    //CreatedTime = activities.CreatedTime,
                    //LastUpdatedTime = activities.LastUpdatedTime,
                })
                .ToListAsync();

            return new BasePaginatedList<LecturerActivitiesResponseDTO>(lecturerActivities, totalItems, currentPage, currentPageSize);
        }
        public async Task<BasePaginatedList<LecturerActivitiesResponseDTO>> GetLecturerActivities(string? id, string? lecturerId, string? activitiesId, int pageIndex, int pageSize)
        {
            IQueryable<LecturerActivities>? query = _unitOfWork.GetRepository<LecturerActivities>().Entities.Where(l => l.DeletedTime == null);
            if (!string.IsNullOrWhiteSpace(id))
            {
                query = query.Where(a => a.Id == id);
            }
            if (!string.IsNullOrWhiteSpace(lecturerId))
            {
                query = query.Where(a => a.LecturerId == lecturerId);
            }
            if (!string.IsNullOrEmpty(activitiesId))
            {
                query = query.Where(a => a.ActivitiesId == activitiesId);
            }
            return await PaginateLecturerActivities(query, pageIndex, pageSize);
        }
    }
}
