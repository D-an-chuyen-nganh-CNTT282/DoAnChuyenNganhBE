using AutoMapper;
using DoAnChuyenNganh.Contract.Repositories.Entity;
using DoAnChuyenNganh.Contract.Repositories.Interface;
using DoAnChuyenNganh.Contract.Services.Interface;
using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.Core.Store;
using DoAnChuyenNganh.Core.Utils;
using DoAnChuyenNganh.ModelViews.BusinessActivitiesModelViews;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DoAnChuyenNganh.Services.Service
{
    public class BusinessActivitiesService : IBusinessActivitesService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public BusinessActivitiesService(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task CreateBusinessActivities(BusinessActivitiesModelView businessActivitiesModelView)
        {
            string? UserId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            BusinessActivities businessActivities = _mapper.Map<BusinessActivities>(businessActivitiesModelView);
            businessActivities.CreatedBy = UserId;
            businessActivities.CreatedTime = CoreHelper.SystemTimeNow;
            await _unitOfWork.GetRepository<BusinessActivities>().InsertAsync(businessActivities);
            await _unitOfWork.SaveAsync();
        }
        public async Task UpdateBusinessActivities(string id, string businessId, string activitiesId, BusinessActivitiesModelView businessActivitiesModelView)
        {
            string? UserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã hoạt động doanh nghiệp!");
            }
            if (string.IsNullOrEmpty(activitiesId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã hoạt động!");
            }
            if (string.IsNullOrEmpty(businessId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã doanh nghiệp!");
            }
            BusinessActivities? oldBusinessActivities = await _unitOfWork.GetRepository<BusinessActivities>()
                .Entities.FirstOrDefaultAsync(a => a.Id == id && a.BusinessId == businessId && a.ActivitiesId == activitiesId)
                ?? throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, $"Không tìm thấy hoạt động doanh nghiệp nào với mã {id}");
            await _unitOfWork.GetRepository<BusinessActivities>().DeleteAsync(id, businessId, activitiesId);
            BusinessActivities newBusinessActivities = new BusinessActivities
            {
                BusinessId = businessActivitiesModelView.BusinessId,
                ActivitiesId = businessActivitiesModelView.ActivitiesId,
                CreatedBy = UserId,
                LastUpdatedBy = UserId,
                CreatedTime = CoreHelper.SystemTimeNow,
                LastUpdatedTime = CoreHelper.SystemTimeNow
            };
            await _unitOfWork.GetRepository<BusinessActivities>().InsertAsync(newBusinessActivities);
            await _unitOfWork.SaveAsync();
        }
        public async Task DeleteBusinessActivities(string id, string businessId, string activitiesId)
        {
            string? UserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã hoạt động doanh nghiệp!");
            }
            if (string.IsNullOrEmpty(activitiesId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã hoạt động!");
            }
            if (string.IsNullOrEmpty(businessId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã doanh nghiệp!");
            }
            BusinessActivities? businessActivities = await _unitOfWork.GetRepository<BusinessActivities>()
                .Entities.FirstOrDefaultAsync(a => a.Id == id && a.BusinessId == businessId && a.ActivitiesId == activitiesId)
                ?? throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, $"Không tìm thấy hoạt động doanh nghiêp nào với mã {id}");
            if (businessActivities.DeletedTime != null)
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, "Hoạt động giảng viên đã bị xóa!");
            }
            businessActivities.DeletedBy = UserId;
            businessActivities.DeletedTime = CoreHelper.SystemTimeNow;
            await _unitOfWork.GetRepository<BusinessActivities>().UpdateAsync(businessActivities);
            await _unitOfWork.SaveAsync();
        }
        private async Task<BasePaginatedList<BusinessActivitiesResponseDTO>> PaginateBusinessActivities(
       IQueryable<BusinessActivities> query,
       int? pageIndex,
       int? pageSize)
        {
            int currentPage = pageIndex ?? 1;
            int currentPageSize = pageSize ?? 10;
            int totalItems = await query.CountAsync();

            List<BusinessActivitiesResponseDTO>? businessActivities = await query
                .Skip((currentPage - 1) * currentPageSize)
                .Take(currentPageSize)
                .Select(activities => new BusinessActivitiesResponseDTO
                {
                    Id = activities.Id,
                    BusinessId = activities.BusinessId,
                    ActivitiesId = activities.ActivitiesId,
                    CreatedBy = activities.CreatedBy,
                    LastUpdatedBy = activities.LastUpdatedBy,
                    CreatedTime = activities.CreatedTime,
                    LastUpdatedTime = activities.LastUpdatedTime,
                })
                .ToListAsync();

            return new BasePaginatedList<BusinessActivitiesResponseDTO>(businessActivities, totalItems, currentPage, currentPageSize);
        }
        public async Task<BasePaginatedList<BusinessActivitiesResponseDTO>> GetBusinessActivities(string? id, string? businessId, string? activitiesId, int pageIndex, int pageSize)
        {
            IQueryable<BusinessActivities>? query = _unitOfWork.GetRepository<BusinessActivities>().Entities.Where(b => b.DeletedTime == null);
            if (!string.IsNullOrWhiteSpace(id))
            {
                query = query.Where(a => a.Id == id);
            }
            if (!string.IsNullOrWhiteSpace(businessId))
            {
                query = query.Where(a => a.BusinessId == businessId);
            }
            if (!string.IsNullOrEmpty(activitiesId))
            {
                query = query.Where(a => a.ActivitiesId == activitiesId);
            }
            return await PaginateBusinessActivities(query, pageIndex, pageSize);
        }
    }
}
