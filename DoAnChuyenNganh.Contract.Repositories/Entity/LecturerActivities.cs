using DoAnChuyenNganh.Core.Base;

namespace DoAnChuyenNganh.Contract.Repositories.Entity
{
    public class LecturerActivities : BaseEntity
    {
        public string LecturerId { get; set; }
        public virtual Lecturer Lecturer { get; set; }
        public string ActivitiesId { get; set; }
        public virtual Activities Activities { get; set; }
    }
}
