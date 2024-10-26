using DoAnChuyenNganh.Contract.Services.Interface;
using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.ModelViews.IncomingDocumentModelViews;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace DoAnChuyenNganhBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncomingDocumentController : ControllerBase
    {
        private readonly IIncomingDocumentService _incomingDocumentService;

        public IncomingDocumentController(IIncomingDocumentService incomingDocumentService)
        {
            _incomingDocumentService = incomingDocumentService;
        }

        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa, Trưởng bộ môn, Giáo vụ khoa")]
        [HttpPost]
        public async Task<IActionResult> Create( IncomingDocumentModelViews model)
        {
            await _incomingDocumentService.Create(model);
            return Ok(BaseResponse<string>.OkResponse("Đã tạo công văn đến thành công"));
        }

        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa, Trưởng bộ môn, Giáo vụ khoa")]
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            await _incomingDocumentService.Delete(id);
            return Ok(BaseResponse<string>.OkResponse("Đã xóa công văn đến thành công"));

        }

        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa, Trưởng bộ môn, Giáo vụ khoa")]
        [HttpPut]
        public async Task<IActionResult> Update(string id,IncomingDocumentModelViews model)
        {
            await _incomingDocumentService.Update(id, model);
            return Ok(BaseResponse<string>.OkResponse("Đã sửa công văn đến thành công"));
        }

        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa, Trưởng bộ môn, Giáo vụ khoa")]
        [HttpGet]
        public async Task<IActionResult> Get( string? id, string? title, Guid? userId, DateTime? dueDate, int pageSize = 10, int pageIndex = 1)
        {
            BasePaginatedList<IncomingDocumentResponseDTO>? paginatedIncomingDocument = await _incomingDocumentService.Get(id, title, userId, dueDate, pageSize, pageIndex);
            return Ok(BaseResponse<BasePaginatedList<IncomingDocumentResponseDTO>>.OkResponse(paginatedIncomingDocument));
        }
    }
}
