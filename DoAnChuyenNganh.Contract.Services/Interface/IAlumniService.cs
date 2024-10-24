using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.ModelViews.AlumniModelViews;
using DoAnChuyenNganh.ModelViews.ResponseDTO;

namespace DoAnChuyenNganh.Contract.Services.Interface
{
    public interface IAlumniService
    {
        Task<BasePaginatedList<AlumniResponseDTO>> Get(string? id, string? alumniName, string? alumniMajor, string? alumniCourse, int pageSize, int pageIndex);
        Task Create(AlumniModelView alumniView);
        Task Update(string id, AlumniModelView alumniView);
    }
}
