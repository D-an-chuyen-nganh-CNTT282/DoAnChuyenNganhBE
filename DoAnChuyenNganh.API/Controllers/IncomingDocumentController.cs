using DoAnChuyenNganh.Contract.Services.Interface;
using DoAnChuyenNganh.ModelViews.IncomingDocumentModelViews;
using DoAnChuyenNganh.Services.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static DoAnChuyenNganh.Contract.Repositories.Entity.IncomingDocument;

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

        // Phương thức tạo tài liệu đi
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] IncomingDocumentModelViews model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _incomingDocumentService.Create(model);
            return Ok(new { message = "Document created successfully" });
        }

        // Phương thức xóa tài liệu đi theo id
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _incomingDocumentService.Delete(id);
                return Ok(new { message = "Document deleted successfully" });
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
        }

        // Phương thức cập nhật tài liệu đi theo id
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] IncomingDocumentModelViews model, IncomingDocumentProcessingStatus status)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _incomingDocumentService.Update(id, model,status);
                return Ok(new { message = "Document updated successfully" });
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
        }

        // Phương thức lấy danh sách tài liệu đi theo phân trang
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? id, [FromQuery] string? title, [FromQuery] Guid userId, [FromQuery] DateTime dueDate, [FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 1)
        {
            var result = await _incomingDocumentService.Get(id, title, userId, dueDate, pageSize, pageIndex);
            return Ok(result);
        }
    }
}
