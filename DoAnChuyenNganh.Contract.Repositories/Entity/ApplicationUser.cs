using Microsoft.AspNetCore.Identity;
using DoAnChuyenNganh.Contract.Repositories.Entity;
using DoAnChuyenNganh.Core.Utils;

namespace DoAnChuyenNganh.Repositories.Entity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string Name { get; set; }
        public string? CreatedBy { get; set; }
        public string? LastUpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }
        public virtual ICollection<ApplicationUserLogins> Logins { get; set; } = new List<ApplicationUserLogins>();
        public virtual ICollection<ApplicationUserRoles> UserRoles { get; set; } = new List<ApplicationUserRoles>();
        public ApplicationUser()
        {
            CreatedTime = CoreHelper.SystemTimeNow;
            LastUpdatedTime = CreatedTime;
        }

    }
}
