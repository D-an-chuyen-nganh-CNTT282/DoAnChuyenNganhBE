namespace DoAnChuyenNganh.ModelViews.ResponseDTO
{
    public class OutgoingDocumentResponseDTO
    {
        public string Id { get; set; } 
        public string OutgoingDocumentTitle { get; set; }
        public string OutgoingDocumentContent { get; set; }
        public DateTime SendDate { get; } = DateTime.Now;
        public string DepartmentId { get; set; }
        public string RecipientEmail { get; set; }
        public Guid UserId { get; set; }
        public string OutgoingDocumentProcessingStatuss { get; set; }
        public DateTime DueDate { get; } = DateTime.Now.AddDays(7);
        public string? CreatedBy { get; set; }
        public string? LastUpdatedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset? LastUpdatedTime { get; set; }
        public string FileScanUrl { get; set; }
    }
}
