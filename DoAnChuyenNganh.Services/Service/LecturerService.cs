using AutoMapper;
using DoAnChuyenNganh.Contract.Repositories.Entity;
using DoAnChuyenNganh.Contract.Repositories.Interface;
using DoAnChuyenNganh.Contract.Services.Interface;
using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.Core.Store;
using DoAnChuyenNganh.Core.Utils;
using DoAnChuyenNganh.ModelViews.LecturerModelViews;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DoAnChuyenNganh.Services.Service
{
    public class LecturerService : ILecturerService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public LecturerService(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task CreateLecturer(LecturerModelView lecturerModelView)
        {
            string? UserId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            Lecturer newLecturer = _mapper.Map<Lecturer>(lecturerModelView);
            newLecturer.CreatedTime = CoreHelper.SystemTimeNow;
            newLecturer.DeletedTime = null;
            newLecturer.CreatedBy = UserId;
            await _unitOfWork.GetRepository<Lecturer>().InsertAsync(newLecturer);
            await _unitOfWork.SaveAsync();
        }
        public async Task DeleteLecturer(string id)
        {
            string? UserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã giảng viên!");
            }
            Lecturer? lecturer = await _unitOfWork.GetRepository<Lecturer>().GetByIdAsync(id)
                ?? throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, $"Không tìm thấy giảng viên nào với mã {id}!");
            if (lecturer.DeletedTime != null)
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, "Thông tin giảng viên đã bị xóa!");
            }
            lecturer.DeletedBy = UserId;
            lecturer.DeletedTime = CoreHelper.SystemTimeNow;
            await _unitOfWork.GetRepository<Lecturer>().UpdateAsync(lecturer);
            await _unitOfWork.SaveAsync();
        }
        public async Task UpdateLecturer(string id, LecturerModelView lecturerModelView)
        {
            string? UserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã giảng viên!");
            }
            Lecturer? lecturer = await _unitOfWork.GetRepository<Lecturer>().GetByIdAsync(id)
                ?? throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, $"Không tìm thấy giảng viên nào với mã {id}!");
            _mapper.Map(lecturerModelView, lecturer);
            lecturer.LastUpdatedTime = CoreHelper.SystemTimeNow;
            lecturer.LastUpdatedBy = UserId;
            await _unitOfWork.GetRepository<Lecturer>().UpdateAsync(lecturer);
            await _unitOfWork.SaveAsync();
        }
        public async Task<BasePaginatedList<LecturerModelView>> GetLecturers(string? id, string? name, int pageIndex, int pageSize)
        {
            IQueryable<Lecturer>? query = _unitOfWork.GetRepository<Lecturer>().Entities.Where(lecturer => lecturer.DeletedTime != null);
            if (!string.IsNullOrWhiteSpace(id))
            {
                query = query.Where(lecturer => lecturer.Id == id);
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(lecturer => lecturer.LecturerName == name);
            }
            BasePaginatedList<Lecturer>? paginatedLecturers = await _unitOfWork.GetRepository<Lecturer>().GetPagging(query, pageIndex, pageSize);
            if (!paginatedLecturers.Items.Any())
            {
                if (!string.IsNullOrWhiteSpace(id))
                {
                    Lecturer? lecturerById = await _unitOfWork.GetRepository<Lecturer>().Entities
                        .FirstOrDefaultAsync(lecturer => lecturer.Id == id && lecturer.DeletedTime == null);
                    if (lecturerById != null)
                    {
                        LecturerModelView? lecturerModel = _mapper.Map<LecturerModelView>(lecturerById);
                        return new BasePaginatedList<LecturerModelView>(new List<LecturerModelView> { lecturerModel }, 1, 1, 1);
                    }
                }

                if (!string.IsNullOrWhiteSpace(name))
                {
                    List<Lecturer>? lecturerByName = await _unitOfWork.GetRepository<Lecturer>().Entities
                        .Where(lecturer => lecturer.LecturerName == name && lecturer.DeletedTime == null)
                        .ToListAsync();
                    if (lecturerByName.Any())
                    {
                        List<LecturerModelView>? lecturerModels = _mapper.Map<List<LecturerModelView>>(lecturerByName);
                        return new BasePaginatedList<LecturerModelView>(lecturerModels, 1, 1, lecturerByName.Count());
                    }
                }
            }
            //GetAll
            List<LecturerModelView>? lecturerModelResult = _mapper.Map<List<LecturerModelView>>(paginatedLecturers.Items);
            return new BasePaginatedList<LecturerModelView> (
                lecturerModelResult,
                paginatedLecturers.TotalItems,
                paginatedLecturers.CurrentPage,
                paginatedLecturers.PageSize
            );
        }
    }
}
