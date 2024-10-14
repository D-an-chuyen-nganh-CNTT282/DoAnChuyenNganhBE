using DoAnChuyenNganh.ModelViews.AuthModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnChuyenNganh.Contract.Services.Interface
{
    public interface IAuthService
    {
        Task<AuthResponse> Login(LoginModelView loginModel);
        Task<AuthResponse> RefreshToken(RefreshTokenModel refreshTokenModel);
        Task Register(RegisterModelView registerModelView);
        Task VerifyOtp(ConfirmOTPModel model, bool isResetPassword);
        Task ResendConfirmationEmail(EmailModelView emailModelView);
        Task ChangePassword(ChangePasswordModel model);
        Task ForgotPassword(EmailModelView emailModelView);
        Task ResetPassword(ResetPasswordModel resetPassword);
    }
}
