using DoAnChuyenNganh.Core.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnChuyenNganh.Contract.Repositories.Entity
{
    public class AlumniCompany : BaseEntity
    {
        [Key, Column(Order = 1)]
        public string AlumniId { get; set; }

        [Key, Column(Order = 2)]
        public string CompanyId { get; set; }

        public string? Duty { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; } = null!;

        [ForeignKey("AlumniId")]
        public virtual Alumni Alumni { get; set; } = null!;
    }
}
