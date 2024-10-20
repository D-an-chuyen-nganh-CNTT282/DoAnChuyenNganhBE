using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using DoAnChuyenNganh.ModelViews.StudentExpectationsModelViews;

namespace DoAnChuyenNganh.Contract.Services.Interface
{
    public interface IStudentExpectationsService
    {
        Task<BasePaginatedList<StudentExpectationsResponseDTO>> GetStudentExpectations(string? id, string? studentId, string? requestCategory, int pageIndex, int pageSize);
        Task CreateStudentExpectations(StudentExpectationsModelView studentExpectationsModelView);
        Task UpdateStudentExpectations(string id, StudentExpectationsModelView studentExpectationsModelView);
    }
}
