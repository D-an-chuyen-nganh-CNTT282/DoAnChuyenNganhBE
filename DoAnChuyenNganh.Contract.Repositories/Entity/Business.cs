using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public virtual ICollection<BusinessActivities> BusinessActivities { get; set; } = new List<BusinessActivities>();
    }
}
