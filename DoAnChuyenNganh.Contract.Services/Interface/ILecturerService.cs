using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.ModelViews.LecturerModelViews;
using DoAnChuyenNganh.ModelViews.ResponseDTO;

namespace DoAnChuyenNganh.Contract.Services.Interface
{
    public interface ILecturerService 
    {
        Task<BasePaginatedList<LecturerResponseDTO>> GetLecturers(string? id, string? name, int pageIndex, int pageSize);
        Task CreateLecturer(LecturerModelView lecturerModelView);
        Task UpdateLecturer(string id, LecturerModelView lecturerModelView);
        Task DeleteLecturer(string id);
    }
}
