using AutoMapper;
using DoAnChuyenNganh.Contract.Repositories.Entity;
using DoAnChuyenNganh.Contract.Repositories.Interface;
using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.Core.Store;
using DoAnChuyenNganh.Core.Utils;
using DoAnChuyenNganh.ModelViews.AlumniActivitiesModelViews;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using DoAnChuyenNganh.Contract.Services.Interface;

namespace DoAnChuyenNganh.Services.Service
{
    public class AlumniActivitiesService: IAlumniActivitiesService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AlumniActivitiesService(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateAlumniActivities(AlumniActivitiesModelView alumniActivitiesModelView)
        {
            string? UserId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            AlumniActivities newAlumniActivities = _mapper.Map<AlumniActivities>(alumniActivitiesModelView);
            newAlumniActivities.CreatedTime = CoreHelper.SystemTimeNow;
            newAlumniActivities.DeletedTime = null;
            newAlumniActivities.CreatedBy = UserId;
            await _unitOfWork.GetRepository<AlumniActivities>().InsertAsync(newAlumniActivities);
            await _unitOfWork.SaveAsync();
        }


        private async Task<BasePaginatedList<AlumniActivitiesResponseDTO>> PaginateAlumniActivities(
        IQueryable<AlumniActivities> query,
        int? pageIndex,
        int? pageSize)
        {
            int currentPage = pageIndex ?? 1;
            int currentPageSize = pageSize ?? 10;
            int totalItems = await query.CountAsync();

            List<AlumniActivitiesResponseDTO>? alumniActivities = await query
                .Skip((currentPage - 1) * currentPageSize)
                .Take(currentPageSize)
                .Select(alumniActiviti => new AlumniActivitiesResponseDTO
                {
                    Id = alumniActiviti.Id,
                    ActivitiesId = alumniActiviti.ActivitiesId,
                    AlumniId = alumniActiviti.AlumniId,
                    CreatedBy = alumniActiviti.CreatedBy,
                    LastUpdatedBy = alumniActiviti.LastUpdatedBy,
                    CreatedTime = alumniActiviti.CreatedTime,
                    LastUpdatedTime = alumniActiviti.LastUpdatedTime,
                })
                .ToListAsync();

            return new BasePaginatedList<AlumniActivitiesResponseDTO>(alumniActivities, totalItems, currentPage, currentPageSize);
        }
        public async Task<BasePaginatedList<AlumniActivitiesResponseDTO>> GetAlumniActivities(string? id, string? alumniId, string? activitiesId, int pageIndex, int pageSize)
        {
            IQueryable<AlumniActivities>? query = _unitOfWork.GetRepository<AlumniActivities>().Entities.Where(l => l.DeletedTime == null);

            if (!string.IsNullOrWhiteSpace(id))
            {
                query = query.Where(a => a.Id == id);
            }
            if (!string.IsNullOrWhiteSpace(alumniId))
            {
                query = query.Where(a => a.AlumniId == alumniId);
            }
            if (!string.IsNullOrWhiteSpace(activitiesId))
            {
                query = query.Where(a => a.ActivitiesId == activitiesId);
            }

            return await PaginateAlumniActivities(query, pageIndex, pageSize);
        }

        public async Task DeleteAlumniActivities(string id,string alumniId, string activitiesId)
        {
            string? UserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã cựu sinh viên!");
            }
            if (string.IsNullOrEmpty(activitiesId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã hoạt động cựu sinh viên!");
            }
            if (string.IsNullOrEmpty(alumniId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã cựu sinh viên!");
            }
            AlumniActivities? alumniActivities = await _unitOfWork.GetRepository<AlumniActivities>()
                .Entities.FirstOrDefaultAsync(a => a.Id == id && a.AlumniId == alumniId && a.ActivitiesId == activitiesId)
                ?? throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, $"Không tìm thấy hoạt động cựu sinh nào với mã {id}");
            if (alumniActivities.DeletedTime != null)
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, "Hoạt động cựu sinh viên đã bị xóa!");
            }
            alumniActivities.DeletedBy = UserId;
            alumniActivities.DeletedTime = CoreHelper.SystemTimeNow;
            await _unitOfWork.GetRepository<AlumniActivities>().UpdateAsync(alumniActivities);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateAlumniActivities(string id, string alumiId, string activitiesId, AlumniActivitiesModelView alumniActivitiesModelView)
        {
            string? UserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã hoạt động cựu sinh viên!");
            }
            if (string.IsNullOrEmpty(activitiesId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã hoạt động!");
            }
            if (string.IsNullOrEmpty(alumiId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã cựu sinh viên!");
            }
            AlumniActivities? oldAlumniActivities = await _unitOfWork.GetRepository<AlumniActivities>()
                .Entities.FirstOrDefaultAsync(a => a.Id == id && a.AlumniId == alumiId && a.ActivitiesId == activitiesId)
                ?? throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, $"Không tìm thấy hoạt động cựu sinh viên nào với mã {id}");
            await _unitOfWork.GetRepository<AlumniActivities>().DeleteAsync(id, alumiId, activitiesId);
            AlumniActivities newAlumniActivities = new AlumniActivities
            {
                AlumniId = alumniActivitiesModelView.AlumniId,
                ActivitiesId = alumniActivitiesModelView.ActivitiesId,
                CreatedBy = UserId,
                LastUpdatedBy = UserId,
                CreatedTime = CoreHelper.SystemTimeNow,
                LastUpdatedTime = CoreHelper.SystemTimeNow
            };
            await _unitOfWork.GetRepository<AlumniActivities>().InsertAsync(newAlumniActivities);
            await _unitOfWork.SaveAsync();
        }
    }
}
