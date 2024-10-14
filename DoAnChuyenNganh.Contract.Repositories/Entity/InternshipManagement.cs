﻿using DoAnChuyenNganh.Core.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnChuyenNganh.Contract.Repositories.Entity
{
    public class InternshipManagement : BaseEntity
    {
        public string StudentId { get; set; }

        public string BusinessId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Remark { get; set; }

        [Range(1, 5, ErrorMessage = "Đánh giá từ 0 đến 10 điểm")]
        public int Rating { get; set; }

        [ForeignKey("BusinessId")]

        public virtual Business Business { get; set; } = null!;
        [ForeignKey("StudentId")]

        public virtual Student Student { get; set; } = null!;
    }
}
