using DoAnChuyenNganh.Contract.Repositories.Entity;
using DoAnChuyenNganh.Repositories.Context;
using Microsoft.EntityFrameworkCore;

namespace DoAnChuyenNganh.Services.Configs
{
    public class DepartmentManager
    {
        private readonly DatabaseContext _context;
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
