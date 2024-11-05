using AutoMapper;
using DoAnChuyenNganh.Contract.Repositories.Entity;
using DoAnChuyenNganh.Contract.Repositories.Interface;
using DoAnChuyenNganh.Contract.Services.Interface;
using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.Core.Store;
using DoAnChuyenNganh.Core.Utils;
using DoAnChuyenNganh.ModelViews.BusinessCollaborationModelViews;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DoAnChuyenNganh.Services.Service
{
    public class BusinessCollaborationService : IBusinessCollaborationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public BusinessCollaborationService(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task CreateBusinessCollaboration(BusinessCollaborationModelView businessCollaborationModelView)
        {
            string? UserId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            BusinessCollaboration businessCollaboration = _mapper.Map<BusinessCollaboration>(businessCollaborationModelView);
            businessCollaboration.CreatedTime = CoreHelper.SystemTimeNow;
            businessCollaboration.DeletedTime = null;
            businessCollaboration.CreatedBy = UserId;
            await _unitOfWork.GetRepository<BusinessCollaboration>().InsertAsync(businessCollaboration);
            await _unitOfWork.SaveAsync();
        }
        public async Task DeleteBusinessCollaboration(string id)
        {
            string? UserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã hợp tác doanh nghiệp!");
            }
            BusinessCollaboration? businessCollaboration = await _unitOfWork.GetRepository<BusinessCollaboration>().GetByIdAsync(id)
                ?? throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, $"Không tìm thấy hợp tác doanh nghiệp nào với mã {id}!");
            if (businessCollaboration.DeletedTime != null)
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, "Hợp tác doanh nghiệp đã bị xóa!");
            }
            businessCollaboration.DeletedBy = UserId;
            businessCollaboration.DeletedTime = CoreHelper.SystemTimeNow;
            await _unitOfWork.GetRepository<BusinessCollaboration>().UpdateAsync(businessCollaboration);
            await _unitOfWork.SaveAsync();
        }
        public async Task UpdateBusinessCollaboration(string id, BusinessCollaborationModelView businessCollaborationModelView)
        {
            string? UserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã hợp tác doanh nghiệp!");
            }
            BusinessCollaboration? businessCollaboration = await _unitOfWork.GetRepository<BusinessCollaboration>().GetByIdAsync(id)
                ?? throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, $"Không tìm thấy hợp tác doanh nghiệp nào với mã {id}!");
            _mapper.Map(businessCollaborationModelView, businessCollaboration);
            businessCollaboration.LastUpdatedTime = CoreHelper.SystemTimeNow;
            businessCollaboration.LastUpdatedBy = UserId;
            await _unitOfWork.GetRepository<BusinessCollaboration>().UpdateAsync(businessCollaboration);
            await _unitOfWork.SaveAsync();
        }
        private async Task<BasePaginatedList<BusinessCollaborationResponseDTO>> PaginateBusinessCollaboration(
        IQueryable<BusinessCollaboration> query,
        int? pageIndex,
        int? pageSize)
        {
            int currentPage = pageIndex ?? 1;
            int currentPageSize = pageSize ?? 10;
            int totalItems = await query.CountAsync();

            List<BusinessCollaborationResponseDTO>? businessCollaborations = await query
                .Skip((currentPage - 1) * currentPageSize)
                .Take(currentPageSize)
                .Select(businessCollaboration => new BusinessCollaborationResponseDTO
                {
                    Id = businessCollaboration.Id,
                    BusinessId = businessCollaboration.BusinessId,
                    ProjectName = businessCollaboration.ProjectName,
                    ProjectStatuss = businessCollaboration.ProjectStatuss.ToString(),
                    Result = businessCollaboration.Result,
                    //CreatedBy = businessCollaboration.CreatedBy,
                    //LastUpdatedBy = businessCollaboration.LastUpdatedBy,
                    //CreatedTime = businessCollaboration.CreatedTime,
                    //LastUpdatedTime = businessCollaboration.LastUpdatedTime,
                })
                .ToListAsync();

            return new BasePaginatedList<BusinessCollaborationResponseDTO>(businessCollaborations, totalItems, currentPage, currentPageSize);
        }
        public async Task<BasePaginatedList<BusinessCollaborationResponseDTO>> GetBusinessCollaborations(string? id, string? businessId, string? projectName, int pageIndex, int pageSize)
        {
            IQueryable<BusinessCollaboration>? query = _unitOfWork.GetRepository<BusinessCollaboration>().Entities.Where(b => b.DeletedTime == null);
            if (!string.IsNullOrWhiteSpace(id))
            {
                query = query.Where(b => b.Id == id);
            }
            if (!string.IsNullOrWhiteSpace(projectName))
            {
                query = query.Where(b => b.ProjectName == projectName);
            }
            if (!string.IsNullOrWhiteSpace(businessId))
            {
                query = query.Where(b => b.BusinessId == businessId);
            }
            return await PaginateBusinessCollaboration(query, pageIndex, pageSize);
        }
    }
}
