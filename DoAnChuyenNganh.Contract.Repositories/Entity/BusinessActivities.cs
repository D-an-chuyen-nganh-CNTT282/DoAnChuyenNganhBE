using DoAnChuyenNganh.Core.Base;

namespace DoAnChuyenNganh.Contract.Repositories.Entity
{
    public class BusinessActivities : BaseEntity
    {
        public string BusinessId { get; set; }
        public virtual Business Business { get; set; }
        public string ActivitiesId { get; set; }
        public virtual Activities Activities { get; set; }
    }
}
