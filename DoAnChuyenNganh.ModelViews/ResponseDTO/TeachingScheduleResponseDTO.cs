namespace DoAnChuyenNganh.ModelViews.ResponseDTO
{
    public class TeachingScheduleResponseDTO
    {
        public string Id { get; set; }
        public required string LecturerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public required string Subject { get; set; }
        public required string Location { get; set; }
        public required string ClassPeriod { get; set; }
        //public string? CreatedBy { get; set; }
        //public string? LastUpdatedBy { get; set; }
        //public DateTimeOffset CreatedTime { get; set; }
        //public DateTimeOffset? LastUpdatedTime { get; set; }
    }
}
