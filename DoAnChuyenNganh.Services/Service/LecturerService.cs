using AutoMapper;
using DoAnChuyenNganh.Contract.Repositories.Entity;
using DoAnChuyenNganh.Contract.Repositories.Interface;
using DoAnChuyenNganh.Contract.Services.Interface;
using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.Core.Store;
using DoAnChuyenNganh.Core.Utils;
using DoAnChuyenNganh.ModelViews.LecturerModelViews;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DoAnChuyenNganh.Services.Service
{
    public class LecturerService : ILecturerService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public LecturerService(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task CreateLecturer(LecturerModelView lecturerModelView)
        {
            string? UserId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            Lecturer newLecturer = _mapper.Map<Lecturer>(lecturerModelView);
            newLecturer.CreatedTime = CoreHelper.SystemTimeNow;
            newLecturer.DeletedTime = null;
            newLecturer.CreatedBy = UserId;
            await _unitOfWork.GetRepository<Lecturer>().InsertAsync(newLecturer);
            await _unitOfWork.SaveAsync();
        }
        public async Task DeleteLecturer(string id)
        {
            string? UserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã giảng viên!");
            }
            Lecturer? lecturer = await _unitOfWork.GetRepository<Lecturer>().GetByIdAsync(id)
                ?? throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, $"Không tìm thấy giảng viên nào với mã {id}!");
            if (lecturer.DeletedTime != null)
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, "Thông tin giảng viên đã bị xóa!");
            }
            lecturer.DeletedBy = UserId;
            lecturer.DeletedTime = CoreHelper.SystemTimeNow;
            await _unitOfWork.GetRepository<Lecturer>().UpdateAsync(lecturer);
            await _unitOfWork.SaveAsync();
        }
        public async Task UpdateLecturer(string id, LecturerModelView lecturerModelView)
        {
            string? UserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã giảng viên!");
            }
            Lecturer? lecturer = await _unitOfWork.GetRepository<Lecturer>().GetByIdAsync(id)
                ?? throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, $"Không tìm thấy giảng viên nào với mã {id}!");
            _mapper.Map(lecturerModelView, lecturer);
            lecturer.LastUpdatedTime = CoreHelper.SystemTimeNow;
            lecturer.LastUpdatedBy = UserId;
            await _unitOfWork.GetRepository<Lecturer>().UpdateAsync(lecturer);
            await _unitOfWork.SaveAsync();
        }

        private async Task<BasePaginatedList<LecturerResponseDTO>> PaginateLecturers(
        IQueryable<Lecturer> query,
        int? pageIndex,
        int? pageSize)
        {
            int currentPage = pageIndex ?? 1;
            int currentPageSize = pageSize ?? 10;
            int totalItems = await query.CountAsync();

            List<LecturerResponseDTO>? lecturers = await query
                .Skip((currentPage - 1) * currentPageSize)
                .Take(currentPageSize)
                .Select(lecturer => new LecturerResponseDTO
                {
                    Id = lecturer.Id,
                    LecturerName = lecturer.LecturerName,
                    DayOfBirth = lecturer.DayOfBirth,
                    LecturerGender = lecturer.LecturerGender,
                    LecturerEmail = lecturer.LecturerEmail,
                    LecturerPhone = lecturer.LecturerPhone,
                    LecturerAddress = lecturer.LecturerAddress,
                    Expertise = lecturer.Expertise,
                    PersonalWebsiteLink = lecturer.PersonalWebsiteLink,
                    CreatedBy = lecturer.CreatedBy,
                    LastUpdatedBy = lecturer.LastUpdatedBy,
                    CreatedTime = lecturer.CreatedTime,
                    LastUpdatedTime = lecturer.LastUpdatedTime,
                })
                .ToListAsync();

            return new BasePaginatedList<LecturerResponseDTO>(lecturers, totalItems, currentPage, currentPageSize);
        }
        public async Task<BasePaginatedList<LecturerResponseDTO>> GetLecturers(string? id, string? name, int pageIndex, int pageSize)
        {
            IQueryable<Lecturer>? query = _unitOfWork.GetRepository<Lecturer>().Entities.Where(l => l.DeletedTime == null);
            if (!string.IsNullOrWhiteSpace(id))
            {
                query = query.Where(lecturer => lecturer.Id == id);
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(lecturer => lecturer.LecturerName == name);
            }
            return await PaginateLecturers(query, pageIndex, pageSize);
        }
    }
}
