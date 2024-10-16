using DoAnChuyenNganh.Core.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnChuyenNganh.Contract.Repositories.Entity
{
    public class Student : BaseEntity
    {
        public string StudentName { get; set; }
        public DateTime DayOfBirth { get; set; }
        public string StudentGender { get; set; }
        public string StudentAddress { get; set; }
        public string StudentEmail { get; set; }
        public string StudentPhone { get; set; }
        public string StudentMajor { get; set; }
        public string StudentCourse { get; set; }
        public string Class { get; set; }
        public float GPA { get; set; }
        public required string LecturerId { get; set; }
        [ForeignKey("LecturerId")]
        public virtual Lecturer Lecturer { get; set; } = null!;
        public virtual ICollection<StudentExpectations> StudentExpectations { get; set; } = new List<StudentExpectations>();
    }
}
