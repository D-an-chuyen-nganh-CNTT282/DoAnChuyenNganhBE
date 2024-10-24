using DoAnChuyenNganh.Contract.Services.Interface;
using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.ModelViews.AlumniCompanyModelViews;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoAnChuyenNganhBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlumniCompanyController : ControllerBase
    {
        private readonly IAlumniCompanyService _alumniCompanyService;

        public AlumniCompanyController(IAlumniCompanyService alumniCompanyService)
        {
            _alumniCompanyService = alumniCompanyService;
        }

        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa")]
        [HttpGet]
        public async Task<IActionResult> GetAlumniCompany(string? id, string? alumniId, string? companyId, int pageIndex = 1, int pageSize = 10)
        {
            BasePaginatedList<AlumniCompanyResponseDTO> paginatedAlumniCompanies = await _alumniCompanyService.GetAlumniCompany(id, alumniId, companyId, pageIndex, pageSize);
            return Ok(BaseResponse<BasePaginatedList<AlumniCompanyResponseDTO>>.OkResponse(paginatedAlumniCompanies));
        }

        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa")]
        [HttpPost]
        public async Task<IActionResult> CreateAlumniCompany(AlumniCompanyModelView alumniCompanyModelView)
        {
            await _alumniCompanyService.CreateAlumniCompany(alumniCompanyModelView);
            return Ok(BaseResponse<string>.OkResponse("Thêm công ty cho cựu sinh viên thành công!"));
        }

        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAlumniCompany(string id, string alumniId, string CompanyId,  AlumniCompanyModelView alumniCompanyModelView)
        {
            await _alumniCompanyService.UpdateAlumniCompany(id,alumniId, CompanyId, alumniCompanyModelView);
            return Ok(BaseResponse<string>.OkResponse("Cập nhật công ty cho cựu sinh viên thành công!"));
        }
        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAlumniCompany(string id, string alumniId, string companyId)
        {
            await _alumniCompanyService.DeleteAlumniCompany(id, alumniId, companyId);
            return Ok(BaseResponse<string>.OkResponse("Xóa công ty cựu sinh viên thành công!"));
        }
    }
}
