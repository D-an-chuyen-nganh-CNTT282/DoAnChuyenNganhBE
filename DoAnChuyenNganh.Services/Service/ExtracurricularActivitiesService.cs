using AutoMapper;
using DoAnChuyenNganh.Contract.Repositories.Entity;
using DoAnChuyenNganh.Contract.Repositories.Interface;
using DoAnChuyenNganh.Contract.Services.Interface;
using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.Core.Store;
using DoAnChuyenNganh.Core.Utils;
using DoAnChuyenNganh.ModelViews.ExtracurricularActivitiesModelViews;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace DoAnChuyenNganh.Services.Service
{
    public class ExtracurricularActivitiesService : IExtracurricularActivitiesService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ExtracurricularActivitiesService(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task CreateExtracurricularActivities(ExtracurricularActivitiesModelView extracurricularActivitiesModelView)
        {
            string? UserId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            ExtracurricularActivities extracurricularActivities = _mapper.Map<ExtracurricularActivities>(extracurricularActivitiesModelView);
            extracurricularActivities.CreatedBy = UserId;
            extracurricularActivities.CreatedTime = CoreHelper.SystemTimeNow;
            await _unitOfWork.GetRepository<ExtracurricularActivities>().InsertAsync(extracurricularActivities);
            await _unitOfWork.SaveAsync();
        }
        public async Task UpdateExtracurricularActivities(string id, string studentId, string activitiesId, ExtracurricularActivitiesModelView extracurricularActivitiesModelView)
        {
            string? UserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã hoạt động ngoại khóa!");
            }
            if (string.IsNullOrEmpty(activitiesId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã hoạt động!");
            }
            if (string.IsNullOrEmpty(studentId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã số sinh viên!");
            }
            ExtracurricularActivities? oldExtracurricularActivities = await _unitOfWork.GetRepository<ExtracurricularActivities>()
                .Entities.FirstOrDefaultAsync(a => a.Id == id && a.StudentId == studentId && a.ActivitiesId == activitiesId)
                ?? throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, $"Không tìm thấy hoạt động ngoại khóa nào với mã {id}");
            await _unitOfWork.GetRepository<ExtracurricularActivities>().DeleteAsync(id, studentId, activitiesId);
            ExtracurricularActivities newExtracurricularActivities = new ExtracurricularActivities
            {
                StudentId = extracurricularActivitiesModelView.StudentId,
                ActivitiesId = extracurricularActivitiesModelView.ActivitiesId,
                CreatedBy = UserId,
                LastUpdatedBy = UserId,
                CreatedTime = CoreHelper.SystemTimeNow,
                LastUpdatedTime = CoreHelper.SystemTimeNow
            };
            await _unitOfWork.GetRepository<ExtracurricularActivities>().InsertAsync(newExtracurricularActivities);
            await _unitOfWork.SaveAsync();
        }

        public async Task<BasePaginatedList<ExtracurricularActivitiesReponseDTO>> GetExtracurricularActivities(string? id, string? studentId, string? activitiesId, int pageIndex, int pageSize)
        {
            IQueryable<ExtracurricularActivities>? query = _unitOfWork.GetRepository<ExtracurricularActivities>().Entities.Where(b => b.DeletedTime == null);

            if (!string.IsNullOrWhiteSpace(id))
            {
                query = query.Where(a => a.Id == id);
            }
            if (!string.IsNullOrWhiteSpace(studentId))
            {
                query = query.Where(a => a.StudentId == studentId);
            }
            if (!string.IsNullOrEmpty(activitiesId))
            {
                query = query.Where(a => a.ActivitiesId == activitiesId);
            }

            int totalItems = await query.CountAsync();
            List<ExtracurricularActivitiesReponseDTO>? extracurricularActivities = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(activities => new ExtracurricularActivitiesReponseDTO
                {
                    Id = activities.Id,
                    StudentId = activities.StudentId,
                    ActivitiesId = activities.ActivitiesId,
                    CreatedBy = activities.CreatedBy,
                    LastUpdatedBy = activities.LastUpdatedBy,
                    CreatedTime = activities.CreatedTime,
                    LastUpdatedTime = activities.LastUpdatedTime,
                })
                .ToListAsync();

            return new BasePaginatedList<ExtracurricularActivitiesReponseDTO>(extracurricularActivities, totalItems, pageIndex, pageSize);
        }
    }
}