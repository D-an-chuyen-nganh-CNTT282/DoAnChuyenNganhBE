using DoAnChuyenNganh.Contract.Services.Interface;
using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using DoAnChuyenNganh.Repositories.Context;
using Microsoft.EntityFrameworkCore;

namespace DoAnChuyenNganh.Services.Service
{
    public class StatisticsService : IStatisticsService
    {
        private readonly DatabaseContext _context;

        public StatisticsService(DatabaseContext context)
        {
            _context = context;
        }

        // Thống kê tổng số lượng Student, Alumni, Business, Lecturer
        public async Task<Dictionary<string, int>> GetEntityCountsAsync()
        {
            var studentCount = await _context.Student.CountAsync();
            var alumniCount = await _context.Alumni.CountAsync();
            var businessCount = await _context.Business.CountAsync();
            var lecturerCount = await _context.Lecturer.CountAsync();

            return new Dictionary<string, int>
            {
                { "Students", studentCount },
                { "Alumni", alumniCount },
                { "Businesses", businessCount },
                { "Lecturers", lecturerCount }
            };
        }

        // Hiển thị các hoạt động sắp tới
        private async Task<BasePaginatedList<ActivitiesResponseDTO>> PaginateActivities(
        IQueryable<ActivitiesResponseDTO> query,
        int? pageIndex,
        int? pageSize)
        {
            int currentPage = pageIndex ?? 1;
            int currentPageSize = pageSize ?? 10;
            int totalItems = await query.CountAsync();

            List<ActivitiesResponseDTO>? activities = await query
                .Skip((currentPage - 1) * currentPageSize)
                .Take(currentPageSize)
                .ToListAsync();

            return new BasePaginatedList<ActivitiesResponseDTO>(activities, totalItems, currentPage, currentPageSize);
        }

        public async Task<BasePaginatedList<ActivitiesResponseDTO>> GetUpcomingActivitiesAsync(int pageIndex, int pageSize)
        {
            var currentDate = DateTime.Now;

            // Lấy dữ liệu từ các bảng và chuyển thành danh sách
            var alumniActivities = await _context.AlumniActivities
                .Include(a => a.Activities)
                .Where(a => a.Activities.EventDate > currentDate)
                .Select(a => new ActivitiesResponseDTO
                {
                    Id = a.Activities.Id,
                    Name = a.Activities.Name,
                    EventDate = a.Activities.EventDate,
                    Location = a.Activities.Location,
                    EventTypes = "Alumni Activity",
                    Description = a.Activities.Description
                })
                .ToListAsync();

            var businessActivities = await _context.BusinessActivities
                .Include(b => b.Activities)
                .Where(b => b.Activities.EventDate > currentDate)
                .Select(b => new ActivitiesResponseDTO
                {
                    Id = b.Activities.Id,
                    Name = b.Activities.Name,
                    EventDate = b.Activities.EventDate,
                    Location = b.Activities.Location,
                    EventTypes = "Business Activity",
                    Description = b.Activities.Description
                })
                .ToListAsync();

            var lecturerActivities = await _context.LecturerActivities
                .Include(l => l.Activities)
                .Where(l => l.Activities.EventDate > currentDate)
                .Select(l => new ActivitiesResponseDTO
                {
                    Id = l.Activities.Id,
                    Name = l.Activities.Name,
                    EventDate = l.Activities.EventDate,
                    Location = l.Activities.Location,
                    EventTypes = "Lecturer Activity",
                    Description = l.Activities.Description
                })
                .ToListAsync();

            var extracurricularActivities = await _context.ExtracurricularActivities
                .Include(e => e.Activities)
                .Where(e => e.Activities.EventDate > currentDate)
                .Select(e => new ActivitiesResponseDTO
                {
                    Id = e.Activities.Id,
                    Name = e.Activities.Name,
                    EventDate = e.Activities.EventDate,
                    Location = e.Activities.Location,
                    EventTypes = "Extracurricular Activity",
                    Description = e.Activities.Description
                })
                .ToListAsync();

            // Kết hợp danh sách và loại bỏ trùng lặp
            var allActivities = alumniActivities
                .Concat(businessActivities)
                .Concat(lecturerActivities)
                .Concat(extracurricularActivities)
                .GroupBy(a => a.Id)
                .Select(g => g.First()) // Giữ bản ghi đầu tiên trong mỗi nhóm
                .OrderBy(a => a.EventDate)
                .ToList();

            // Phân trang
            int totalItems = allActivities.Count;
            var paginatedActivities = allActivities
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new BasePaginatedList<ActivitiesResponseDTO>(paginatedActivities, totalItems, pageIndex, pageSize);
        }


    }
}
