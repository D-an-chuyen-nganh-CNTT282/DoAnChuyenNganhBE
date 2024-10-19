using DoAnChuyenNganh.Contract.Services.Interface;
using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using DoAnChuyenNganh.ModelViews.StudentModelViews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoAnChuyenNganhBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa, Trưởng bộ môn, Giáo Vụ Khoa")]
        [HttpGet]
        public async Task<IActionResult> GetStudents(string? id, string? name, string? studentClass = null, string? studentMajor = null, int index = 1, int pageSize = 10)
        {
            BasePaginatedList<StudentResponseDTO>? paginatedStudents = await _studentService.GetStudents(id, name, studentClass, studentMajor, index, pageSize);
            return Ok(BaseResponse<BasePaginatedList<StudentResponseDTO>>.OkResponse(paginatedStudents));
        }

        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa, Trưởng bộ môn, Giáo Vụ Khoa")]
        [HttpPost]
        public async Task<IActionResult> CreateStudent(StudentModelView studentModelView)
        {
            await _studentService.CreateStudent(studentModelView);
            return Ok(BaseResponse<string>.OkResponse("Thêm thông tin sinh viên thành công!"));
        }

        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa, Trưởng bộ môn, Giáo Vụ Khoa")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(string id, StudentModelView studentModelView)
        {
            await _studentService.UpdateStudent(id, studentModelView);
            return Ok(BaseResponse<string>.OkResponse("Sửa thông tin sinh viên thành công!"));
        }

        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa, Trưởng bộ môn, Giáo Vụ Khoa")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(string id)
        {
            await _studentService.DeleteStudent(id);
            return Ok(BaseResponse<string>.OkResponse("Xóa thông tin sinh viên thành công!"));
        }
    }
}
