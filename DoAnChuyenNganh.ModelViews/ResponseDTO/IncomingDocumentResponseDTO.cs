namespace DoAnChuyenNganh.ModelViews.ResponseDTO
{
    public class IncomingDocumentResponseDTO
    {
        public string Id {  get; set; }
        public string IncomingDocumentTitle { get; set; }
        public string IncomingDocumentContent { get; set; }
        public DateTime ReceivedDate { get; set; }
        public string DepartmentId { get; set; }
        public string IncomingDocumentProcessingStatuss { get; set; }
        public string FileScanUrl { get; set; }
        public DateTime DueDate { get; set; }
        public Guid UserId { get; set; }
        public string? CreatedBy { get; set; }
        public string? LastUpdatedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset? LastUpdatedTime { get; set; }
    }
}
