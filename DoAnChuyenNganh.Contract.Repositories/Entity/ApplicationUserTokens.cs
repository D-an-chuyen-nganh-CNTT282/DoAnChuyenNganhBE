﻿using Microsoft.AspNetCore.Identity;
using DoAnChuyenNganh.Core.Utils;
using DoAnChuyenNganh.Repositories.Entity;

namespace DoAnChuyenNganh.Contract.Repositories.Entity
{
    public class ApplicationUserTokens : IdentityUserToken<Guid>
    {
        public string? CreatedBy { get; set; }
        public string? LastUpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }
        public virtual ApplicationUser User { get; set; }
        public ApplicationUserTokens()
        {
            CreatedTime = CoreHelper.SystemTimeNow;
            LastUpdatedTime = CreatedTime;
        }
    }
}