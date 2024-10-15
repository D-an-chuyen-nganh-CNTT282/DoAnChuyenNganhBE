using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.Repositories.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnChuyenNganh.Contract.Repositories.Entity
{
    public class TeachingSchedule : BaseEntity
    {
        public Guid UserId { get; set; }
        public required string LecturerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public required string Subject { get; set; }
        public required string Location { get; set; }
        public required string ClassPeriod { get; set; }
        [ForeignKey("LecturerId")]
        public virtual Lecturer Lecturer { get; set; } = null!;
        public virtual ApplicationUser User { get; set; }
    }
}
