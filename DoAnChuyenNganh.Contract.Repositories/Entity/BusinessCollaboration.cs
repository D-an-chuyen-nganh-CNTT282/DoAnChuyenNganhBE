using DoAnChuyenNganh.Core.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnChuyenNganh.Contract.Repositories.Entity
{
    public class BusinessCollaboration : BaseEntity
    {
        public enum ProjectStatus
        {
            UnderDiscussion, //Đang thảo luận
            PendingApproval, //Chờ phê duyệt
            Approved, //Phê duyệt
            InProgress, //Đang tiến hành
            OnHold, //Tạm hoãn
            Completed, //Hoàn thành
            Ended //Kết thúc
        }
        public string BusinessId { get; set; }

        public string ProjectName { get; set; }

        public required ProjectStatus ProjectStatuss { get; set; }

        public string? Result { get; set; }

        [ForeignKey("BusinessId")]
        public virtual Business Business { get; set; } = null!;
    }
}
