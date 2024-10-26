using AutoMapper;
using DoAnChuyenNganh.Contract.Repositories.Entity;
using DoAnChuyenNganh.Contract.Repositories.Interface;
using DoAnChuyenNganh.Contract.Services.Interface;
using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.Core.Store;
using DoAnChuyenNganh.Core.Utils;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using DoAnChuyenNganh.ModelViews.UserModelViews;
using DoAnChuyenNganh.Repositories.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace DoAnChuyenNganh.Services.Service
{
    public class UserService(IEmailService emailService, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor,
     RoleManager<ApplicationRole> roleManager, IUnitOfWork unitOfWork, SignInManager<ApplicationUser> signInManager, IMapper mapper) : IUserService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IEmailService emailService = emailService;
        private readonly IUnitOfWork unitOfWork = unitOfWork;
        private readonly UserManager<ApplicationUser> userManager = userManager;
        private readonly RoleManager<ApplicationRole> roleManager = roleManager;
        private readonly SignInManager<ApplicationUser> signInManager = signInManager;
        private readonly IMapper _mapper = mapper;
        private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;

        // Cập nhật thông tin người dùng
        public async Task UpdateUser(UserUpdateModelView userUpdateModelView)
        {
            string? userID = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                         ?? throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Token không hợp lệ");
            ApplicationUser? user = await userManager.FindByIdAsync(userID)
              ?? throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, "NotFound", $"Người dùng với id {userID} không tồn tại");

            _mapper.Map(userUpdateModelView, user);
            user.LastUpdatedTime = CoreHelper.SystemTimeNow;
            user.LastUpdatedBy = userID;
            user.UserName = userUpdateModelView.Email;

            await _unitOfWork.GetRepository<ApplicationUser>().UpdateAsync(user);
            await _unitOfWork.SaveAsync();
        }

        // Xóa người dùng
        public async Task DeleteUser(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "User ID không được để trống, trống hoặc chỉ chứa các ký tự không hợp lệ");
            }
            string? handleBy = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
             ?? throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Token không hợp lệ");
            ApplicationUser? userExists = await userManager.FindByIdAsync(userId)
             ?? throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, "Người dùng không tồn tại hoặc đã bị xóa");
            userExists.DeletedTime = CoreHelper.SystemTimeNow;
            userExists.DeletedBy = handleBy;
            await _unitOfWork.GetRepository<ApplicationUser>().UpdateAsync(userExists);
            await _unitOfWork.SaveAsync();
        }
        private readonly Random random = new Random();

        private string GenerateRandomPassword(int length = 8)
        {
            const string upperCaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowerCaseChars = "abcdefghijklmnopqrstuvwxyz";
            const string numbers = "0123456789";
            const string specialChars = "@$!%*?&.";


            if (length < 8)
            {
                length = 8;
            }


            char[] passwordChars = new char[length];
            passwordChars[0] = upperCaseChars[random.Next(upperCaseChars.Length)];
            passwordChars[1] = lowerCaseChars[random.Next(lowerCaseChars.Length)];
            passwordChars[2] = numbers[random.Next(numbers.Length)];
            passwordChars[3] = specialChars[random.Next(specialChars.Length)];


            for (int i = 4; i < length; i++)
            {
                string allChars = upperCaseChars + lowerCaseChars + numbers + specialChars;
                passwordChars[i] = allChars[random.Next(allChars.Length)];
            }

            string password = new string(passwordChars.OrderBy(x => random.Next()).ToArray());


            if (!IsPasswordValid(password))
            {

                return GenerateRandomPassword(length);
            }

            return password;
        }

        private bool IsPasswordValid(string password)
        {
            Regex? regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&.])[A-Za-z\d@$!%*?&.]{8,16}$");
            return regex.IsMatch(password);
        }

        public async Task AddUserWithRoleAsync(UserModelView userModel)
        {
            string? handleBy = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(handleBy))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Token không hợp lệ");
            }
            ApplicationUser? user = await userManager.FindByIdAsync(handleBy);
            ApplicationUser? userExists = await userManager.FindByEmailAsync(userModel.Email);
            if (userExists != null)
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Email đã tồn tại");
            }
            ApplicationUser? newUser = _mapper.Map<ApplicationUser>(userModel);
            newUser.CreatedBy = handleBy;
            newUser.EmailConfirmed = true;
            newUser.Name = userModel.Name;
            newUser.UserName = userModel.Email;
            string passwordChars = GenerateRandomPassword();
            IdentityResult? result = await userManager.CreateAsync(newUser, passwordChars);
            if (result.Succeeded)
            {
                ApplicationRole? role = await roleManager.FindByIdAsync(userModel.RoleId);
                await userManager.AddToRoleAsync(newUser, role.Name);
                string toEmail = userModel.Email;
                string subject = "Cấp tài khoản sử dụng hệ thống";
                string logoUrl = "https://drive.google.com/uc?export=view&id=1i49oPfikilcn0r01zkJGcSJuBg-gJHbY";
                string body = $@"
                <p>Mật khẩu của bạn là: <strong>{passwordChars}</strong>.</p>
                <p>Trân trọng.</p>
                <p>Văn phòng Khoa Công nghệ thông tin - HUIT.</p>
                <p><i>Email này được gửi tự động thông qua hệ thống quản lý học vụ của khoa. Mọi thông tin phản hồi vui lòng gửi qua email người đại diện bên dưới.</i></p>
                <br>
                -------------------------
                <br>
                <table style='width:100%; margin-top:20px;'>
                    <tr>
                        <td style='width:20%; vertical-align:top;'>
                            <img src='{logoUrl}' alt='System Logo' width='150' height='150' style='display:block;'/>
                        </td>
                        <td style='width:80%; vertical-align:top; padding-left:10px;'>
                            <p><strong>Thông tin liên hệ:</strong></p>
                            <p>Đại diện: {user.Name}</p>
                            <p>Email: {user.Email}</p>
                            <p>Điện thoại: {user.PhoneNumber}</p>
                        </td>
                    </tr>
                </table>";
                await emailService.SendEmailAsync(toEmail, subject, body);
                await _unitOfWork.SaveAsync();
            }
            else
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, result.Errors.FirstOrDefault()?.Description);
            }

        }

        public async Task<UserProfileResponseModelView> GetUserProfile()
        {
            string? userIdToken = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
             ?? throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Token không hợp lệ");

            ApplicationUser? user = await userManager.FindByIdAsync(userIdToken)
             ?? throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, "Không tìm thấy người dùng");

            UserProfileResponseModelView? userResponse = _mapper.Map<UserProfileResponseModelView>(user);

            return userResponse;
        }


        public async Task UpdateUserByAdmin(string userID, UserUpdateByAdminModel model)
        {
            if (string.IsNullOrWhiteSpace(userID))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "User ID không được để trống, trống hoặc chỉ chứa các ký tự không hợp lệ");
            }

            string? handleBy = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
             ?? throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Token không hợp lệ");
            ApplicationUser? user = await userManager.FindByIdAsync(handleBy);
            ApplicationUser? userExists = await userManager.FindByIdAsync(userID)
             ?? throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, "Người dùng không tồn tại hoặc đã bị xóa");

            _mapper.Map(model, userExists);

            userExists.UserName = model.Email;
            userExists.LastUpdatedBy = handleBy;
            string toEmail = model.Email;
            string subject = "Cấp lại mật khẩu cho tài khoản";
            string logoUrl = "https://drive.google.com/uc?export=view&id=1i49oPfikilcn0r01zkJGcSJuBg-gJHbY";
            string body = $@"
                <p>Mật khẩu của bạn vừa được thay đổi: <strong>{model.Password}</strong>.</p>
                <p>Vui lòng đăng nhập theo mật khẩu vừa được cấp và thay đổi lại theo ý bạn tại giao diện Đổi mật khẩu.</p>
                <p>Trân trọng.</p>
                <p>Văn phòng Khoa Công nghệ thông tin - HUIT.</p>
                <p><i>Email này được gửi tự động thông qua hệ thống quản lý học vụ của khoa. Mọi thông tin phản hồi vui lòng gửi qua email người đại diện bên dưới.</i></p>
                <br>
                -------------------------
                <br>
                <table style='width:100%; margin-top:20px;'>
                    <tr>
                        <td style='width:20%; vertical-align:top;'>
                            <img src='{logoUrl}' alt='System Logo' width='150' height='150' style='display:block;'/>
                        </td>
                        <td style='width:80%; vertical-align:top; padding-left:10px;'>
                            <p><strong>Thông tin liên hệ:</strong></p>
                            <p>Đại diện: {user.Name}</p>
                            <p>Email: {user.Email}</p>
                            <p>Điện thoại: {user.PhoneNumber}</p>
                        </td>
                    </tr>
                </table>";
            await emailService.SendEmailAsync(toEmail, subject, body);

            if (!string.IsNullOrEmpty(model.Password))
            {
                string? passwordResetToken = await userManager.GeneratePasswordResetTokenAsync(userExists);
                IdentityResult? result = await userManager.ResetPasswordAsync(userExists, passwordResetToken, model.Password);
                if (!result.Succeeded)
                {
                    throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Không thể cập nhật mật khẩu");
                }
            }

            IdentityResult? updateResult = await userManager.UpdateAsync(userExists);
            if (!updateResult.Succeeded)
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Không thể cập nhật người dùng");
            }
        }
        private IQueryable<ApplicationUser> FilterUsers(
            IQueryable<ApplicationUser> query,
            string? name,
            string? phone,
            string? email,
            string? role)
        {
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(u => u.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(phone))
            {
                query = query.Where(u => u.PhoneNumber.Contains(phone));
            }

            if (!string.IsNullOrEmpty(email))
            {
                query = query.Where(u => u.Email.Contains(email));
            }

            if (!string.IsNullOrEmpty(role))
            {
                IList<ApplicationUser>? usersInRole = userManager.GetUsersInRoleAsync(role).Result;
                query = query.Where(u => usersInRole.Contains(u));
            }

            return query;
        }
        private IQueryable<ApplicationUser> SortUsers(IQueryable<ApplicationUser> query,
            SortBy sortBy,
            SortOrder sortOrder)
        {

            switch (sortBy)
            {
                case SortBy.Name:
                    query = sortOrder == SortOrder.asc
                        ? query.OrderBy(u => u.Name)
                        : query.OrderByDescending(u => u.Name);
                    break;

                case SortBy.Email:
                    query = sortOrder == SortOrder.asc
                        ? query.OrderBy(u => u.Email)
                        : query.OrderByDescending(u => u.Email);
                    break;

                case SortBy.PhoneNumber:
                    query = sortOrder == SortOrder.asc
                        ? query.OrderBy(u => u.PhoneNumber)
                        : query.OrderByDescending(u => u.PhoneNumber);
                    break;

                case SortBy.RoleName:
                    query = sortOrder == SortOrder.asc
                        ? query.OrderBy(u => u.UserRoles.FirstOrDefault().Role.Name)
                        : query.OrderByDescending(u => u.UserRoles.FirstOrDefault().Role.Name);
                    break;

                case SortBy.CreatedDate:
                    query = sortOrder == SortOrder.asc
                        ? query.OrderBy(u => u.CreatedTime)
                        : query.OrderByDescending(u => u.CreatedTime);
                    break;

                default:
                    break;
            }


            return query;
        }
        private async Task<BasePaginatedList<UserResponseDTO>> PaginateUsers(
        IQueryable<ApplicationUser> query,
        int? page,
        int? pageSize)
        {
            int currentPage = page ?? 1;
            int currentPageSize = pageSize ?? 10;
            int totalItems = await query.CountAsync();

            List<UserResponseDTO>? users = await query
                .Skip((currentPage - 1) * currentPageSize)
                .Take(currentPageSize)
                .Select(u => new UserResponseDTO
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    RoleName = u.UserRoles.FirstOrDefault().Role.Name,
                    CreatedBy = u.CreatedBy,
                    LastUpdatedBy = u.LastUpdatedBy,
                    CreatedTime = u.CreatedTime,
                    LastUpdatedTime = u.LastUpdatedTime,
                })
                .ToListAsync();

            return new BasePaginatedList<UserResponseDTO>(users, totalItems, currentPage, currentPageSize);
        }


        public async Task<BasePaginatedList<UserResponseDTO>> GetAsync(
            int? page,
            int? pageSize,
            string? name,
            string? phone,
            string? email,
            SortBy sortBy,
            SortOrder sortOrder,
            string? role,
            string? id
            )
        {
            IQueryable<ApplicationUser>? query = userManager.Users.AsQueryable();
            query = query.Where(u => u.DeletedTime == null);
            if (!string.IsNullOrEmpty(id))
            {
                query = query.Where(u => u.Id == Guid.Parse(id));
            }

            query = FilterUsers(query, name, phone, email, role);
            query = SortUsers(query, sortBy, sortOrder);

            return await PaginateUsers(query, page, pageSize);
        }

    }
}
