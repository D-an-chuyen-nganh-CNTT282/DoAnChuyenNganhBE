using AutoMapper;
using DoAnChuyenNganh.Contract.Repositories.Entity;
using DoAnChuyenNganh.Contract.Repositories.Interface;
using DoAnChuyenNganh.Contract.Services.Interface;
using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.Core.Store;
using DoAnChuyenNganh.Core.Utils;
using DoAnChuyenNganh.ModelViews.ActivitiesModelViews;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DoAnChuyenNganh.Services.Service
{
    public class ActivitiesService : IActivitiesService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ActivitiesService(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task CreateActivities(ActivitiesModelView activitiesModelView)
        {
            string? UserId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            Activities activities = _mapper.Map<Activities>(activitiesModelView);
            activities.CreatedTime = CoreHelper.SystemTimeNow;
            activities.DeletedTime = null;
            //activities.CreatedBy = UserId;
            await _unitOfWork.GetRepository<Activities>().InsertAsync(activities);
            await _unitOfWork.SaveAsync();
        }
        public async Task DeleteActivities(string id)
        {
            string? UserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã hoạt động!");
            }
            Activities? activities = await _unitOfWork.GetRepository<Activities>().GetByIdAsync(id)
                ?? throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, $"Không tìm thấy hoạt động nào với mã {id}!");
            if (activities.DeletedTime != null)
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, "Hoạt động đã bị xóa!");
            }
            activities.DeletedBy = UserId;
            activities.DeletedTime = CoreHelper.SystemTimeNow;
            await _unitOfWork.GetRepository<Activities>().UpdateAsync(activities);
            await _unitOfWork.SaveAsync();
        }
        public async Task UpdateActivities(string id, ActivitiesModelView activitiesModelView)
        {
            string? UserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã hoạt động!");
            }
            Activities? activities = await _unitOfWork.GetRepository<Activities>().GetByIdAsync(id)
                ?? throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, $"Không tìm thấy hoạt động nào với mã {id}!");
            _mapper.Map(activitiesModelView, activities);
            activities.LastUpdatedTime = CoreHelper.SystemTimeNow;
            activities.LastUpdatedBy = UserId;
            await _unitOfWork.GetRepository<Activities>().UpdateAsync(activities);
            await _unitOfWork.SaveAsync();
        }
        private async Task<BasePaginatedList<ActivitiesResponseDTO>> PaginateActivities(
        IQueryable<Activities> query,
        int? pageIndex,
        int? pageSize)
        {
            int currentPage = pageIndex ?? 1;
            int currentPageSize = pageSize ?? 10;
            int totalItems = await query.CountAsync();

            List<ActivitiesResponseDTO>? activities = await query
                .Skip((currentPage - 1) * currentPageSize)
                .Take(currentPageSize)
                .Select(activity => new ActivitiesResponseDTO
                {
                    Id = activity.Id,
                    Name = activity.Name,
                    EventDate = activity.EventDate,
                    Location = activity.Location,
                    EventTypes = activity.EventTypes.ToString(),
                    Description = activity.Description,
                    //CreatedBy = activity.CreatedBy,
                    //LastUpdatedBy = activity.LastUpdatedBy,
                    //CreatedTime = activity.CreatedTime,
                    //LastUpdatedTime = activity.LastUpdatedTime,
                })
                .ToListAsync();

            return new BasePaginatedList<ActivitiesResponseDTO>(activities, totalItems, currentPage, currentPageSize);
        }
        public async Task<BasePaginatedList<ActivitiesResponseDTO>> GetActivities(string? id, string? name, int pageIndex, int pageSize)
        {
            IQueryable<Activities>? query = _unitOfWork.GetRepository<Activities>().Entities.Where(l => l.DeletedTime == null);
            if (!string.IsNullOrWhiteSpace(id))
            {
                query = query.Where(a => a.Id == id);
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(a => a.Name == name);
            }
            return await PaginateActivities(query, pageIndex, pageSize);
        }
    }
}

