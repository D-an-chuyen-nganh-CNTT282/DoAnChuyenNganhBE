using DoAnChuyenNganh.Contract.Repositories.Entity;
using DoAnChuyenNganh.Repositories.Context;
using Google;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnChuyenNganh.Services.Configs
{
    public class DepartmentManager
    {
        private readonly DatabaseContext _context;

        // Constructor nhận ApplicationDbContext để làm việc với cơ sở dữ liệu
        public DepartmentManager(DatabaseContext context)
        {
            _context = context;
        }
        public async Task<Department?> FindByNameAsync(string name)
        {
            return await _context.Department.FirstOrDefaultAsync(d => d.DepartmentName == name);
        }

        public async Task CreateIfNotExistsAsync(string name)
        {
            Department? existingDepartment = await FindByNameAsync(name);
            if (existingDepartment is null)
            {
                Department newDepartment = new Department
                {
                    DepartmentName = name,
                };
                _context.Department.Add(newDepartment);
                await _context.SaveChangesAsync();
            }
        }
    }
}
