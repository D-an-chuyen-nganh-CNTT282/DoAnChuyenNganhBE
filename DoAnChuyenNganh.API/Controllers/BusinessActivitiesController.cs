using DoAnChuyenNganh.Contract.Services.Interface;
using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.ModelViews.BusinessActivitiesModelViews;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoAnChuyenNganhBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessActivitiesController : ControllerBase
    {
        private readonly IBusinessActivitesService _businessActivitiesService;
        public BusinessActivitiesController(IBusinessActivitesService businessActivitesService)
        {
            _businessActivitiesService = businessActivitesService;
        }
        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa")]
        [HttpGet]
        public async Task<IActionResult> GetBusinessActivities(string? id, string? businessId, string? activitiesId, int index = 1, int pageSize = 10)
        {
            BasePaginatedList<BusinessActivitiesResponseDTO>? paginatedBusinessActivities = await _businessActivitiesService.GetBusinessActivities(id, businessId, activitiesId, index, pageSize);
            return Ok(BaseResponse<BasePaginatedList<BusinessActivitiesResponseDTO>>.OkResponse(paginatedBusinessActivities));
        }
        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa")]
        [HttpPost]
        public async Task<IActionResult> CreateBusinessActivities(BusinessActivitiesModelView businessActivitiesModelView)
        {
            await _businessActivitiesService.CreateBusinessActivities(businessActivitiesModelView);
            return Ok(BaseResponse<string>.OkResponse("Thêm hoạt động doanh nghiệp thành công!"));
        }
        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa")]
        [HttpPut]
        public async Task<IActionResult> UpdateBusinessActivities(string id, string businessId, string activitiesId, BusinessActivitiesModelView businessActivitiesModelView)
        {
            await _businessActivitiesService.UpdateBusinessActivities(id, businessId, activitiesId, businessActivitiesModelView);
            return Ok(BaseResponse<string>.OkResponse("Sửa hoạt động doanh nghiệp thành công!"));
        }
        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa")]
        [HttpDelete]
        public async Task<IActionResult> DeleteBusinessActivities(string id, string businessId, string activitiesId)
        {
            await _businessActivitiesService.DeleteBusinessActivities(id, businessId, activitiesId);
            return Ok(BaseResponse<string>.OkResponse("Xóa hoạt động doanh nghiệp thành công!"));
        }
    }
}
