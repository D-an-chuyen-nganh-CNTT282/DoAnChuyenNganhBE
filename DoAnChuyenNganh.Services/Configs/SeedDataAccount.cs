using DoAnChuyenNganh.Contract.Repositories.Entity;
using DoAnChuyenNganh.Repositories.Entity;
using DoAnChuyenNganh.Services.Configs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace DoAnChuyenNganh.Contract.Services.Configs
{
    public class SeedDataAccount
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            UserManager<ApplicationUser> userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            RoleManager<ApplicationRole> roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            DepartmentManager departmentManager = serviceProvider.GetRequiredService<DepartmentManager>();
            LecturerManager lecturerManager = serviceProvider.GetRequiredService<LecturerManager>();

            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new ApplicationRole { Name = "Admin" });
            }
            string emailAdmin = "admin@gmail.com";
            string passwordAdmin = "Admin123.";
            string nameAdmin = "Admin";
            ApplicationUser? adminAccount = await userManager.FindByEmailAsync(emailAdmin);
            if (adminAccount is null)
            {
                ApplicationUser? newAccount = new ApplicationUser
                {
                    UserName = emailAdmin,
                    Email = emailAdmin,
                    EmailConfirmed = true,
                    Name = nameAdmin
                };
                await userManager.CreateAsync(newAccount, passwordAdmin);
                await userManager.AddToRoleAsync(newAccount, "Admin");
            }

            if (!await roleManager.RoleExistsAsync("Trưởng khoa"))
            {
                await roleManager.CreateAsync(new ApplicationRole { Name = "Trưởng khoa" });
            }
            string emailTk = "tk@gmail.com";
            string passwordTk = "Tk12345.";
            string nameTk = "Trưởng khoa";
            ApplicationUser? TkAccount = await userManager.FindByEmailAsync(emailTk);
            if (TkAccount is null)
            {
                ApplicationUser? newAccount = new ApplicationUser
                {
                    UserName = emailTk,
                    Email = emailTk,
                    EmailConfirmed = true,
                    Name = nameTk,
                };
                await userManager.CreateAsync(newAccount, passwordTk);
                await userManager.AddToRoleAsync(newAccount, "Trưởng khoa");
            }

            if (!await roleManager.RoleExistsAsync("Phó trưởng khoa"))
            {
                await roleManager.CreateAsync(new ApplicationRole { Name = "Phó trưởng khoa" });
            }
            string emailPtk = "ptk@gmail.com";
            string passwordPtk = "Ptk1234.";
            string namePtk = "Phó trưởng khoa";
            ApplicationUser? PtkAccount = await userManager.FindByEmailAsync(emailPtk);
            if (PtkAccount is null)
            {
                ApplicationUser? newAccount = new ApplicationUser
                {
                    UserName = emailPtk,
                    Email = emailPtk,
                    EmailConfirmed = true,
                    Name = namePtk,
                };
                await userManager.CreateAsync(newAccount, passwordPtk);
                await userManager.AddToRoleAsync(newAccount, "Phó trưởng khoa");
            }

            if (!await roleManager.RoleExistsAsync("Giáo vụ khoa"))
            {
                await roleManager.CreateAsync(new ApplicationRole { Name = "Giáo vụ khoa" });
            }
            string emailGvk = "gvk@gmail.com";
            string passwordGvk = "Gvk1234.";
            string nameGvk = "Giáo vụ khoa";
            ApplicationUser? GvkAccount = await userManager.FindByEmailAsync(emailGvk);
            if (GvkAccount is null)
            {
                ApplicationUser? newAccount = new ApplicationUser
                {
                    UserName = emailGvk,
                    Email = emailGvk,
                    EmailConfirmed = true,
                    Name = nameGvk
                };
                await userManager.CreateAsync(newAccount, passwordGvk);
                await userManager.AddToRoleAsync(newAccount, "Giáo vụ khoa");
            }

            if (!await roleManager.RoleExistsAsync("Giảng viên"))
            {
                await roleManager.CreateAsync(new ApplicationRole { Name = "Giảng viên" });
            }
            string emailGv = "gv@gmail.com";
            string passwordGv = "Gv12345.";
            string nameGv = "Giảng viên";
            ApplicationUser? GvAccount = await userManager.FindByEmailAsync(emailGv);
            if (GvAccount is null)
            {
                ApplicationUser? newAccount = new ApplicationUser
                {
                    UserName = emailGv,
                    Email = emailGv,
                    EmailConfirmed = true,
                    Name = nameGv
                };
                await userManager.CreateAsync(newAccount, passwordGv);
                await userManager.AddToRoleAsync(newAccount, "Giảng viên");
            }

            if (!await roleManager.RoleExistsAsync("Trưởng bộ môn"))
            {
                await roleManager.CreateAsync(new ApplicationRole { Name = "Trưởng bộ môn" });
            }
            string emailTbm = "tbm@gmail.com";
            string passwordTbm = "Tbm1234.";
            string nameTbm = "Trưởng bộ môn";
            ApplicationUser? TbmAccount = await userManager.FindByEmailAsync(emailTbm);
            if (TbmAccount is null)
            {
                ApplicationUser? newAccount = new ApplicationUser
                {
                    UserName = emailTbm,
                    Email = emailTbm,
                    EmailConfirmed = true,
                    Name = nameTbm
                };
                await userManager.CreateAsync(newAccount, passwordTbm);
                await userManager.AddToRoleAsync(newAccount, "Trưởng bộ môn");
            }
            //Seed data department
            await departmentManager.CreateIfNotExistsAsync("Phòng Công tác Sinh viên và Thanh tra giáo dục");
            await departmentManager.CreateIfNotExistsAsync("Phòng Đào tạo");
            await departmentManager.CreateIfNotExistsAsync("Phòng Hợp tác Quốc tế");
            await departmentManager.CreateIfNotExistsAsync("Phòng Kế hoạch - Tài chính");
            await departmentManager.CreateIfNotExistsAsync("Phòng Khoa học công nghệ");
            await departmentManager.CreateIfNotExistsAsync("Phòng Quản lý Sau đại học");
            await departmentManager.CreateIfNotExistsAsync("Phòng Quản trị - Thiết bị");
            await departmentManager.CreateIfNotExistsAsync("Phòng Tổ chức - Hành chính");
            await departmentManager.CreateIfNotExistsAsync("Công đoàn trường");
            await departmentManager.CreateIfNotExistsAsync("Đoàn thanh niên - Hội sinh viên");
            await departmentManager.CreateIfNotExistsAsync("Hội đồng Khoa học - Đào tạo");
            await departmentManager.CreateIfNotExistsAsync("Hội đồng CDGS");
            //Seed data lecturer
            await lecturerManager.CreateIfNotExistsAsync("Trưởng khoa", "1970-06-22 00:00", "Nam", "tk@gmail.com", "0938747828", "TP HCM", "Khoa học máy tính & Trí tuệ nhân tạo", null);
            await lecturerManager.CreateIfNotExistsAsync("Phó trưởng khoa", "1972-08-10 00:00", "Nam", "ptk@gmail.com", "0934758373", "TP HCM", "Công nghệ phần mềm", null);
            await lecturerManager.CreateIfNotExistsAsync("Trưởng bộ môn", "1974-07-15 00:00", "Nữ", "tbm@gmail.com", "0823747839", "TP HCM", "Mạng máy tính", null);
            await lecturerManager.CreateIfNotExistsAsync("Giảng viên", "1980-05-25 00:00", "Nam", "gv@gmail.com", "0928747828", "TP HCM", "Hệ thống thông tin", null);
            await lecturerManager.CreateIfNotExistsAsync("Giáo vụ khoa", "1986-12-21 00:00", "Nữ", "gvk@gmail.com", "0987263717", "TP HCM", "Công nghệ phần mềm", null);
        }
    }
}
