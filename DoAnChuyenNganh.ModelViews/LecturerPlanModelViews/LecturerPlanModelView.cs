﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnChuyenNganh.ModelViews.LecturerPlanModelViews
{
    public class LecturerPlanModelView
    {
        public Guid UserId { get; set; }
        public string LecturerId { get; set; }
        public required string PlanContent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Note { get; set; }
    }
}
