using DoAnChuyenNganh.Core.Base;
using System.ComponentModel.DataAnnotations;

namespace DoAnChuyenNganh.Contract.Repositories.Entity
{
    public class InternshipManagement : BaseEntity
    {
        public string StudentId { get; set; }
        public virtual Student Student { get; set; }
        public string BusinessId { get; set; }
        public virtual Business Business { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Remark { get; set; }
        [Range(1, 10, ErrorMessage = "Đánh giá từ 0 đến 10 điểm")]
        public int? Rating { get; set; }
        
    }
}
