using AutoMapper;
using DoAnChuyenNganh.Contract.Repositories.Entity;
using DoAnChuyenNganh.Contract.Repositories.Interface;
using DoAnChuyenNganh.Contract.Services.Interface;
using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.Core.Store;
using DoAnChuyenNganh.ModelViews.AuthModelViews;
using DoAnChuyenNganh.Repositories.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DoAnChuyenNganh.Services.Service
{
    public class AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
      IMapper mapper, IHttpContextAccessor httpContextAccessor,
      RoleManager<ApplicationRole> roleManager, IEmailService emailService, IMemoryCache memoryCache,
      IUnitOfWork unitOfWork) : IAuthService
    {
        private readonly UserManager<ApplicationUser> userManager = userManager;
        private readonly SignInManager<ApplicationUser> signInManager = signInManager;
        private readonly RoleManager<ApplicationRole> roleManager = roleManager;
        private readonly IUnitOfWork unitOfWork = unitOfWork;
        private readonly IEmailService emailService = emailService;
        private readonly IMapper mapper = mapper;
        private readonly IMemoryCache memoryCache = memoryCache;
        private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;
        #region Private Service
        private async Task<ApplicationUser> CheckRefreshToken(string refreshToken)
        {

            List<ApplicationUser>? users = await userManager.Users.ToListAsync();
            foreach (ApplicationUser user in users)
            {
                string? storedToken = await userManager.GetAuthenticationTokenAsync(user, "Default", "RefreshToken");

                if (storedToken == refreshToken)
                {
                    return user;
                }
            }
            throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Token không hợp lệ");
        }
        private (string token, IEnumerable<string> roles) GenerateJwtToken(ApplicationUser user)
        {
            byte[] key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY") ?? throw new Exception("JWT_KEY is not set"));
            List<Claim> claims = new List<Claim> {
            new(ClaimTypes.NameIdentifier,user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        };
            IEnumerable<string> roles = userManager.GetRolesAsync(user: user).Result;
            foreach (string role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? throw new Exception("JWT_ISSUER is not set"),
                Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? throw new Exception("JWT_AUDIENCE is not set"),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return (tokenHandler.WriteToken(token), roles);
        }
        private async Task<string> GenerateRefreshToken(ApplicationUser user)
        {
            string? refreshToken = Guid.NewGuid().ToString();

            string? initToken = await userManager.GetAuthenticationTokenAsync(user, "Default", "RefreshToken");
            if (initToken != null)
            {

                await userManager.RemoveAuthenticationTokenAsync(user, "Default", "RefreshToken");

            }

            await userManager.SetAuthenticationTokenAsync(user, "Default", "RefreshToken", refreshToken);
            return refreshToken;
        }
        private string GenerateOtp()
        {
            Random random = new Random();
            string otp = random.Next(100000, 999999).ToString();
            return otp;
        }
        #endregion


        #region Implementation Interface
        public async Task<AuthResponse> Login(LoginModelView loginModel)
        {
            ApplicationUser? user = await userManager.FindByEmailAsync(loginModel.Email)
             ?? throw new BaseException.ErrorException(DoAnChuyenNganh.Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, "Không tìm thấy user");

            if (user.DeletedTime.HasValue)
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Tài khoản đã bị xóa");
            }
            if (!await userManager.IsEmailConfirmedAsync(user))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Tài khoản chưa được xác nhận");
            }
            SignInResult result = await signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, false, false);
            if (!result.Succeeded)
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Mật khẩu không đúng");
            }
            (string token, IEnumerable<string> roles) = GenerateJwtToken(user);
            string refreshToken = await GenerateRefreshToken(user);
            return new AuthResponse
            {
                AccessToken = token,
                RefreshToken = refreshToken,
                TokenType = "JWT",
                AuthType = "Bearer",
                ExpiresIn = DateTime.UtcNow.AddHours(1),
                User = new UserInfo
                {
                    Email = user.Email,
                    Roles = roles.ToList()
                }
            };

        }
        public async Task ChangePassword(ChangePasswordModel model)
        {
            string? userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Token không hợp lệ");
            ApplicationUser? admin = await userManager.FindByIdAsync(userId) ?? throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, "Không tìm thấy user");
            if (admin.DeletedTime.HasValue)
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Tài khoản đã bị xóa");
            }
            else
            {
                IdentityResult result = await userManager.ChangePasswordAsync(admin, model.OldPassword, model.NewPassword);
                if (!result.Succeeded)
                {
                    throw new BaseException.ErrorException(Core.Store.StatusCodes.ServerError, ErrorCode.ServerError, result.Errors.FirstOrDefault()?.Description);
                }
            }

        }
        public async Task<AuthResponse> RefreshToken(RefreshTokenModel refreshTokenModel)
        {
            ApplicationUser? user = await CheckRefreshToken(refreshTokenModel.refreshToken);
            (string token, IEnumerable<string> roles) = GenerateJwtToken(user);
            string refreshToken = await GenerateRefreshToken(user);
            return new AuthResponse
            {
                AccessToken = token,
                RefreshToken = refreshToken,
                TokenType = "JWT",
                AuthType = "Bearer",
                ExpiresIn = DateTime.UtcNow.AddHours(1),
                User = new UserInfo
                {
                    Email = user.Email,
                    Roles = roles.ToList()
                }
            };
        }
        #endregion
    }
}
