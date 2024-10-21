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
            string? userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Please log in!");
            }

            AlumniCompany alumniCompany = _mapper.Map<AlumniCompany>(alumniCompanyModelView);
            alumniCompany.CreatedBy = userId;
            alumniCompany.CreatedTime = CoreHelper.SystemTimeNow;
            await _unitOfWork.GetRepository<AlumniCompany>().InsertAsync(alumniCompany);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateAlumniCompany(string id, string alumniId, string companyId, AlumniCompanyModelView alumniCompanyModelView)
        {
            string? userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Please log in!");
            }

            if (string.IsNullOrWhiteSpace(id))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Please enter a valid alumni company ID!");
            }

            AlumniCompany? existingAlumniCompany = await _unitOfWork.GetRepository<AlumniCompany>()
                .Entities.FirstOrDefaultAsync(a => a.Id == id)
                ?? throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, $"No alumni company found with ID {id}");

            _mapper.Map(alumniCompanyModelView, existingAlumniCompany);
            existingAlumniCompany.LastUpdatedBy = userId;
            existingAlumniCompany.LastUpdatedTime = CoreHelper.SystemTimeNow;

            await _unitOfWork.SaveAsync();
        }

        public async Task<BasePaginatedList<AlumniCompanyResponseDTO>> GetAlumniCompany(string id, string? alumniId, string? companyId, int pageIndex, int pageSize)
        {
            IQueryable<AlumniCompany> query = _unitOfWork.GetRepository<AlumniCompany>().Entities.Where(b => b.DeletedTime == null);

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

            int totalItems = await query.CountAsync();
            List<AlumniCompanyResponseDTO> alumniCompanies = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(company => new AlumniCompanyResponseDTO
                {
                    Id = company.Id,
                    AlumniId = company.AlumniId,
                    CompanyId = company.CompanyId,
                    CreatedBy = company.CreatedBy,
                    LastUpdatedBy = company.LastUpdatedBy,
                    CreatedTime = company.CreatedTime,
                    LastUpdatedTime = company.LastUpdatedTime,
                })
                .ToListAsync();

            return new BasePaginatedList<AlumniCompanyResponseDTO>(alumniCompanies, totalItems, pageIndex, pageSize);
        }
    }
}
