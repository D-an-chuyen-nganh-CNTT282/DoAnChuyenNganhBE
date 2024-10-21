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
using AutoMapper.QueryableExtensions;
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
            string UserId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }

            AlumniActivities alumniActivities = _mapper.Map<AlumniActivities>(alumniActivitiesModelView);
            alumniActivities.CreatedBy = UserId;
            alumniActivities.CreatedTime = CoreHelper.SystemTimeNow;
            
            await _unitOfWork.GetRepository<AlumniActivities>().InsertAsync(alumniActivities);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateAlumniActivities(string id, AlumniActivitiesModelView alumniActivitiesModelView)
        {
            // Get the current user ID from the HttpContext
            string? UserId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }

            // Validate the input parameters
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã hoạt động cựu sinh viên!");
            }

            // Find the existing AlumniActivities entry
            AlumniActivities? oldAlumniActivities = await _unitOfWork.GetRepository<AlumniActivities>()
                .Entities.FirstOrDefaultAsync(a => a.Id == id && a.DeletedTime == null)
                ?? throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, $"Không tìm thấy hoạt động nào với mã {id}");

            // Use AutoMapper to map the updated values from the model view
            _mapper.Map(alumniActivitiesModelView, oldAlumniActivities);

            // Set the fields that should be updated manually (like LastUpdatedBy and LastUpdatedTime)
            oldAlumniActivities.LastUpdatedBy = UserId;
            oldAlumniActivities.LastUpdatedTime = CoreHelper.SystemTimeNow;

            // Update the entity in the database
            await _unitOfWork.GetRepository<AlumniActivities>().UpdateAsync(oldAlumniActivities);
            await _unitOfWork.SaveAsync();
        }


        public async Task<BasePaginatedList<AlumniActivitiesResponseDTO>> GetAlumniActivities(string id, string? alumniId, string? activitiesId, int pageIndex, int pageSize)
        {
            IQueryable<AlumniActivities>? query = _unitOfWork.GetRepository<AlumniActivities>()
                .Entities.Where(a => a.DeletedTime == null);

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

            int totalItems = await query.CountAsync();

            List<AlumniActivitiesResponseDTO>? alumniActivitiesDTOs = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<AlumniActivitiesResponseDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return new BasePaginatedList<AlumniActivitiesResponseDTO>(alumniActivitiesDTOs, totalItems, pageIndex, pageSize);
        }

    }
}
