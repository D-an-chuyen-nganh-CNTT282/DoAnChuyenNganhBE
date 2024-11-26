using DoAnChuyenNganh.Contract.Services.Interface;
using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.ModelViews.IncomingDocumentModelViews;
using DoAnChuyenNganh.ModelViews.OutgoingDocumentModelViews;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using DoAnChuyenNganh.Services.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoAnChuyenNganhBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OutgoingDocumentController : ControllerBase
    {
        private readonly IOutgoingDocumentService _outgoingDocumentService; 

        public OutgoingDocumentController(IOutgoingDocumentService outgoingDocumentService)
        {
            _outgoingDocumentService = outgoingDocumentService;
        }

        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa, Trưởng bộ môn, Giáo vụ khoa")]
        [HttpGet]
        public async Task<IActionResult> GetOutgoingDocuments(string? title, string? departmentId, Guid? userId, int pageIndex = 1, int pageSize = 10)
        {
            BasePaginatedList<OutgoingDocumentResponseDTO> paginatedStudentExpectations = await _outgoingDocumentService.GetOutgoingDocuments(title, departmentId, userId, pageIndex, pageSize);
            return Ok(BaseResponse<BasePaginatedList<OutgoingDocumentResponseDTO>>.OkResponse(paginatedStudentExpectations));
        }

        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa, Trưởng bộ môn, Giáo vụ khoa")]
        [HttpPost]
        public async Task<IActionResult> CreateOutgoingDocument(OutgoingDocumentModelView outgoingDocumentModelView)
        {
            await _outgoingDocumentService.CreateOutgoingDocument(outgoingDocumentModelView);
            return Ok(BaseResponse<string>.OkResponse("Thêm công văn mới thành công!"));
        }
        [Authorize(Roles = "Trưởng khoa, Phó trưởng khoa, Trưởng bộ môn, Giáo vụ khoa")]
        [HttpPut]
        public async Task<IActionResult> Update(string id, OutgoingDocumentModelView model)
        {
            await _outgoingDocumentService.UpdateOutgoingDocument(id, model);
            return Ok(BaseResponse<string>.OkResponse("Đã sửa công văn đi thành công"));
        }
    }
}
