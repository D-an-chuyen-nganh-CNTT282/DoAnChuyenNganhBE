using DoAnChuyenNganh.Core.Base;

namespace DoAnChuyenNganh.Contract.Repositories.Entity
{
    public class Lecturer : BaseEntity
    {
        public string LecturerName { get; set; }

        public DateTime DayOfBirth { get; set; }
        public string LecturerGender { get; set; }
        public string LecturerEmail { get; set; }
        public string LecturerPhone { get; set; }
        public string LecturerAddress { get; set; }

        public string Expertise { get; set; }

        public string? PersonalWebsiteLink { get; set; } //Lưu link giáo án, bài viết,...


        public virtual ICollection<Alumni> Alumni { get; set; } = new List<Alumni>();
        public virtual ICollection<TeachingSchedule> TeachingSchedule { get; set; } = new List<TeachingSchedule>();
        public virtual ICollection<Student> Student { get; set; } = new List<Student>();
        public virtual ICollection<Business> Business { get; set; } = new List<Business>();
        public virtual ICollection<LecturerPlan> LecturerPlan { get; set; } = new List<LecturerPlan>();
    }
}
