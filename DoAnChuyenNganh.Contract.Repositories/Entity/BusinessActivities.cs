using DoAnChuyenNganh.Core.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnChuyenNganh.Contract.Repositories.Entity
{
    public class BusinessActivities : BaseEntity
    {
        public string Name { get; set; }

        public DateTime EventDate { get; set; }

        public required string Location { get; set; }

        public string? Description { get; set; }

        public string BusinessId { get; set; }
        [ForeignKey("BusinessId")]
        public virtual Business Business { get; set; } = null!;
    }
}
