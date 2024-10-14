using DoAnChuyenNganh.Core.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnChuyenNganh.Contract.Repositories.Entity
{
    public class AlumniActivities : BaseEntity
    {
        public string AlumniId { get; set; }

        public string ActivityName { get; set; }

        public DateTime EventDate { get; set; }
        [ForeignKey("AlumniId")]
        public virtual Alumni Alumni { get; set; } = null!;
    }
}
