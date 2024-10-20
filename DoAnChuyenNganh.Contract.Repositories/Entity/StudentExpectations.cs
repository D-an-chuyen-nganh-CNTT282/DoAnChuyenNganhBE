using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.Repositories.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnChuyenNganh.Contract.Repositories.Entity
{
    public class StudentExpectations : BaseEntity
    {
        public enum ProcessingStatus
        {
            Pending,
            Received,
            InProcess,
            Processed
        }

        public string StudentId { get; set; }

        public required string RequestCategory { get; set; }

        public Guid UserId { get; set; }

        public required ProcessingStatus ProcessingStatuss { get; set; }

        public DateTime RequestDate { get; set; }

        public DateTime? CompletionDate { get; set; }
        public required string FileScanUrl { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        public virtual Student Student { get; set; } = null!;
    }
}
