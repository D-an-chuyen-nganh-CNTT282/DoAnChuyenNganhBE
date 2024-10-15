using DoAnChuyenNganh.ModelViews.AuthModelViews;

namespace DoAnChuyenNganh.Contract.Services.Interface
{
    public interface IAuthService
    {
        Task<AuthResponse> Login(LoginModelView loginModel);
        Task<AuthResponse> RefreshToken(RefreshTokenModel refreshTokenModel);
        Task ChangePassword(ChangePasswordModel model);
    }
}
