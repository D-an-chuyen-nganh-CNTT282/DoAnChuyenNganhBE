using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.Repositories.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnChuyenNganh.Contract.Repositories.Entity
{
    public class OutgoingDocument : BaseEntity
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

        public required Guid UserId { get; set; }

        public required OutgoingDocumentProcessingStatus OutgoingDocumentProcessingStatuss { get; set; }

        public DateTime DueDate { get; set; }
        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; } = null!;
        public virtual ApplicationUser User { get; set; }
    }
}
