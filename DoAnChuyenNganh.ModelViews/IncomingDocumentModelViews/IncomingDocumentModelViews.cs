namespace DoAnChuyenNganh.ModelViews.IncomingDocumentModelViews
{
    public class IncomingDocumentModelViews
    {
        public string IncomingDocumentTitle { get; set; }
        public string IncomingDocumentContent { get; set; }
        public DateTime ReceivedDate { get; set; }
        public string DepartmentId { get; set; }
        public string IncomingDocumentProcessingStatuss { get; set; }
        public required string FileScanUrl { get; set; }
        //public required Guid UserId { get; set; }
    }
}
