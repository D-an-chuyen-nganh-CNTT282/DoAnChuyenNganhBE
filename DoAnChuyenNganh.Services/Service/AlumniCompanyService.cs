using AutoMapper;
using DoAnChuyenNganh.Contract.Repositories.Entity;
using DoAnChuyenNganh.Contract.Repositories.Interface;
using DoAnChuyenNganh.Contract.Services.Interface;
using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.Core.Store;
using DoAnChuyenNganh.Core.Utils;
using DoAnChuyenNganh.ModelViews.AlumniCompanyModelViews;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DoAnChuyenNganh.Services.Service
{
    public class AlumniCompanyService : IAlumniCompanyService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AlumniCompanyService(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateAlumniCompany(AlumniCompanyModelView alumniCompanyModelView)
        {
            string? UserId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            AlumniCompany newAlumniCompany = _mapper.Map<AlumniCompany>(alumniCompanyModelView);
            newAlumniCompany.CreatedTime = CoreHelper.SystemTimeNow;
            newAlumniCompany.DeletedTime = null;
            newAlumniCompany.CreatedBy = UserId;
            await _unitOfWork.GetRepository<AlumniCompany>().InsertAsync(newAlumniCompany);
            await _unitOfWork.SaveAsync();
        }

        private async Task<BasePaginatedList<AlumniCompanyResponseDTO>> PaginateAlumniCompany(
        IQueryable<AlumniCompany> query,
        int? pageIndex,
        int? pageSize)
        {
            int currentPage = pageIndex ?? 1;
            int currentPageSize = pageSize ?? 10;
            int totalItems = await query.CountAsync();

            List<AlumniCompanyResponseDTO>? alumnicompanys = await query
                .Skip((currentPage - 1) * currentPageSize)
                .Take(currentPageSize)
                .Select(alumnicompany => new AlumniCompanyResponseDTO
                {
                    Id = alumnicompany.Id,
                    AlumniId = alumnicompany.AlumniId,
                    CompanyId = alumnicompany.CompanyId,
                    StartDate = alumnicompany.StartDate,
                    EndDate = alumnicompany.EndDate,
                    //CreatedBy = alumnicompany.CreatedBy,
                    //LastUpdatedBy = alumnicompany.LastUpdatedBy,
                    //CreatedTime = alumnicompany.CreatedTime,
                    //LastUpdatedTime = alumnicompany.LastUpdatedTime,
                })
                .ToListAsync();

            return new BasePaginatedList<AlumniCompanyResponseDTO>(alumnicompanys, totalItems, currentPage, currentPageSize);
        }
        public async Task<BasePaginatedList<AlumniCompanyResponseDTO>> GetAlumniCompany(string? id, string? alumniId, string? companyId, int pageIndex, int pageSize)
        {
            IQueryable<AlumniCompany>? query = _unitOfWork.GetRepository<AlumniCompany>().Entities.Where(l => l.DeletedTime == null);

            if (!string.IsNullOrWhiteSpace(id))
            {
                query = query.Where(a => a.Id == id);
            }
            if (!string.IsNullOrWhiteSpace(alumniId))
            {
                query = query.Where(a => a.AlumniId == alumniId);
            }
            if (!string.IsNullOrWhiteSpace(companyId))
            {
                query = query.Where(a => a.CompanyId == companyId);
            }

            return await PaginateAlumniCompany(query, pageIndex, pageSize);
        }

        public async Task UpdateAlumniCompany(string id, string alumiId, string CompanyId, AlumniCompanyModelView alumniCompanyModelView)
        {
            string? UserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã công ty cựu sinh viên!");
            }
            if (string.IsNullOrEmpty(CompanyId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã công ty!");
            }
            if (string.IsNullOrEmpty(alumiId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã cựu sinh viên!");
            }
            AlumniCompany? oldAlumniCompany = await _unitOfWork.GetRepository<AlumniCompany>()
                .Entities.FirstOrDefaultAsync(a => a.Id == id && a.AlumniId == alumiId && a.CompanyId == CompanyId)
                ?? throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, $"Không tìm thấy công ty cựu sinh viên nào với mã {id}");
            await _unitOfWork.GetRepository<AlumniCompany>().DeleteAsync(id, alumiId, CompanyId);
            AlumniCompany newAlumniCompany = new AlumniCompany
            { 
                CompanyId = alumniCompanyModelView.CompanyId,
                AlumniId = alumniCompanyModelView.AlumniId,
                Duty = alumniCompanyModelView.Duty,
                StartDate = alumniCompanyModelView.StartDate,
                EndDate = alumniCompanyModelView.EndDate,
                CreatedBy = UserId,
                LastUpdatedBy = UserId,
                CreatedTime = CoreHelper.SystemTimeNow,
                LastUpdatedTime = CoreHelper.SystemTimeNow
            };
            await _unitOfWork.GetRepository<AlumniCompany>().InsertAsync(newAlumniCompany);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAlumniCompany(string id, string alumniId, string companyId)
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
            if (string.IsNullOrEmpty(companyId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã hoạt động!");
            }
            if (string.IsNullOrEmpty(alumniId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã giảng viên!");
            }
            AlumniCompany? alumniCompany = await _unitOfWork.GetRepository<AlumniCompany>()
                .Entities.FirstOrDefaultAsync(a => a.Id == id && a.AlumniId == alumniId && a.CompanyId == companyId)
                ?? throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, $"Không tìm thấy hoạt động giảng viên nào với mã {id}");
            if (alumniCompany.DeletedTime != null)
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, "Hoạt động giảng viên đã bị xóa!");
            }
            alumniCompany.DeletedBy = UserId;
            alumniCompany.DeletedTime = CoreHelper.SystemTimeNow;
            await _unitOfWork.GetRepository<AlumniCompany>().UpdateAsync(alumniCompany);
            await _unitOfWork.SaveAsync();
        }
    }
}
