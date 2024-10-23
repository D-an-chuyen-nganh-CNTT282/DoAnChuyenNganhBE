using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using DoAnChuyenNganh.ModelViews.StudentModelViews;

namespace DoAnChuyenNganh.Contract.Services.Interface
{
    public interface IStudentService
    {
        Task<BasePaginatedList<StudentResponseDTO>> GetStudents(string? id, string? name, string? studentClass, string? studentMajor, int pageIndex, int pageSize);
        Task CreateStudent(StudentModelView studentModelView);
        Task UpdateStudent(string id, StudentModelView studentModelView);
        Task DeleteStudent(string id);
    }
}
