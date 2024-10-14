using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnChuyenNganh.Contract.Repositories.Entity
{
    public class IncomingDocument : BaseEntity
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

        public DateTime ReceivedDate { get; set; }

        public string DepartmentId { get; set; }

        public required string FileScanUrl { get; set; }

        public required IncomingDocumentProcessingStatus IncomingDocumentProcessingStatuss { get; set; }

        public required Guid UserId { get; set; }

        public DateTime DueDate { get; set; }
        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; } = null!;
        public virtual ApplicationUser User { get; set; }
    }
}
