using Microsoft.AspNetCore.Mvc;
using DoAnChuyenNganh.ModelViews.AuthModelViews;
using DoAnChuyenNganh.Contract.Services.Interface;
using DoAnChuyenNganh.Core.Base;
using Microsoft.AspNetCore.Authorization;

namespace DoAnChuyenNganhBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpPost("Auth_Account")]
        public async Task<IActionResult> Login(LoginModelView model)
        {
            AuthResponse? result = await _authService.Login(model);
            return Ok(BaseResponse<AuthResponse>.OkResponse(result));
        }

        [Authorize]
        [HttpPatch("Change_Password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            await _authService.ChangePassword(model);
            return Ok(BaseResponse<string>.OkResponse("Đổi mật khẩu thành công"));
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken(RefreshTokenModel model)
        {
            AuthResponse? result = await _authService.RefreshToken(model);
            return Ok(BaseResponse<AuthResponse>.OkResponse(result));
        }
    }
}
