using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.ModelViews.AlumniCompanyModelViews;
using DoAnChuyenNganh.ModelViews.ResponseDTO;


namespace DoAnChuyenNganh.Contract.Services.Interface
{
    public interface IAlumniCompanyService
    {
        Task<BasePaginatedList<AlumniCompanyResponseDTO>> GetAlumniCompany(string id, string? alumniId, string? CompanyId, int pageIndex, int pageSize);
        Task CreateAlumniCompany(AlumniCompanyModelView alumniCompanyModelView);
        Task UpdateAlumniCompany(string id, string alumniId, string CompanyId, AlumniCompanyModelView alumniCompanyModelView);
    }
}
