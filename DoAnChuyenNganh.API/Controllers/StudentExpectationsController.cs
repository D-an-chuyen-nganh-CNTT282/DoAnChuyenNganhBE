using DoAnChuyenNganh.Contract.Services.Interface;
using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using DoAnChuyenNganh.ModelViews.StudentExpectationsModelViews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoAnChuyenNganhBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentExpectationsController : ControllerBase
    {
        private readonly IStudentExpectationsService _studentExpectationsService;

        public StudentExpectationsController(IStudentExpectationsService studentExpectationsService)
        {
            _studentExpectationsService = studentExpectationsService;
        }

        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa, Trưởng bộ môn, Giáo vụ khoa")]
        [HttpGet]
        public async Task<IActionResult> GetStudentExpectations(string? id, string? studentId, string? requestCategory, int pageIndex = 1, int pageSize = 10)
        {
            var paginatedStudentExpectations = await _studentExpectationsService.GetStudentExpectations(id, studentId, requestCategory, pageIndex, pageSize);
            return Ok(BaseResponse<BasePaginatedList<StudentExpectationsResponseDTO>>.OkResponse(paginatedStudentExpectations));
        }

        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa, Trưởng bộ môn, Giáo vụ khoa")]
        [HttpPost]
        public async Task<IActionResult> CreateStudentExpectations(StudentExpectationsModelView studentExpectationsModelView)
        {
            await _studentExpectationsService.CreateStudentExpectations(studentExpectationsModelView);
            return Ok(BaseResponse<string>.OkResponse("Thêm yêu cầu của sinh viên thành công!"));
        }

        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa, Trưởng bộ môn, Giáo vụ khoa")]
        [HttpPut]
        public async Task<IActionResult> UpdateStudentExpectations(string id, StudentExpectationsModelView studentExpectationsModelView)
        {
            await _studentExpectationsService.UpdateStudentExpectations(id, studentExpectationsModelView);
            return Ok(BaseResponse<string>.OkResponse("Sửa yêu cầu của sinh viên thành công!"));
        }
    }
}
