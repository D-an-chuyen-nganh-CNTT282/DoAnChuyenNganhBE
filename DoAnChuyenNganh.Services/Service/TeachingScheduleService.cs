using AutoMapper;
using DoAnChuyenNganh.Contract.Repositories.Entity;
using DoAnChuyenNganh.Contract.Repositories.Interface;
using DoAnChuyenNganh.Contract.Services.Interface;
using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.Core.Store;
using DoAnChuyenNganh.Core.Utils;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using DoAnChuyenNganh.ModelViews.TeachingScheduleModelViews;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DoAnChuyenNganh.Services.Service
{
    public class TeachingScheduleService : ITeachingScheduleService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TeachingScheduleService(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task CreateTeachingSchedule(TeachingScheduleModelView teachingScheduleModelView)
        {
            string? UserId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            TeachingSchedule teachingSchedule = _mapper.Map<TeachingSchedule>(teachingScheduleModelView);
            teachingSchedule.CreatedTime = CoreHelper.SystemTimeNow;
            teachingSchedule.DeletedTime = null;
            teachingSchedule.CreatedBy = UserId;
            teachingSchedule.UserId = Guid.Parse(UserId);
            await _unitOfWork.GetRepository<TeachingSchedule>().InsertAsync(teachingSchedule);
            await _unitOfWork.SaveAsync();
        }
        public async Task DeleteTeachingSchedule(string id)
        {
            string? UserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã lịch giảng dạy!");
            }
            TeachingSchedule? teachingSchedule = await _unitOfWork.GetRepository<TeachingSchedule>().GetByIdAsync(id)
                ?? throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, $"Không tìm thấy lịch giảng dạy nào với mã {id}!");
            if (teachingSchedule.DeletedTime != null)
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, "Lịch giảng dạy đã bị xóa!");
            }
            teachingSchedule.DeletedBy = UserId;
            teachingSchedule.DeletedTime = CoreHelper.SystemTimeNow;
            await _unitOfWork.GetRepository<TeachingSchedule>().UpdateAsync(teachingSchedule);
            await _unitOfWork.SaveAsync();
        }
        public async Task UpdateTeachingSchedule(string id, TeachingScheduleModelView teachingScheduleModelView)
        {
            string? UserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã lịch giảng dạy!");
            }
            TeachingSchedule? teachingSchedule = await _unitOfWork.GetRepository<TeachingSchedule>().GetByIdAsync(id)
                ?? throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, $"Không tìm thấy lịch giảng dạy nào với mã {id}!");
            _mapper.Map(teachingScheduleModelView, teachingSchedule);
            teachingSchedule.LastUpdatedTime = CoreHelper.SystemTimeNow;
            teachingSchedule.LastUpdatedBy = UserId;
            await _unitOfWork.GetRepository<TeachingSchedule>().UpdateAsync(teachingSchedule);
            await _unitOfWork.SaveAsync();
        }
        private async Task<BasePaginatedList<TeachingScheduleResponseDTO>> PaginateTeachingSchedule(
        IQueryable<TeachingSchedule> query,
        int? pageIndex,
        int? pageSize)
        {
            int currentPage = pageIndex ?? 1;
            int currentPageSize = pageSize ?? 10;
            int totalItems = await query.CountAsync();

            List<TeachingScheduleResponseDTO>? teachingSchedules = await query
                .Skip((currentPage - 1) * currentPageSize)
                .Take(currentPageSize)
                .Select(teachingSchedule => new TeachingScheduleResponseDTO
                {
                    Id = teachingSchedule.Id,
                    LecturerId = teachingSchedule.LecturerId,
                    StartDate = teachingSchedule.StartDate,
                    EndDate = teachingSchedule.EndDate,
                    Subject = teachingSchedule.Subject,
                    Location = teachingSchedule.Location,
                    ClassPeriod = teachingSchedule.ClassPeriod,
                    CreatedBy = teachingSchedule.CreatedBy,
                    LastUpdatedBy = teachingSchedule.LastUpdatedBy,
                    CreatedTime = teachingSchedule.CreatedTime,
                    LastUpdatedTime = teachingSchedule.LastUpdatedTime,
                })
                .ToListAsync();

            return new BasePaginatedList<TeachingScheduleResponseDTO>(teachingSchedules, totalItems, currentPage, currentPageSize);
        }
        public async Task<BasePaginatedList<TeachingScheduleResponseDTO>> GetTeachingSchedules(string? id, string? subject, int pageIndex, int pageSize)
        {
            IQueryable<TeachingSchedule>? query = _unitOfWork.GetRepository<TeachingSchedule>().Entities.Where(l => l.DeletedTime == null);
            if (!string.IsNullOrWhiteSpace(id))
            {
                query = query.Where(t => t.Id == id);
            }
            if (!string.IsNullOrWhiteSpace(subject))
            {
                query = query.Where(t => t.Subject == subject);
            }
            return await PaginateTeachingSchedule(query, pageIndex, pageSize);
        }
    }
}
