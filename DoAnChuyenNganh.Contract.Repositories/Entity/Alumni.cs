﻿using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.Repositories.Entity;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnChuyenNganh.Contract.Repositories.Entity
{
    public class Alumni : BaseEntity
    {
        public required Guid UserId { get; set; }
        public string AlumniName { get; set; }

        public DateTime DayOfBirth { get; set; }

        public string AlumniGender { get; set; }

        public string AlumniAddress { get; set; }

        public string AlumniEmail { get; set; }

        public string AlumniPhone { get; set; }

        public string AlumniMajor { get; set; }

        public string AlumniCourse { get; set; }

        public string PreviousClass { get; set; }

        public DateTime GraduationDay { get; set; }

        public required string LecturerId { get; set; }
        [ForeignKey("LecturerId")]
        public virtual Lecturer Lecturer { get; set; } = null!;

        public virtual ICollection<AlumniActivities> AlumniActivities { get; set; } = new List<AlumniActivities>();

        public virtual ApplicationUser User { get; set; }
    }
}