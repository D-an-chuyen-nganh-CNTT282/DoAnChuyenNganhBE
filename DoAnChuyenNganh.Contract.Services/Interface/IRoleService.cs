using DoAnChuyenNganh.ModelViews.RoleViewModel;

namespace DoAnChuyenNganh.Contract.Services.Interface
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleViewModel>> GetRoles();
    }
}
