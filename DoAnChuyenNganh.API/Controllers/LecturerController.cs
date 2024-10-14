using DoAnChuyenNganh.Contract.Services.Interface;
using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.ModelViews.LecturerModelViews;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DoAnChuyenNganhBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LecturerController : ControllerBase
    {
        private readonly ILecturerService _lecturerService;
        public LecturerController(ILecturerService lecturerService)
        {
            _lecturerService = lecturerService;
        }
        [HttpGet]
        public async Task<IActionResult> GetLecturers(string? id, string? name, int index = 1, int pageSize = 10)
        {
            BasePaginatedList<LecturerResponseDTO>? paginatedLecturers = await _lecturerService.GetLecturers(id, name, index, pageSize);
            return Ok(BaseResponse<BasePaginatedList<LecturerResponseDTO>>.OkResponse(paginatedLecturers));
        }
        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa, Trưởng bộ môn")]
        [HttpPost]
        public async Task<IActionResult> CreateLecturer(LecturerModelView lecturerModelView)
        {
            await _lecturerService.CreateLecturer(lecturerModelView);
            return Ok(BaseResponse<string>.OkResponse("Thêm thông tin giảng viên thành công!"));
        }
        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa, Trưởng bộ môn")]
        [HttpPut]
        public async Task<IActionResult> UpdateLecturer(string id, LecturerModelView lecturerModelView)
        {
            await _lecturerService.UpdateLecturer(id, lecturerModelView);
            return Ok(BaseResponse<string>.OkResponse("Sửa thông tin giảng viên thành công!"));
        }
        [Authorize(Roles = "Trưởng khoa")]
        [HttpDelete]
        public async Task<IActionResult> DeleteLecturer(string id)
        {
            await _lecturerService.DeleteLecturer(id);
            return Ok(BaseResponse<string>.OkResponse("Xóa thông tin giảng viên thành công!"));
        }
    }
}
