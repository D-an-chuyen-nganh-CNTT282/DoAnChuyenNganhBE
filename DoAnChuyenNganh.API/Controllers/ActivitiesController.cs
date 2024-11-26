using DoAnChuyenNganh.Contract.Services.Interface;
using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.ModelViews.ActivitiesModelViews;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoAnChuyenNganhBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {
        private readonly IActivitiesService _activitiesService;
        public ActivitiesController(IActivitiesService activitiesService)
        {
            _activitiesService = activitiesService;
        }
        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa, Giảng viên, Giáo vụ khoa")]
        [HttpGet]
        public async Task<IActionResult> GetActivities(string? id, string? name, int index = 1, int pageSize = 10)
        {
            BasePaginatedList<ActivitiesResponseDTO>? paginatedActivities = await _activitiesService.GetActivities(id, name, index, pageSize);
            return Ok(BaseResponse<BasePaginatedList<ActivitiesResponseDTO>>.OkResponse(paginatedActivities));
        }
        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa")]
        [HttpPost]
        public async Task<IActionResult> CreateActivities(ActivitiesModelView activitiesModelView)
        {
            await _activitiesService.CreateActivities(activitiesModelView);
            return Ok(BaseResponse<string>.OkResponse("Thêm hoạt động thành công!"));
        }
        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa")]
        [HttpPut]
        public async Task<IActionResult> UpdateActivities(string id, ActivitiesModelView activitiesModelView)
        {
            await _activitiesService.UpdateActivities(id, activitiesModelView);
            return Ok(BaseResponse<string>.OkResponse("Sửa hoạt động thành công!"));
        }
        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa")]
        [HttpDelete]
        public async Task<IActionResult> DeleteActivities(string id)
        {
            await _activitiesService.DeleteActivities(id);
            return Ok(BaseResponse<string>.OkResponse("Xóa hoạt động thành công!"));
        }
    }
}
