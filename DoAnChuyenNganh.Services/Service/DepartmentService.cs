using DoAnChuyenNganh.Contract.Repositories.Entity;
using DoAnChuyenNganh.Contract.Repositories.Interface;
using DoAnChuyenNganh.Contract.Services.Interface;
using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using Microsoft.EntityFrameworkCore;

namespace DoAnChuyenNganh.Services.Service
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        public DepartmentService (IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        private async Task<BasePaginatedList<DepartmentResponseDTO>> PaginateDepartments(
        IQueryable<Department> query,
        int? pageIndex,
        int? pageSize)
        {
            int currentPage = pageIndex ?? 1;
            int currentPageSize = pageSize ?? 10;
            int totalItems = await query.CountAsync();

            List<DepartmentResponseDTO>? departments = await query
                .Skip((currentPage - 1) * currentPageSize)
                .Take(currentPageSize)
                .Select(department => new DepartmentResponseDTO
                {
                    Id = department.Id,
                    DepartmentName = department.DepartmentName,
                })
                .ToListAsync();

            return new BasePaginatedList<DepartmentResponseDTO>(departments, totalItems, currentPage, currentPageSize);
        }
        public async Task<BasePaginatedList<DepartmentResponseDTO>> GetDepartments(string? id, string? name, int pageIndex, int pageSize)
        {
            IQueryable<Department>? query = _unitOfWork.GetRepository<Department>().Entities.Where(dep => dep.DeletedTime == null);
            if (!string.IsNullOrWhiteSpace(id))
            {
                query = query.Where(dep => dep.Id == id);
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(dep => dep.DepartmentName == name);
            }
            return await PaginateDepartments(query, pageIndex, pageSize);
        }
    }
}
