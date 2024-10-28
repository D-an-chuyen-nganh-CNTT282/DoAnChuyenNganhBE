using DoAnChuyenNganh.Contract.Services.Interface;
using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.ModelViews.BusinessCollaborationModelViews;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoAnChuyenNganhBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessCollaboratonController : ControllerBase
    {
        private readonly IBusinessCollaborationService _businessCollaborationService;
        public BusinessCollaboratonController(IBusinessCollaborationService businessCollaborationService)
        {
            _businessCollaborationService = businessCollaborationService;
        }
        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa")]
        [HttpGet]
        public async Task<IActionResult> GetBusinessCollabrorations(string? id, string? businessId, string? projectName, int index = 1, int pageSize = 10)
        {
            BasePaginatedList<BusinessCollaborationResponseDTO>? paginatedBusinessCollaborations = await _businessCollaborationService.GetBusinessCollaborations(id, businessId, projectName, index, pageSize);
            return Ok(BaseResponse<BasePaginatedList<BusinessCollaborationResponseDTO>>.OkResponse(paginatedBusinessCollaborations));
        }
        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa")]
        [HttpPost]
        public async Task<IActionResult> CreateBusinessCollaboration(BusinessCollaborationModelView businessCollaborationModelView)
        {
            await _businessCollaborationService.CreateBusinessCollaboration(businessCollaborationModelView);
            return Ok(BaseResponse<string>.OkResponse("Thêm hợp tác doanh nghiệp thành công!"));
        }
        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa")]
        [HttpPut]
        public async Task<IActionResult> UpdateBusinessCollaboration(string id, BusinessCollaborationModelView businessCollaborationModelView)
        {
            await _businessCollaborationService.UpdateBusinessCollaboration(id, businessCollaborationModelView);
            return Ok(BaseResponse<string>.OkResponse("Sửa hợp tác doanh nghiệp thành công!"));
        }
        [Authorize(Roles = "Trưởng khoa")]
        [HttpDelete]
        public async Task<IActionResult> DeleteBusinessCollaboration(string id)
        {
            await _businessCollaborationService.DeleteBusinessCollaboration(id);
            return Ok(BaseResponse<string>.OkResponse("Xóa hợp tác doanh nghiệp thành công!"));
        }
    }
}
