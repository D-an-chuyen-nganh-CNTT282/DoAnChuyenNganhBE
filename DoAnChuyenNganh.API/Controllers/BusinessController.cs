using DoAnChuyenNganh.Contract.Services.Interface;
using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.ModelViews.BusinessModelViews;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoAnChuyenNganhBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessController : ControllerBase
    {
        private readonly IBusinessService _businessService;
        public BusinessController(IBusinessService businessService)
        {
            _businessService = businessService;
        }
        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa")]
        [HttpGet]
        public async Task<IActionResult> GetBusiness(string? id, string? name, int index = 1, int pageSize = 10)
        {
            BasePaginatedList<BusinessResponseDTO>? paginatedBusiness = await _businessService.GetBusiness(id, name, index, pageSize);
            return Ok(BaseResponse<BasePaginatedList<BusinessResponseDTO>>.OkResponse(paginatedBusiness));
        }
        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa")]
        [HttpPost]
        public async Task<IActionResult> CreateBusiness(BusinessModelView businessModelView)
        {
            await _businessService.CreateBusiness(businessModelView);
            return Ok(BaseResponse<string>.OkResponse("Thêm thông tin doanh nghiệp thành công!"));
        }
        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa")]
        [HttpPut]
        public async Task<IActionResult> UpdateBusiness(string id, BusinessModelView businessModelView)
        {
            await _businessService.UpdateBusiness(id, businessModelView);
            return Ok(BaseResponse<string>.OkResponse("Sửa thông tin doanh nghiệp thành công!"));
        }
        [Authorize(Roles = "Trưởng khoa")]
        [HttpDelete]
        public async Task<IActionResult> DeleteBusiness(string id)
        {
            await _businessService.DeleteBusiness(id);
            return Ok(BaseResponse<string>.OkResponse("Xóa thông tin doanh nghiệp thành công!"));
        }
    }
}
