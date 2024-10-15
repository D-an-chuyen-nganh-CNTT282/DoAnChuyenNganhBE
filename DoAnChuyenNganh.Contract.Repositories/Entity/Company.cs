using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.Repositories.Entity;

namespace DoAnChuyenNganh.Contract.Repositories.Entity
{
    public class Company : BaseEntity
    {
        public Guid UserId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyPhone { get; set; }
        public string CompanyEmail { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
