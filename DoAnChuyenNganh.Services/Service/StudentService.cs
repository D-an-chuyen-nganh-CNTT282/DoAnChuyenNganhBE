using AutoMapper;
using DoAnChuyenNganh.Contract.Repositories.Entity;
using DoAnChuyenNganh.Contract.Repositories.Interface;
using DoAnChuyenNganh.Contract.Services.Interface;
using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.Core.Store;
using DoAnChuyenNganh.Core.Utils;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using DoAnChuyenNganh.ModelViews.StudentModelViews;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DoAnChuyenNganh.Services.Service
{
    public class StudentService : IStudentService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StudentService(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateStudent(StudentModelView studentModelView)
        {
            string? UserId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }

            Student newStudent = _mapper.Map<Student>(studentModelView);
            newStudent.CreatedTime = CoreHelper.SystemTimeNow.DateTime;
            newStudent.DeletedTime = null;
            newStudent.CreatedBy = UserId;

            await _unitOfWork.GetRepository<Student>().InsertAsync(newStudent);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteStudent(string id)
        {
            string? UserId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }

            if (string.IsNullOrWhiteSpace(id))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã số sinh viên!");
            }

            Student? student = await _unitOfWork.GetRepository<Student>().GetByIdAsync(id)
                ?? throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, $"Không tìm thấy sinh viên nào với mã {id}!");

            if (student.DeletedTime != null)
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, "Thông tin sinh viên đã bị xóa!");
            }

            student.DeletedBy = UserId;
            student.DeletedTime = CoreHelper.SystemTimeNow.DateTime;

            await _unitOfWork.GetRepository<Student>().UpdateAsync(student);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateStudent(string id, StudentModelView studentModelView)
        {
            string? UserId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }

            if (string.IsNullOrWhiteSpace(id))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã số sinh viên!");
            }

            Student? student = await _unitOfWork.GetRepository<Student>().GetByIdAsync(id)
                ?? throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, $"Không tìm thấy sinh viên nào với mã {id}!");

            _mapper.Map(studentModelView, student);
            student.LastUpdatedTime = CoreHelper.SystemTimeNow.DateTime;
            student.LastUpdatedBy = UserId;

            await _unitOfWork.GetRepository<Student>().UpdateAsync(student);
            await _unitOfWork.SaveAsync();
        }

        private async Task<BasePaginatedList<StudentResponseDTO>> PaginateStudents(
            IQueryable<Student> query,
            int? pageIndex,
            int? pageSize)
        {
            int currentPage = pageIndex ?? 1;
            int currentPageSize = pageSize ?? 10;
            int totalItems = await query.CountAsync();

            List<StudentResponseDTO>? students = await query
                .Skip((currentPage - 1) * currentPageSize)
                .Take(currentPageSize)
                .Select(student => new StudentResponseDTO
                {
                    Id = student.Id,
                    StudentName = student.StudentName,
                    DayOfBirth = student.DayOfBirth,
                    StudentGender = student.StudentGender,
                    StudentEmail = student.StudentEmail,
                    StudentPhone = student.StudentPhone,
                    StudentAddress = student.StudentAddress,
                    StudentMajor = student.StudentMajor,
                    StudentCourse = student.StudentCourse,
                    Class = student.Class,
                    GPA = student.GPA,
                    LecturerId = student.LecturerId,
                    CreatedBy = student.CreatedBy,
                    LastUpdatedBy = student.LastUpdatedBy,
                    CreatedTime = student.CreatedTime,
                    LastUpdatedTime = student.LastUpdatedTime,
                })
                .ToListAsync();

            return new BasePaginatedList<StudentResponseDTO>(students, totalItems, currentPage, currentPageSize);
        }

        public async Task<BasePaginatedList<StudentResponseDTO>> GetStudents(string? id, string? name, string? studentClass, string? studentMajor, int pageIndex, int pageSize)
        {
            IQueryable<Student>? query = _unitOfWork.GetRepository<Student>().Entities.Where(l => l.DeletedTime == null);

            if (!string.IsNullOrWhiteSpace(id))
            {
                query = query.Where(student => student.Id == id);
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(student => student.StudentName == name);
            }

            if (!string.IsNullOrWhiteSpace(studentClass))
            {
                query = query.Where(student => student.Class == studentClass);
            }

            if (!string.IsNullOrWhiteSpace(studentMajor))
            {
                query = query.Where(student => student.StudentMajor == studentMajor);
            }

            return await PaginateStudents(query, pageIndex, pageSize);
        }
    }
}