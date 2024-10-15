using DoAnChuyenNganh.Core.Base;

namespace DoAnChuyenNganh.Contract.Repositories.Entity
{
    public class ExtracurricularActivities : BaseEntity
    {   
        public string StudentId { get; set; }
        public virtual Student Student { get; set; }
        public string ActivitiesId { get; set; }
        public virtual Activities Activities { get; set; }
    }
}
