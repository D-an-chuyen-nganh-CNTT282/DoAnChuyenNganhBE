using AutoMapper;
using DoAnChuyenNganh.Contract.Repositories.Entity;
using DoAnChuyenNganh.Contract.Repositories.Interface;
using DoAnChuyenNganh.Contract.Services.Interface;
using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.Core.Store;
using DoAnChuyenNganh.Core.Utils;
using DoAnChuyenNganh.ModelViews.CompanyModelViews;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DoAnChuyenNganh.Services.Service
{
    public class CompanyService : ICompanyService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CompanyService(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task CreateCompany(CompanyModelViews companyModelView)
        {
            string? UserId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            Company newCompany = _mapper.Map<Company>(companyModelView);
            newCompany.CreatedTime = CoreHelper.SystemTimeNow;
            newCompany.DeletedTime = null;
            newCompany.CreatedBy = UserId;
            newCompany.UserId = Guid.Parse(UserId);
            await _unitOfWork.GetRepository<Company>().InsertAsync(newCompany);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteCompany(string id)
        {
            string? UserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã công ty!");
            }
            Company? company = await _unitOfWork.GetRepository<Company>().GetByIdAsync(id)
                ?? throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, $"Không tìm thấy công ty nào với mã {id}!");
            if (company.DeletedTime != null)
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, "Thông tin công ty đã bị xóa!");
            }
            company.DeletedBy = UserId;
            company.DeletedTime = CoreHelper.SystemTimeNow;
            await _unitOfWork.GetRepository<Company>().UpdateAsync(company);
            await _unitOfWork.SaveAsync();
        }
        private async Task<BasePaginatedList<CompanyResponseDTO>> PaginateCompany(
       IQueryable<Company> query,
       int? pageIndex,
       int? pageSize)
        {
            int currentPage = pageIndex ?? 1;
            int currentPageSize = pageSize ?? 10;
            int totalItems = await query.CountAsync();

            List<CompanyResponseDTO>? companys = await query
                .Skip((currentPage - 1) * currentPageSize)
                .Take(currentPageSize)
                .Select(company => new CompanyResponseDTO
                {
                    Id = company.Id,
                    CompanyAddress = company.CompanyAddress,
                    CompanyName = company.CompanyName,
                    CompanyEmail = company.CompanyEmail,
                    CompanyPhone = company.CompanyPhone,
                    UserId = company.UserId,
                    CreatedBy = company.CreatedBy,
                    LastUpdatedBy = company.LastUpdatedBy,
                    CreatedTime = company.CreatedTime,
                    LastUpdatedTime = company.LastUpdatedTime,
                })
                .ToListAsync();

            return new BasePaginatedList<CompanyResponseDTO>(companys, totalItems, currentPage, currentPageSize);
        }
        public async Task<BasePaginatedList<CompanyResponseDTO>> GetCompany(string? id, string? name, int pageIndex, int pageSize)
        {
            IQueryable<Company>? query = _unitOfWork.GetRepository<Company>().Entities.Where(l => l.DeletedTime == null);
            if (!string.IsNullOrWhiteSpace(id))
            {
                query = query.Where(lecturer => lecturer.Id == id);
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(lecturer => lecturer.CompanyName == name);
            }
            return await PaginateCompany(query, pageIndex, pageSize);
        }

        public async Task UpdateCompany(string id, CompanyModelViews companyModelView)
        {
            string? UserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã công ty!");
            }
            Company? company = await _unitOfWork.GetRepository<Company>().GetByIdAsync(id)
                ?? throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, $"Không tìm thấy công ty nào với mã {id}!");
            _mapper.Map(companyModelView, company);
            company.LastUpdatedTime = CoreHelper.SystemTimeNow;
            company.LastUpdatedBy = UserId;
            company.UserId = Guid.Parse(UserId);
            await _unitOfWork.GetRepository<Company>().UpdateAsync(company);
            await _unitOfWork.SaveAsync();
        }
    }
}
