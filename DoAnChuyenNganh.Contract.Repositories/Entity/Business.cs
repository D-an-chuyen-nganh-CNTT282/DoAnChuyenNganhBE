using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.Repositories.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnChuyenNganh.Contract.Repositories.Entity
{
    public class Business : BaseEntity
    {
        public Guid UserId { get; set; }
        public string LecturerId { get; set; }

        public string BusinessName { get; set; }

        public string BusinessAddress { get; set; }

        public string BusinessPhone { get; set; }

        public string BusinessEmail { get; set; }

        public virtual ICollection<BusinessCollaboration> BusinessCollaboration { get; set; } = new List<BusinessCollaboration>();
        public virtual ApplicationUser User { get; set; }
        [ForeignKey("LecturerId")]
        public virtual Lecturer Lecturer { get; set; } = null!;
        public virtual ICollection<InternshipManagement> InternshipManagement { get; set; } = new List<InternshipManagement>();
    }
}
