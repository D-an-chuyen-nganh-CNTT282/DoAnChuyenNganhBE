﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnChuyenNganh.ModelViews.ResponseDTO
{
    public class UserResponseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string RoleName { get; set; }
        public string? CreatedBy { get; set; }
        public string? LastUpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }
    }
}