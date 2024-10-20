using AutoMapper;
using DoAnChuyenNganh.Contract.Repositories.Entity;
using DoAnChuyenNganh.Contract.Repositories.Interface;
using DoAnChuyenNganh.Contract.Services.Interface;
using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.Core.Utils;
using DoAnChuyenNganh.ModelViews.AlumniModelViews;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using DoAnChuyenNganh.Core.Store;
using DoAnChuyenNganh.ModelViews.IncomingDocumentModelViews;

namespace DoAnChuyenNganh.Services.Service
{
    public class AlumniService : IAlumniService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AlumniService(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Create(AlumniModelView alumniModelView)
        {
            string? UserId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }

            Alumni newAlumni = _mapper.Map<Alumni>(alumniModelView);
            newAlumni.CreatedTime = CoreHelper.SystemTimeNow;
            newAlumni.CreatedBy = UserId;
            await _unitOfWork.GetRepository<Alumni>().InsertAsync(newAlumni);
            await _unitOfWork.SaveAsync();
        }

        public async Task Delete(string? id)
        {
            string? UserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }

            if (string.IsNullOrWhiteSpace(id))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã cựu sinh viên!");
            }

            Alumni? alumni = await _unitOfWork.GetRepository<Alumni>().GetByIdAsync(id)
                ?? throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, $"Không tìm thấy cựu sinh viên nào với mã {id}!");

            if (alumni.DeletedTime != null)
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, "Thông tin cựu sinh viên đã bị xóa!");
            }

            alumni.DeletedBy = UserId;
            alumni.DeletedTime = CoreHelper.SystemTimeNow;
            await _unitOfWork.GetRepository<Alumni>().UpdateAsync(alumni);
            await _unitOfWork.SaveAsync();
        }

        public async Task<BasePaginatedList<AlumniResponseDTO>> Get(string? id, string? alumniName, string? alumniMajor, string? alumniCourse, int pageSize, int pageIndex)
        {
            if (pageIndex == 0 && pageSize == 0)
            {
                pageSize = 5; // Default page size
                pageIndex = 1; // Default page index
            }

            IQueryable<Alumni> query = _unitOfWork.GetRepository<Alumni>().Entities.Where(a => a.DeletedTime == null);

            // Apply filters if any are provided
            if (!string.IsNullOrWhiteSpace(id))
            {
                query = query.Where(a => a.Id == id);
            }

            if (!string.IsNullOrWhiteSpace(alumniName))
            {
                query = query.Where(a => a.AlumniName.Contains(alumniName));
            }

            if (!string.IsNullOrWhiteSpace(alumniMajor))
            {
                query = query.Where(a => a.AlumniMajor.Contains(alumniMajor));
            }

            if (!string.IsNullOrWhiteSpace(alumniCourse))
            {
                query = query.Where(a => a.AlumniCourse.Contains(alumniCourse));
            }

            // Pagination and count
            int totalItems = await query.CountAsync();
            List<Alumni> paginatedAlumni = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Mapping to DTOs
            List<AlumniResponseDTO> alumniDTOs = _mapper.Map<List<AlumniResponseDTO>>(paginatedAlumni);

            return new BasePaginatedList<AlumniResponseDTO>(alumniDTOs, totalItems, pageIndex, pageSize);
        }


        public async Task Update(string? id, AlumniModelView alumniView)
        {
            string? UserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }

            if (string.IsNullOrWhiteSpace(id))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã cựu sinh viên!");
            }

            Alumni? alumni = await _unitOfWork.GetRepository<Alumni>().GetByIdAsync(id)
                ?? throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, $"Không tìm thấy cựu sinh viên nào với mã {id}!");

            _mapper.Map(alumniView, alumni); // Assuming the mapping exists between IncomingDocumentModelViews and Alumni
            alumni.LastUpdatedTime = CoreHelper.SystemTimeNow;
            alumni.LastUpdatedBy = UserId;
            await _unitOfWork.GetRepository<Alumni>().UpdateAsync(alumni);
            await _unitOfWork.SaveAsync();
        }

        
    }
}
