using AutoMapper;
using DoAnChuyenNganh.Contract.Repositories.Entity;
using DoAnChuyenNganh.Contract.Services.Interface;
using DoAnChuyenNganh.ModelViews.RoleViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DoAnChuyenNganh.Services.Service
{
    public class RoleService(RoleManager<ApplicationRole> roleManager, IMapper mapper) : IRoleService
    {
        private readonly RoleManager<ApplicationRole> roleManager = roleManager;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<RoleViewModel>> GetRoles()
        {
            List<ApplicationRole>? roles = await roleManager.Roles.ToListAsync();
            return _mapper.Map<IEnumerable<RoleViewModel>>(roles);
        }

    }
}