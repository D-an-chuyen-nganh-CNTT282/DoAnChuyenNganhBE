﻿using DoAnChuyenNganh.ModelViews.RoleViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnChuyenNganh.Contract.Services.Interface
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleViewModel>> GetRoles();
    }
}
