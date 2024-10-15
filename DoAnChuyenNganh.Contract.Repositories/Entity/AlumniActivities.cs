using DoAnChuyenNganh.Core.Base;

namespace DoAnChuyenNganh.Contract.Repositories.Entity
{
    public class AlumniActivities : BaseEntity
    {
        public string AlumniId { get; set; }
        public virtual Alumni Alumni { get; set; }
        public string ActivitiesId { get; set; }
        public virtual Activities Activities { get; set; }
    }
}
