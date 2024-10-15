using DoAnChuyenNganh.Contract.Services.Interface;
using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.ModelViews.LecturerActivitiesModelViews;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoAnChuyenNganhBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LecturerActivitiesController : ControllerBase
    {
        private readonly ILecturerActivitiesService _lecturerActivitiesService;
        public LecturerActivitiesController(ILecturerActivitiesService lecturerActivitiesService)
        {
            _lecturerActivitiesService = lecturerActivitiesService;
        }
        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa, Trưởng bộ môn, Giảng viên")]
        [HttpGet]
        public async Task<IActionResult> GetLecturerActivities(string? id, string? lecturerId, string? activitiesId, int index = 1, int pageSize = 10)
        {
            BasePaginatedList<LecturerActivitiesResponseDTO>? paginatedLecturerActivities = await _lecturerActivitiesService.GetLecturerActivities(id, lecturerId, activitiesId, index, pageSize);
            return Ok(BaseResponse<BasePaginatedList<LecturerActivitiesResponseDTO>>.OkResponse(paginatedLecturerActivities));
        }
        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa, Trưởng bộ môn")]
        [HttpPost]
        public async Task<IActionResult> CreateLecturerActivities(LecturerActivitiesModelView lecturerActivitiesModelView)
        {
            await _lecturerActivitiesService.CreateLecturerActivities(lecturerActivitiesModelView);
            return Ok(BaseResponse<string>.OkResponse("Thêm hoạt động giảng viên thành công!"));
        }
        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa, Trưởng bộ môn")]
        [HttpPut]
        public async Task<IActionResult> UpdateLecturerActivities(string id, string lecturerId, string activitiesId, LecturerActivitiesModelView lecturerActivitiesModelView)
        {
            await _lecturerActivitiesService.UpdateLecturerActivities(id, lecturerId, activitiesId, lecturerActivitiesModelView);
            return Ok(BaseResponse<string>.OkResponse("Sửa hoạt động giảng viên thành công!"));
        }
        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa")]
        [HttpDelete]
        public async Task<IActionResult> DeleteLecturerActivities(string id, string lecturerId, string activitiesId)
        {
            await _lecturerActivitiesService.DeleteLecturerActivities(id, lecturerId, activitiesId);
            return Ok(BaseResponse<string>.OkResponse("Xóa hoạt động giảng viên thành công!"));
        }
    }
}
