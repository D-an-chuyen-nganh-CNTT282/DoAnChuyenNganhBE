using AutoMapper;
using DoAnChuyenNganh.Contract.Repositories.Entity;
using DoAnChuyenNganh.Contract.Repositories.Interface;
using DoAnChuyenNganh.Contract.Services.Interface;
using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.Core.Utils;
using DoAnChuyenNganh.ModelViews.AlumniModelViews;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using DoAnChuyenNganh.Core.Store;


namespace DoAnChuyenNganh.Services.Service
{
    public class AlumniService : IAlumniService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AlumniService(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Create(AlumniModelView alumniModelView)
        {
            string? UserId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            Alumni newAlumni = _mapper.Map<Alumni>(alumniModelView);
            newAlumni.CreatedTime = CoreHelper.SystemTimeNow;
            newAlumni.DeletedTime = null;
            newAlumni.CreatedBy = UserId;
            newAlumni.UserId = Guid.Parse(UserId);
            await _unitOfWork.GetRepository<Alumni>().InsertAsync(newAlumni);
            await _unitOfWork.SaveAsync();
        }


        private async Task<BasePaginatedList<AlumniResponseDTO>> PaginateAlumni(IQueryable<Alumni> query, int? pageIndex, int? pageSize)
        {
            int currentPage = pageIndex ?? 1;
            int currentPageSize = pageSize ?? 10;
            int totalItems = await query.CountAsync();

            List<AlumniResponseDTO>? alumnis = await query
                .Skip((currentPage - 1) * currentPageSize)
                .Take(currentPageSize)
                .Select(alumni => new AlumniResponseDTO
                {
                    Id = alumni.Id,
                    AlumniName = alumni.AlumniName,
                    DayOfBirth = alumni.DayOfBirth,
                    AlumniMajor = alumni.AlumniMajor,
                    AlumniGender = alumni.AlumniGender,
                    AlumniAddress = alumni.AlumniAddress,
                    AlumniCourse = alumni.AlumniCourse,
                    AlumniEmail = alumni.AlumniEmail,
                    AlumniPhone = alumni.AlumniPhone,
                    GraduationDay = alumni.GraduationDay,
                    LecturerId = alumni.LecturerId,
                    UserId = alumni.UserId,
                    PreviousClass = alumni.PreviousClass,
                    CreatedBy = alumni.CreatedBy,
                    LastUpdatedBy = alumni.LastUpdatedBy,
                    CreatedTime = alumni.CreatedTime,
                    LastUpdatedTime = alumni.LastUpdatedTime,
                })
                .ToListAsync();

            return new BasePaginatedList<AlumniResponseDTO>(alumnis, totalItems, currentPage, currentPageSize);
        }
        public async Task<BasePaginatedList<AlumniResponseDTO>> Get(string? id, string? alumniName, string? alumniMajor, string? alumniCourse, int pageSize, int pageIndex)
        {

            IQueryable<Alumni>? query = _unitOfWork.GetRepository<Alumni>().Entities.Where(l => l.DeletedTime == null);

            if (!string.IsNullOrWhiteSpace(id))
            {
                query = query.Where(a => a.Id == id);
            }

            if ( !string.IsNullOrEmpty(alumniName))
            {
                query = query.Where(a => a.AlumniName.Contains(alumniName));
            }

            if (!string.IsNullOrEmpty(alumniMajor))
            {
                query = query.Where(a => a.AlumniMajor.Contains(alumniMajor));
            }

            if (!string.IsNullOrEmpty(alumniCourse))
            {
                query = query.Where(a => a.AlumniCourse.Contains(alumniCourse));
            }
            return await PaginateAlumni(query, pageIndex, pageSize);
        }


        public async Task Update(string id, AlumniModelView alumniView)
        {
            string? UserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã cựa sinh viên!");
            }
            Alumni? alumni = await _unitOfWork.GetRepository<Alumni>().GetByIdAsync(id)
                ?? throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, $"Không tìm thấy cựu sinh viên nào với mã {id}!");
            _mapper.Map(alumniView, alumni);
            alumni.LastUpdatedTime = CoreHelper.SystemTimeNow;
            alumni.LastUpdatedBy = UserId;
            alumni.UserId = Guid.Parse(UserId);
            await _unitOfWork.GetRepository<Alumni>().UpdateAsync(alumni);
            await _unitOfWork.SaveAsync();
        }

        
    }
}
