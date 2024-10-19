
namespace DoAnChuyenNganh.ModelViews.ResponseDTO
{
    public class StudentResponseDTO
    {
        public string Id { get; set; }
        public string StudentName { get; set; }
        public DateTime DayOfBirth { get; set; }
        public string StudentGender { get; set; }
        public string StudentAddress { get; set; }
        public string StudentEmail { get; set; }
        public string StudentPhone { get; set; }
        public string StudentMajor { get; set; }
        public string StudentCourse { get; set; }
        public string Class { get; set; }
        public float GPA { get; set; }
        public required string LecturerId { get; set; }
        public string? CreatedBy { get; set; }
        public string? LastUpdatedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset? LastUpdatedTime { get; set; }
    }
}
