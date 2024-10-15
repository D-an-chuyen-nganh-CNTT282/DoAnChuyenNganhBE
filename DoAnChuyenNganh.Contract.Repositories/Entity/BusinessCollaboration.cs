using DoAnChuyenNganh.Core.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnChuyenNganh.Contract.Repositories.Entity
{
    public class BusinessCollaboration : BaseEntity
    {
        public enum ProjectStatus
        {
            UnderDiscussion,
            PendingApproval,
            Approved,
            InProgress,
            OnHold,
            Completed,
            Ended
        }
        public string BusinessId { get; set; }

        public string ProjectName { get; set; }

        public required ProjectStatus ProjectStatuss { get; set; }

        public string? Result { get; set; }

        [ForeignKey("BusinessId")]
        public virtual Business Business { get; set; } = null!;
    }
}
