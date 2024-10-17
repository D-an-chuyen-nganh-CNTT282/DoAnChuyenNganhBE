using DoAnChuyenNganh.Contract.Services.Interface;
using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.ModelViews.InternshipMangamentModelViews;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoAnChuyenNganhBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InternshipManagementController : ControllerBase
    {
        public readonly IInternshipManagementService _internshipManagementService;
        public InternshipManagementController(IInternshipManagementService internshipManagementService)
        {
            _internshipManagementService = internshipManagementService;
        }
        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa")]
        [HttpGet]
        public async Task<IActionResult> GetInternshipManagements(string? id, string? studentId, string? businessId, int index = 1, int pageSize = 10)
        {
            BasePaginatedList<InternshipManagementResponseDTO> paginatedInternshipManagements = await _internshipManagementService.GetInternshipManagements(id, studentId, businessId, index, pageSize);
            return Ok(BaseResponse<BasePaginatedList<InternshipManagementResponseDTO>>.OkResponse(paginatedInternshipManagements));
        }
        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa")]
        [HttpPost]
        public async Task<IActionResult> CreateInternshipManagement(InternshipManagementModelView internshipManagementModelView)
        {
            await _internshipManagementService.CreateInternshipManagement(internshipManagementModelView);
            return Ok(BaseResponse<string>.OkResponse("Thêm thông tin thực tập thành công!"));
        }
        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa")]
        [HttpPut]
        public async Task<IActionResult> UpdateInternshipManagement(string id, string studentId, string businessId, InternshipManagementModelView internshipManagementModelView)
        {
            await _internshipManagementService.UpdateInternshipManagement(id, studentId, businessId, internshipManagementModelView);
            return Ok(BaseResponse<string>.OkResponse("Sửa thông tin thực tập thành công!"));
        }
        [Authorize(Roles = "Trưởng khoa")]
        [HttpDelete]
        public async Task<IActionResult> DeleteInternshipManagement(string id, string studentId, string businessId)
        {
            await _internshipManagementService.DeleteInternshipManagement(id, studentId, businessId);
            return Ok(BaseResponse<string>.OkResponse("Xóa thông tin thực tập thành công!"));
        }
    }
}
