using AutoMapper;
using DoAnChuyenNganh.Contract.Repositories.Entity;
using DoAnChuyenNganh.Contract.Repositories.Interface;
using DoAnChuyenNganh.Contract.Services.Interface;
using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.Core.Store;
using DoAnChuyenNganh.Core.Utils;
using DoAnChuyenNganh.ModelViews.LecturerPlanModelViews;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DoAnChuyenNganh.Services.Service
{
    public class LecturerPlanService : ILecturerPlanService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public LecturerPlanService(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task CreateLecturerPlan(LecturerPlanModelView lecturerPlanModelView)
        {
            string? UserId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            string? email = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.Email).Value;
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            Lecturer? lecturer = await _unitOfWork.GetRepository<Lecturer>()
                .Entities
                .FirstOrDefaultAsync(l => l.LecturerEmail == email)
                ?? throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, "Không tìm thấy giảng viên tương ứng với tài khoản email này!");
            lecturerPlanModelView.LecturerId = lecturer.Id;
            LecturerPlan lecturerPlan = _mapper.Map<LecturerPlan>(lecturerPlanModelView);
            lecturerPlan.CreatedBy = UserId;
            lecturerPlan.CreatedTime = CoreHelper.SystemTimeNow;
            lecturerPlan.UserId = Guid.Parse(UserId);
            await _unitOfWork.GetRepository<LecturerPlan>().InsertAsync(lecturerPlan);
            await _unitOfWork.SaveAsync();
        }
        public async Task DeleteLecturerPlan(string id)
        {
            string? UserId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            string? email = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.Email).Value;
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            Lecturer? lecturer = await _unitOfWork.GetRepository<Lecturer>()
                .Entities
                .FirstOrDefaultAsync(l => l.LecturerEmail == email)
                ?? throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, "Không tìm thấy giảng viên tương ứng với tài khoản email này!");
            LecturerPlan? lecturerPlan = await _unitOfWork.GetRepository<LecturerPlan>().GetByIdAsync(id)
                ?? throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, $"Không tìm thấy kế hoạch nào với mã {id}!");
            if (lecturerPlan.LecturerId != lecturer.Id)
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Bạn không có quyền truy cập kế hoạch này!");
            }
            if (lecturerPlan.DeletedTime != null)
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, "Kế hoạch đã bị xóa!");
            }    
            lecturerPlan.DeletedBy = UserId;
            lecturerPlan.DeletedTime = CoreHelper.SystemTimeNow;
            await _unitOfWork.GetRepository<LecturerPlan>().UpdateAsync(lecturerPlan);
            await _unitOfWork.SaveAsync();
        }
        public async Task UpdateLecturerPlan(string id, LecturerPlanModelView lecturerPlanModelView)
        {
            string? UserId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            string? email = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.Email).Value;
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            Lecturer? lecturer = await _unitOfWork.GetRepository<Lecturer>()
                .Entities
                .FirstOrDefaultAsync(l => l.LecturerEmail == email)
                ?? throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, "Không tìm thấy giảng viên tương ứng với tài khoản email này!");
            LecturerPlan? lecturerPlan = await _unitOfWork.GetRepository<LecturerPlan>().GetByIdAsync(id)
                ?? throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, $"Không tìm thấy kế hoạch nào với mã {id}!");
            if (lecturerPlan.LecturerId != lecturer.Id)
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Bạn không có quyền truy cập kế hoạch này!");
            }
            _mapper.Map(lecturerPlanModelView, lecturerPlan);
            lecturerPlan.LastUpdatedBy = UserId;
            lecturerPlan.LastUpdatedTime = CoreHelper.SystemTimeNow;
            lecturerPlan.UserId = Guid.Parse(UserId);
            lecturerPlan.LecturerId = lecturer.Id;
            await _unitOfWork.GetRepository<LecturerPlan>().UpdateAsync(lecturerPlan);
            await _unitOfWork.SaveAsync();
        }
        private async Task<BasePaginatedList<LecturerPlanResponseDTO>> PaginateLecturerPlans(
        IQueryable<LecturerPlan> query,
        int? pageIndex,
        int? pageSize)
        {
            int currentPage = pageIndex ?? 1;
            int currentPageSize = pageSize ?? 10;
            int totalItems = await query.CountAsync();

            List<LecturerPlanResponseDTO>? lecturerPlans = await query
                .Skip((currentPage - 1) * currentPageSize)
                .Take(currentPageSize)
                .Select(lecturerPlan => new LecturerPlanResponseDTO
                {
                    Id = lecturerPlan.Id,
                    PlanContent = lecturerPlan.PlanContent,
                    StartDate = lecturerPlan.StartDate,
                    EndDate = lecturerPlan.EndDate,
                    Note = lecturerPlan.Note,
                    //CreatedBy = lecturerPlan.CreatedBy,
                    //LastUpdatedBy = lecturerPlan.LastUpdatedBy,
                    //CreatedTime = lecturerPlan.CreatedTime,
                    //LastUpdatedTime = lecturerPlan.LastUpdatedTime,
                })
                .ToListAsync();

            return new BasePaginatedList<LecturerPlanResponseDTO>(lecturerPlans, totalItems, currentPage, currentPageSize);
        }

        public async Task<BasePaginatedList<LecturerPlanResponseDTO>> GetLecturerPlans(string? id, int pageIndex, int pageSize)
        {
            string? email = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.Email).Value;
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            Lecturer? lecturer = await _unitOfWork.GetRepository<Lecturer>()
                .Entities
                .FirstOrDefaultAsync(l => l.LecturerEmail == email)
                ?? throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, "Không tìm thấy giảng viên tương ứng với tài khoản email này!");
            IQueryable<LecturerPlan>? query = _unitOfWork.GetRepository<LecturerPlan>().Entities
                .Where(p => p.LecturerId == lecturer.Id && p.DeletedTime == null);
            if (!string.IsNullOrWhiteSpace(id))
            {
                query = query.Where(lecturerPlan => lecturerPlan.Id == id);
            }
            return await PaginateLecturerPlans(query, pageIndex, pageSize);
        }
    }
}
