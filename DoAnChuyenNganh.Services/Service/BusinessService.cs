using AutoMapper;
using DoAnChuyenNganh.Contract.Repositories.Entity;
using DoAnChuyenNganh.Contract.Repositories.Interface;
using DoAnChuyenNganh.Contract.Services.Interface;
using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.Core.Store;
using DoAnChuyenNganh.Core.Utils;
using DoAnChuyenNganh.ModelViews.BusinessModelViews;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DoAnChuyenNganh.Services.Service
{
    public class BusinessService : IBusinessService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public BusinessService(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task CreateBusiness(BusinessModelView businessModelView)
        {
            string? UserId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            Business business = _mapper.Map<Business>(businessModelView);
            business.CreatedTime = CoreHelper.SystemTimeNow;
            business.DeletedTime = null;
            business.CreatedBy = UserId;
            business.UserId = Guid.Parse(UserId);
            await _unitOfWork.GetRepository<Business>().InsertAsync(business);
            await _unitOfWork.SaveAsync();
        }
        public async Task DeleteBusiness(string id)
        {
            string? UserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã doanh nghiệp!");
            }
            Business? business = await _unitOfWork.GetRepository<Business>().GetByIdAsync(id)
                ?? throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, $"Không tìm thấy doanh nghiệp nào với mã {id}!");
            if (business.DeletedTime != null)
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, "Thông tin doanh nghiệp đã bị xóa!");
            }
            business.DeletedBy = UserId;
            business.DeletedTime = CoreHelper.SystemTimeNow;
            await _unitOfWork.GetRepository<Business>().UpdateAsync(business);
            await _unitOfWork.SaveAsync();
        }
        public async Task UpdateBusiness(string id, BusinessModelView businessModelView)
        {
            string? UserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã doanh nghiệp!");
            }
            Business? business = await _unitOfWork.GetRepository<Business>().GetByIdAsync(id)
                ?? throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, $"Không tìm thấy doanh nghiệp nào với mã {id}!");
            _mapper.Map(businessModelView, business);
            business.LastUpdatedTime = CoreHelper.SystemTimeNow;
            business.LastUpdatedBy = UserId;
            business.UserId = Guid.Parse(UserId);
            await _unitOfWork.GetRepository<Business>().UpdateAsync(business);
            await _unitOfWork.SaveAsync();
        }
        private async Task<BasePaginatedList<BusinessResponseDTO>> PaginateBusiness(
        IQueryable<Business> query,
        int? pageIndex,
        int? pageSize)
        {
            int currentPage = pageIndex ?? 1;
            int currentPageSize = pageSize ?? 10;
            int totalItems = await query.CountAsync();

            List<BusinessResponseDTO>? businesses = await query
                .Skip((currentPage - 1) * currentPageSize)
                .Take(currentPageSize)
                .Select(bussiness => new BusinessResponseDTO
                {
                    Id = bussiness.Id,
                    LecturerId = bussiness.LecturerId,
                    BusinessName = bussiness.BusinessName,
                    BusinessAddress = bussiness.BusinessAddress,
                    BusinessPhone = bussiness.BusinessPhone,
                    BusinessEmail = bussiness.BusinessEmail,
                    //CreatedBy = bussiness.CreatedBy,
                    //LastUpdatedBy = bussiness.LastUpdatedBy,
                    //CreatedTime = bussiness.CreatedTime,
                    //LastUpdatedTime = bussiness.LastUpdatedTime,
                })
                .ToListAsync();

            return new BasePaginatedList<BusinessResponseDTO>(businesses, totalItems, currentPage, currentPageSize);
        }
        public async Task<BasePaginatedList<BusinessResponseDTO>> GetBusiness(string? id, string? name, int pageIndex, int pageSize)
        {
            IQueryable<Business>? query = _unitOfWork.GetRepository<Business>().Entities.Where(b => b.DeletedTime == null);
            if (!string.IsNullOrWhiteSpace(id))
            {
                query = query.Where(b => b.Id == id);
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(b => b.BusinessName == name);
            }
            return await PaginateBusiness(query, pageIndex, pageSize);
        }
    }
}
