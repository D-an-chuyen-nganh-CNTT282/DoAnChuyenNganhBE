using DoAnChuyenNganh.Contract.Services.Interface;
using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using Microsoft.AspNetCore.Mvc;

namespace DoAnChuyenNganhBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        public readonly IDepartmentService _departmentservice;
        public DepartmentController(IDepartmentService departmentservice)
        {
            _departmentservice = departmentservice;
        }
        [HttpGet]
        public async Task<IActionResult> GetDepartment(string? id, string? name, int index = 1, int pageSize = 10)
        {
            BasePaginatedList<DepartmentResponseDTO>? paginatedDepartment = await _departmentservice.GetDepartments(id, name, index, pageSize);
            return Ok(BaseResponse<BasePaginatedList<DepartmentResponseDTO>>.OkResponse(paginatedDepartment));
        }
    }
}
