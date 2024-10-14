using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DoAnChuyenNganh.Contract.Repositories.Entity;
using DoAnChuyenNganh.Repositories.Entity;

namespace DoAnChuyenNganh.Repositories.Context
{
    public class DatabaseContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid, ApplicationUserClaims, ApplicationUserRoles, ApplicationUserLogins, ApplicationRoleClaims, ApplicationUserTokens>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        #region Entity
        public virtual DbSet<ApplicationUser> ApplicationUsers => Set<ApplicationUser>();
        public virtual DbSet<ApplicationRole> ApplicationRoles => Set<ApplicationRole>();
        public virtual DbSet<ApplicationUserClaims> ApplicationUserClaims => Set<ApplicationUserClaims>();
        public virtual DbSet<ApplicationUserRoles> ApplicationUserRoles => Set<ApplicationUserRoles>();
        public virtual DbSet<ApplicationUserLogins> ApplicationUserLogins => Set<ApplicationUserLogins>();
        public virtual DbSet<ApplicationRoleClaims> ApplicationRoleClaims => Set<ApplicationRoleClaims>();
        public virtual DbSet<ApplicationUserTokens> ApplicationUserTokens => Set<ApplicationUserTokens>();
        public virtual DbSet<Alumni> Alumni => Set<Alumni>();
        public virtual DbSet<AlumniActivities> AlumniActivities => Set<AlumniActivities>();
        public virtual DbSet<AlumniCompany> AlumniCompany => Set<AlumniCompany>();
        public virtual DbSet<Business> Business => Set<Business>();
        public virtual DbSet<BusinessActivities> BusinessActivities => Set<BusinessActivities>();
        public virtual DbSet<BusinessCollaboration> BusinessCollaboration => Set<BusinessCollaboration>();
        public virtual DbSet<Company> Company => Set<Company>();
        public virtual DbSet<Department> Department => Set<Department>();
        public virtual DbSet<ExtracurricularActivities> ExtracurricularActivities => Set<ExtracurricularActivities>();
        public virtual DbSet<IncomingDocument> IncomingDocument => Set<IncomingDocument>();
        public virtual DbSet<InternshipManagement> InternshipManagement => Set<InternshipManagement>();
        public virtual DbSet<Lecturer> Lecturer => Set<Lecturer>();
        public virtual DbSet<LecturerActivities> LecturerActivities => Set<LecturerActivities>();
        public virtual DbSet<LecturerPlan> LecturerPlan => Set<LecturerPlan>();
        public virtual DbSet<OutgoingDocument> OutgoingDocument => Set<OutgoingDocument>();
        public virtual DbSet<Student> Student => Set<Student>();
        public virtual DbSet<StudentExpectations> StudentExpectations => Set<StudentExpectations>();
        public virtual DbSet<TeachingSchedule> TeachingSchedule => Set<TeachingSchedule>();
        #endregion
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable("Users");
            });

            modelBuilder.Entity<ApplicationRole>(entity =>
            {
                entity.ToTable("Roles");
            });

            modelBuilder.Entity<ApplicationUserClaims>(entity =>
            {
                entity.ToTable("UserClaims");
            });

            modelBuilder.Entity<ApplicationUserRoles>(entity =>
            {
                entity.ToTable("UserRoles");
            });

            modelBuilder.Entity<ApplicationUserLogins>(entity =>
            {
                entity.ToTable("UserLogins");
            });

            modelBuilder.Entity<ApplicationRoleClaims>(entity =>
            {
                entity.ToTable("RoleClaims");
            });

            modelBuilder.Entity<ApplicationUserTokens>(entity =>
            {
                entity.ToTable("UserTokens");
            });

            modelBuilder.Entity<ApplicationUserLogins>()
                .HasKey(l => new { l.UserId, l.LoginProvider, l.ProviderKey });
            modelBuilder.Entity<ApplicationUserRoles>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });
            modelBuilder.Entity<ApplicationUserTokens>()
                .HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

            // Cấu hình khóa chính tổng hợp cho CongTyCuuSinhVien
            modelBuilder.Entity<AlumniCompany>()
                .HasKey(cs => new { cs.AlumniId, cs.CompanyId });

            // Cấu hình quan hệ với CuuSinhVien
            modelBuilder.Entity<AlumniCompany>()
                .HasOne(cs => cs.Alumni)
                .WithMany() // Không cần thuộc tính điều hướng ngược lại trong CuuSinhVien
                .HasForeignKey(cs => cs.AlumniId)
                .OnDelete(DeleteBehavior.Restrict); // Tránh cascade xóa gây ra nhiều đường dẫn cascade

            // Cấu hình quan hệ với CongTy
            modelBuilder.Entity<AlumniCompany>()
                .HasOne(cs => cs.Company)
                .WithMany() // Không cần thuộc tính điều hướng ngược lại trong CongTy
                .HasForeignKey(cs => cs.CompanyId)
                .OnDelete(DeleteBehavior.Restrict); // Tránh cascade xóa gây ra nhiều đường dẫn cascade
                                                    // Quan hệ với bảng SinhVien
            modelBuilder.Entity<InternshipManagement>()
                .HasOne(q => q.Student)
                .WithMany(s => s.InternshipManagement)
                .HasForeignKey(q => q.StudentId)
                .OnDelete(DeleteBehavior.Restrict); // Hoặc DeleteBehavior.NoAction

            // Quan hệ với bảng DoanhNghiep
            modelBuilder.Entity<InternshipManagement>()
                .HasOne(q => q.Business)
                .WithMany(d => d.InternshipManagement)
                .HasForeignKey(q => q.BusinessId)
                .OnDelete(DeleteBehavior.Restrict); // Hoặc DeleteBehavior.NoAction
        }
    }
}