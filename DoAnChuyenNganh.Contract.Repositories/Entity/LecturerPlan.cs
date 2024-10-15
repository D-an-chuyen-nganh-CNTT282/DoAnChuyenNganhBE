using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.Repositories.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnChuyenNganh.Contract.Repositories.Entity
{
    public class LecturerPlan : BaseEntity
    {
        public Guid UserId { get; set; }
        public string LecturerId { get; set; }
        public required string PlanContent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Note { get; set; }

        [ForeignKey("LecturerId")]
        public virtual Lecturer Lecturer { get; set; } = null!;
        public virtual ApplicationUser User { get; set; }
    }
}
