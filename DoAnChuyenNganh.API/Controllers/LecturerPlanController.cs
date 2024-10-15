using DoAnChuyenNganh.Contract.Services.Interface;
using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.ModelViews.LecturerPlanModelViews;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoAnChuyenNganhBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LecturerPlanController : ControllerBase
    {
        public readonly ILecturerPlanService _lecturerPlanService;
        public LecturerPlanController(ILecturerPlanService lecturerPlanService)
        {
            _lecturerPlanService = lecturerPlanService;
        }
        [Authorize(Roles = "Giảng viên")]
        [HttpGet]
        public async Task<IActionResult> GetLecturerPlans(string? id, int index = 1, int pageSize = 10)
        {
            BasePaginatedList<LecturerPlanResponseDTO>? paginatedLecturerPlans = await _lecturerPlanService.GetLecturerPlans(id, index, pageSize);
            return Ok(BaseResponse<BasePaginatedList<LecturerPlanResponseDTO>>.OkResponse(paginatedLecturerPlans));
        }
        [Authorize(Roles = "Giảng viên")]
        [HttpPost]
        public async Task<IActionResult> CreateLecturerPlan(LecturerPlanModelView lecturerPlanModelView)
        {
            await _lecturerPlanService.CreateLecturerPlan(lecturerPlanModelView);
            return Ok(BaseResponse<string>.OkResponse("Thêm kế hoạch thành công!"));
        }
        [Authorize(Roles = "Giảng viên")]
        [HttpPut]
        public async Task<IActionResult> UpdateLecturerPlan(string id, LecturerPlanModelView lecturerPlanModelView)
        {
            await _lecturerPlanService.UpdateLecturerPlan(id, lecturerPlanModelView);
            return Ok(BaseResponse<string>.OkResponse("Sửa kế hoạch thành công!"));
        }
        [Authorize(Roles = "Giảng viên")]
        [HttpDelete]
        public async Task<IActionResult> DeleteLecturerPlan(string id)
        {
            await _lecturerPlanService.DeleteLecturerPlan(id);
            return Ok(BaseResponse<string>.OkResponse("Xóa kế hoạch thành công!"));
        }
    }
}
