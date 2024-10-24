using DoAnChuyenNganh.Contract.Services.Interface;
using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.ModelViews.CompanyModelViews;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoAnChuyenNganhBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _CompanyService;
        public CompanyController(ICompanyService CompanyService)
        {
            _CompanyService = CompanyService;
        }
        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa")]
        [HttpGet]
        public async Task<IActionResult> GetCompany(string? id, string? name, int index = 1, int pageSize = 10)
        {
            BasePaginatedList<CompanyResponseDTO>? paginatedCompany = await  _CompanyService.GetCompany(id, name, index, pageSize);
            return Ok(BaseResponse<BasePaginatedList<CompanyResponseDTO>>.OkResponse(paginatedCompany));
        }
        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa")]
        [HttpPost]
        public async Task<IActionResult> CreateCompany(CompanyModelViews companyModelView)
        {
            await  _CompanyService.CreateCompany(companyModelView);
            return Ok(BaseResponse<string>.OkResponse("Thêm thông tin công ty thành công!"));
        }
        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa")]
        [HttpPut]
        public async Task<IActionResult> UpdateCompany(string id, CompanyModelViews companyModelView)
        {
            await  _CompanyService.UpdateCompany(id, companyModelView);
            return Ok(BaseResponse<string>.OkResponse("Sửa thông tin công ty thành công!"));
        }
        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa")]
        [HttpDelete]
        public async Task<IActionResult> DeleteCompany(string id)
        {
            await  _CompanyService.DeleteCompany(id);
            return Ok(BaseResponse<string>.OkResponse("Xóa thông tin công ty thành công!"));
        }
    }
}
