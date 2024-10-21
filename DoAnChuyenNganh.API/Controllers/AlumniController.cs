using DoAnChuyenNganh.Contract.Services.Interface;
using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.ModelViews.AlumniModelViews;
using Microsoft.AspNetCore.Mvc;

namespace DoAnChuyenNganhBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlumniController : ControllerBase
    {
        private readonly IAlumniService _Ialumservice;
        public AlumniController(IAlumniService Ialumservice)
        {
           _Ialumservice = Ialumservice;
        }

        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AlumniModelView model)
        {
            await _Ialumservice.Create(model);
            return Ok(BaseResponse<string>.OkResponse("Đã tạo thành công"));

        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _Ialumservice.Delete(id);
            return Ok(BaseResponse<string>.OkResponse("Đã xóa thành công"));

        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, AlumniModelView model)
        {
            await _Ialumservice.Update(id, model);
            return Ok(BaseResponse<string>.OkResponse("Đã sửa thành công"));
        }

        // Phương thức phân trang
        [HttpGet]
        public async Task<IActionResult> Get( string? id,  string? alumniName, string? alumniMajor,  string? alumniCourse, int pageSize = 10, int pageIndex = 1)
        {
            var result = await _Ialumservice.Get(id, alumniName, alumniMajor, alumniCourse,pageSize, pageIndex);
            return Ok(result);
        }
    }
}
