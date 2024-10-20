namespace DoAnChuyenNganh.ModelViews.ResponseDTO
{
    public class StudentExpectationsResponseDTO
    {
        public string Id { get; set; }
        public string StudentId { get; set; }
        public string RequestCategory { get; set; }
        public Guid UserId { get; set; }
        public string ProcessingStatus { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public string? FileScanUrl { get; set; }
        public string? CreatedBy { get; set; }
        public string? LastUpdatedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset? LastUpdatedTime { get; set; }
    }
}