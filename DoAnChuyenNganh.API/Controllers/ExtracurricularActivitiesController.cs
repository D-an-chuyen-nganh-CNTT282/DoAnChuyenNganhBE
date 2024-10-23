using DoAnChuyenNganh.Contract.Services.Interface;
using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.ModelViews.ExtracurricularActivitiesModelViews;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoAnChuyenNganhBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExtracurricularActivitiesController : ControllerBase
    {
        private readonly IExtracurricularActivitiesService _extracurricularActivitiesService;

        public ExtracurricularActivitiesController(IExtracurricularActivitiesService extracurricularActivitiesService)
        {
            _extracurricularActivitiesService = extracurricularActivitiesService;
        }

        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa, Trưởng bộ môn, Giáo vụ khoa, Giảng Viên")]
        [HttpGet]
        public async Task<IActionResult> GetExtracurricularActivities(string? id, string? studentId, string? activitiesId, int pageIndex = 1, int pageSize = 10)
        {
            BasePaginatedList<ExtracurricularActivitiesReponseDTO> paginatedExtracurricularActivities = await _extracurricularActivitiesService.GetExtracurricularActivities(id, studentId, activitiesId, pageIndex, pageSize);
            return Ok(BaseResponse<BasePaginatedList<ExtracurricularActivitiesReponseDTO>>.OkResponse(paginatedExtracurricularActivities));
        }

        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa, Trưởng bộ môn, Giáo vụ khoa, Giảng Viên")]
        [HttpPost]
        public async Task<IActionResult> CreateExtracurricularActivities(ExtracurricularActivitiesModelView extracurricularActivitiesModelView)
        {
            await _extracurricularActivitiesService.CreateExtracurricularActivities(extracurricularActivitiesModelView);
            return Ok(BaseResponse<string>.OkResponse("Thêm hoạt động ngoại khóa thành công!"));
        }

        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa, Trưởng bộ môn, Giáo vụ khoa, Giảng Viên")]
        [HttpPut]
        public async Task<IActionResult> UpdateExtracurricularActivities(string id, string studentId, string activitiesId, ExtracurricularActivitiesModelView extracurricularActivitiesModelView)
        {
            await _extracurricularActivitiesService.UpdateExtracurricularActivities(id, studentId, activitiesId, extracurricularActivitiesModelView);
            return Ok(BaseResponse<string>.OkResponse("Sửa hoạt động ngoại khóa thành công!"));
        }
    }
}