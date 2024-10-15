using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.ModelViews.LecturerPlanModelViews;
using DoAnChuyenNganh.ModelViews.ResponseDTO;

namespace DoAnChuyenNganh.Contract.Services.Interface
{
    public interface ILecturerPlanService
    {
        Task<BasePaginatedList<LecturerPlanResponseDTO>> GetLecturerPlans(string? id, int pageIndex, int pageSize);
        Task CreateLecturerPlan(LecturerPlanModelView lecturerPlanModelView);
        Task UpdateLecturerPlan(string id, LecturerPlanModelView lecturerPlanModelView);
        Task DeleteLecturerPlan(string id);
    }
}
