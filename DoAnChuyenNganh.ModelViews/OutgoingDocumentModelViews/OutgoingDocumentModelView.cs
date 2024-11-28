using Microsoft.AspNetCore.Http;

namespace DoAnChuyenNganh.ModelViews.OutgoingDocumentModelViews
{
    public class OutgoingDocumentModelView
    {
        public enum OutgoingDocumentProcessingStatus
        {
            PendingResponse,
            Responded,
            Overdue
        }
        public string OutgoingDocumentTitle { get; set; }
        public string OutgoingDocumentContent { get; set; }
        public DateTime SendDate { get; set; }
        public string DepartmentId { get; set; }
        public string RecipientEmail { get; set; }
        //public Guid UserId { get; set; }
        public required OutgoingDocumentProcessingStatus OutgoingDocumentProcessingStatuss { get; set; }
        public DateTime DueDate { get; set; }
        public required IFormFile FileScanUrl { get; set; }
    }
}
