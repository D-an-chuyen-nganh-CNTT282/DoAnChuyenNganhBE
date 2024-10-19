using AutoMapper;
using DoAnChuyenNganh.Contract.Repositories.Entity;
using DoAnChuyenNganh.Contract.Repositories.Interface;
using DoAnChuyenNganh.Contract.Services.Interface;
using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.Core.Store;
using DoAnChuyenNganh.Core.Utils;
using DoAnChuyenNganh.ModelViews.StudentExpectationsModelViews;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DoAnChuyenNganh.Services.Service
{
    public class StudentExpectationsService : IStudentExpectationsService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StudentExpectationsService(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateStudentExpectations(StudentExpectationsModelView studentExpectationsModelView)
        {
            string? UserId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            StudentExpectations studentExpectations = _mapper.Map<StudentExpectations>(studentExpectationsModelView);
            studentExpectations.CreatedBy = UserId;
            studentExpectations.CreatedTime = CoreHelper.SystemTimeNow;
            await _unitOfWork.GetRepository<StudentExpectations>().InsertAsync(studentExpectations);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateStudentExpectations(string id, StudentExpectationsModelView studentExpectationsModelView)
        {
            string? UserId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã yêu cầu của sinh viên!");
            }

            StudentExpectations? existingExpectation = await _unitOfWork.GetRepository<StudentExpectations>()
                .Entities.FirstOrDefaultAsync(a => a.Id == id)
                ?? throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, $"Không tìm thấy yêu cầu nào với mã {id}");

            existingExpectation.RequestCategory = studentExpectationsModelView.RequestCategory;
            existingExpectation.ProcessingStatuss = (StudentExpectations.ProcessingStatus)Enum.Parse(typeof(StudentExpectations.ProcessingStatus), studentExpectationsModelView.ProcessingStatus);
            existingExpectation.LastUpdatedBy = UserId;
            existingExpectation.LastUpdatedTime = CoreHelper.SystemTimeNow;

            await _unitOfWork.GetRepository<StudentExpectations>().UpdateAsync(existingExpectation);
            await _unitOfWork.SaveAsync();
        }

        public async Task<BasePaginatedList<StudentExpectationsResponseDTO>> GetStudentExpectations(string? id, string? studentId, string? requestCategory, int pageIndex, int pageSize)
        {
            IQueryable<StudentExpectations>? query = _unitOfWork.GetRepository<StudentExpectations>().Entities.Where(b => b.DeletedTime == null);

            if (!string.IsNullOrWhiteSpace(id))
            {
                query = query.Where(a => a.Id == id);
            }
            if (!string.IsNullOrWhiteSpace(studentId))
            {
                query = query.Where(a => a.StudentId == studentId);
            }
            if (!string.IsNullOrEmpty(requestCategory))
            {
                query = query.Where(a => a.RequestCategory == requestCategory);
            }

            int totalItems = await query.CountAsync();
            List<StudentExpectationsResponseDTO>? studentExpectations = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(expectation => new StudentExpectationsResponseDTO
                {
                    Id = expectation.Id,
                    StudentId = expectation.StudentId,
                    RequestCategory = expectation.RequestCategory,
                    ProcessingStatus = expectation.ProcessingStatuss.ToString(),
                    CreatedBy = expectation.CreatedBy,
                    LastUpdatedBy = expectation.LastUpdatedBy,
                    CreatedTime = expectation.CreatedTime,
                    LastUpdatedTime = expectation.LastUpdatedTime,
                })
                .ToListAsync();

            return new BasePaginatedList<StudentExpectationsResponseDTO>(studentExpectations, totalItems, pageIndex, pageSize);
        }
    }
}
