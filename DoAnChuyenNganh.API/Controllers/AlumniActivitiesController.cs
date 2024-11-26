using DoAnChuyenNganh.Contract.Services.Interface;
using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.ModelViews.AlumniActivitiesModelViews;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoAnChuyenNganhBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlumniActivitiesController : ControllerBase
    {
        private readonly IAlumniActivitiesService _alumniActivitiesService;

        public AlumniActivitiesController(IAlumniActivitiesService alumniActivitiesService)
        {
            _alumniActivitiesService = alumniActivitiesService;
        }

        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa, Trưởng bộ môn, Giáo vụ khoa, Giảng viên")]
        [HttpGet]
        public async Task<IActionResult> GetAlumniActivities(string? id, string? alumniId, string? activitiesId, int pageIndex = 1, int pageSize = 10)
        {
            BasePaginatedList<AlumniActivitiesResponseDTO>? paginatedAlumniActivities = await _alumniActivitiesService.GetAlumniActivities(id, alumniId, activitiesId, pageIndex, pageSize);
            return Ok(BaseResponse<BasePaginatedList<AlumniActivitiesResponseDTO>>.OkResponse(paginatedAlumniActivities));
        }

        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa, Trưởng bộ môn, Giáo vụ khoa, Giảng viên")]
        [HttpPost]
        public async Task<IActionResult> CreateAlumniActivities(AlumniActivitiesModelView alumniActivitiesModelView)
        {
            await _alumniActivitiesService.CreateAlumniActivities(alumniActivitiesModelView);
            return Ok(BaseResponse<string>.OkResponse("Thêm hoạt động cựu sinh viên thành công!"));
        }

        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa, Trưởng bộ môn, Giáo vụ khoa, Giảng viên")]
        [HttpPut]
        public async Task<IActionResult> UpdateAlumniActivities(string id, string alumniId,string activitiId , AlumniActivitiesModelView alumniActivitiesModelView)
        {
            await _alumniActivitiesService.UpdateAlumniActivities(id,alumniId,activitiId, alumniActivitiesModelView);
            return Ok(BaseResponse<string>.OkResponse("Sửa hoạt động cựu sinh viên thành công!"));
        }

        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa, Trưởng bộ môn, Giáo vụ khoa, Giảng viên")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAlumniActivities(string id, string alumniId, string activitiesId)
        {
            await _alumniActivitiesService.DeleteAlumniActivities(id, alumniId, activitiesId);
            return Ok(BaseResponse<string>.OkResponse("Xóa hoạt động cựu sinh viên thành công!"));
        }
    }
}

