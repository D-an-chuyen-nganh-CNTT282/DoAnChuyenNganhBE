using DoAnChuyenNganh.Contract.Services.Interface;
using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using DoAnChuyenNganh.ModelViews.TeachingScheduleModelViews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoAnChuyenNganhBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachingScheduleController : ControllerBase
    {
        private readonly ITeachingScheduleService _teachingScheduleService;
        public TeachingScheduleController(ITeachingScheduleService teachingScheduleService)
        {
            _teachingScheduleService = teachingScheduleService;
        }
        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa, Trưởng bộ môn")]
        [HttpGet]
        public async Task<IActionResult> GetTeachingSchedules(string? id, string? subject, int index = 1, int pageSize = 10)
        {
            BasePaginatedList<TeachingScheduleResponseDTO>? paginatedTeachingSchedule = await _teachingScheduleService.GetTeachingSchedules(id, subject, index, pageSize);
            return Ok(BaseResponse<BasePaginatedList<TeachingScheduleResponseDTO>>.OkResponse(paginatedTeachingSchedule));
        }
        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa, Trưởng bộ môn")]
        [HttpPost]
        public async Task<IActionResult> CreateTeachingSchedule(TeachingScheduleModelView teachingScheduleModelView)
        {
            await _teachingScheduleService.CreateTeachingSchedule(teachingScheduleModelView);
            return Ok(BaseResponse<string>.OkResponse("Thêm lịch giảng dạy thành công!"));
        }
        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa, Trưởng bộ môn")]
        [HttpPut]
        public async Task<IActionResult> UpdateTeachingSchedule(string id, TeachingScheduleModelView teachingScheduleModelView)
        {
            await _teachingScheduleService.UpdateTeachingSchedule(id, teachingScheduleModelView);
            return Ok(BaseResponse<string>.OkResponse("Sửa lịch giảng dạy thành công!"));
        }
        [Authorize(Roles = "Trưởng khoa")]
        [HttpDelete]
        public async Task<IActionResult> DeleteTeachingSchedule(string id)
        {
            await _teachingScheduleService.DeleteTeachingSchedule(id);
            return Ok(BaseResponse<string>.OkResponse("Xóa lịch giảng dạy thành công!"));
        }
    }
}
