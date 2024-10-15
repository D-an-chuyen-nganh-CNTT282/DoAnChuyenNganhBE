using DoAnChuyenNganh.Core.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnChuyenNganh.Contract.Repositories.Entity
{
    public class AlumniCompany : BaseEntity
    {
        public string AlumniId { get; set; }
        public string CompanyId { get; set; }
        public string Duty { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; } = null!;
        [ForeignKey("AlumniId")]
        public virtual Alumni Alumni { get; set; } = null!;
    }
}
