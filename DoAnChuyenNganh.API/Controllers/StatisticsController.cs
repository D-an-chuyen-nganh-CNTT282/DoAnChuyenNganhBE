using DoAnChuyenNganh.Contract.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DoAnChuyenNganhBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;
        public StatisticsController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }
        [HttpGet("entity-counts")]
        public async Task<IActionResult> GetEntityCounts()
        {
            var counts = await _statisticsService.GetEntityCountsAsync();
            return Ok(counts); // Trả về JSON kết quả thống kê
        }
        [HttpGet("upcoming-activities")]
        public async Task<IActionResult> GetUpcomingActivities([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            if (pageIndex < 1 || pageSize < 1)
            {
                return BadRequest("PageIndex và PageSize phải lớn hơn 0.");
            }

            try
            {
                var activities = await _statisticsService.GetUpcomingActivitiesAsync(pageIndex, pageSize);
                return Ok(activities); // Trả về dữ liệu với mã trạng thái 200 OK
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và trả về mã trạng thái 500 Internal Server Error
                return StatusCode(500, new { Message = "Đã xảy ra lỗi khi lấy danh sách hoạt động.", Details = ex.Message });
            }
        }
    }
}
