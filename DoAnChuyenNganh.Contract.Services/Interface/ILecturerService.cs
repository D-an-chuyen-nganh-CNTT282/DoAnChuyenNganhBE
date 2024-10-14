using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.ModelViews.LecturerModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnChuyenNganh.Contract.Services.Interface
{
    public interface ILecturerService 
    {
        Task<BasePaginatedList<LecturerModelView>> GetLecturers(string? id, string? name, int index, int pageSize);
        Task CreateLecturer(LecturerModelView lecturerModelView);
        Task UpdateLecturer(string id, LecturerModelView lecturerModelView);
        Task DeleteLecturer(string id);
    }
}
