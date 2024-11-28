using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DoAnChuyenNganh.ModelViews.StudentExpectationsModelViews
{
    public class StudentExpectationsModelView
    {
        public enum ProcessingStatus
        {
            Pending,
            Received,
            InProcess,
            Processed
        }
        [Required]
        public string StudentId { get; set; }
        public required string RequestCategory { get; set; }
        //public Guid UserId { get; set; }
        public required ProcessingStatus ProcessingStatuss { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public required IFormFile FileScanUrl { get; set; }
    }
}

