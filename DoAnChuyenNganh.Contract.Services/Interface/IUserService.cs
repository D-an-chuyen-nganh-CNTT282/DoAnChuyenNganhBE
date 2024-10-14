using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using DoAnChuyenNganh.ModelViews.UserModelViews;

namespace DoAnChuyenNganh.Contract.Services.Interface
{
    public interface IUserService
    {
        Task UpdateUser(UserUpdateModelView userUpdateModelView);
        Task DeleteUser(string userId);
        Task AddUserWithRoleAsync(UserModelView userModel);
        Task<UserProfileResponseModelView> GetUserProfile();

        Task UpdateUserByAdmin(string userID, UserUpdateByAdminModel model);
        Task<BasePaginatedList<UserResponseDTO>> GetAsync(
            int? page,
            int? pageSize,
            string? name,
            string? phone,
            string? email,
            SortBy sortBy,
            SortOrder sortOrder,
            string? role,
            string? id
            );

    }
}
