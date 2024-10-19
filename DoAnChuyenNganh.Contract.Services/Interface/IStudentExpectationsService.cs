using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using DoAnChuyenNganh.ModelViews.StudentExpectationsModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnChuyenNganh.Contract.Services.Interface
{
    public interface IStudentExpectationsService
    {
        Task<BasePaginatedList<StudentExpectationsResponseDTO>> GetStudentExpectations(string? id, string? studentId, string? requestCategory, int pageIndex, int pageSize);
        Task CreateStudentExpectations(StudentExpectationsModelView studentExpectationsModelView);
        Task UpdateStudentExpectations(string id, StudentExpectationsModelView studentExpectationsModelView);
    }
}
