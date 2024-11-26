using DoAnChuyenNganh.Contract.Services.Interface;
using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.ModelViews.AlumniModelViews;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace DoAnChuyenNganhBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlumniController : ControllerBase
    {
        private readonly IAlumniService _Ialumniservice;
        public AlumniController(IAlumniService Ialumservice)
        {
           _Ialumniservice = Ialumservice;
        }

        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa")]
        [HttpPost]
        public async Task<IActionResult> Create( AlumniModelView model)
        {
            await _Ialumniservice.Create(model);
            return Ok(BaseResponse<string>.OkResponse("Đã tạo cựu sinh viên thành công"));

        }

        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa")]
        [HttpPut]
        public async Task<IActionResult> Update(string id, AlumniModelView model)
        {
            await _Ialumniservice.Update(id, model);
            return Ok(BaseResponse<string>.OkResponse("Đã sửa cựu sinh viên thành công"));
        }

        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa, Giảng viên, Giáo vụ khoa")]
        [HttpGet]
        public async Task<IActionResult> Get( string? id,  string? alumniName, string? alumniMajor,  string? alumniCourse, int pageSize = 10, int pageIndex = 1)
        {
            BasePaginatedList<AlumniResponseDTO>? paginatedAlumni = await _Ialumniservice.Get(id, alumniName, alumniMajor, alumniCourse, pageSize, pageIndex);
            return Ok(BaseResponse<BasePaginatedList<AlumniResponseDTO>>.OkResponse(paginatedAlumni));
        }
    }
}
