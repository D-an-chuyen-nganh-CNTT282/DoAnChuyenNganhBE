using DoAnChuyenNganh.Core.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnChuyenNganh.Contract.Repositories.Entity
{
    public class LecturerActivities : BaseEntity
    {
        public string Name { get; set; }

        public DateTime EventDate { get; set; }

        public string Location { get; set; }

        public string? Description { get; set; }

        public string LecturerId { get; set; }
        [ForeignKey("GiangVienId")]
        public virtual Lecturer? Lecturer { get; set; }
    }
}
