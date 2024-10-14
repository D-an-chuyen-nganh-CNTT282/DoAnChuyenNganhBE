using DoAnChuyenNganh.Core.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnChuyenNganh.Contract.Repositories.Entity
{
    public class ExtracurricularActivities : BaseEntity
    {
        public string StudentId { get; set; }

        public string Name { get; set; }

        public DateTime EventDate { get; set; }
        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; } = null!;
    }
}
