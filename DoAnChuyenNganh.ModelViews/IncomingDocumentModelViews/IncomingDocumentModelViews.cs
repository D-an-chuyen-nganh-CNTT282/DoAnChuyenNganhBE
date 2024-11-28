using Microsoft.AspNetCore.Http;

namespace DoAnChuyenNganh.ModelViews.IncomingDocumentModelViews
{
    public class IncomingDocumentModelViews
    {
        public enum IncomingDocumentProcessingStatus
        {
            Received,
            InProcess,
            Completed,
            Overdue
        }
        public string IncomingDocumentTitle { get; set; }

        public string IncomingDocumentContent { get; set; }

        public DateTime? ReceivedDate { get; set; }

        public string DepartmentId { get; set; }

        public required IFormFile FileScanUrl { get; set; }

        public required IncomingDocumentProcessingStatus IncomingDocumentProcessingStatuss { get; set; }

        public DateTime? DueDate { get; set; } 
    }
}
