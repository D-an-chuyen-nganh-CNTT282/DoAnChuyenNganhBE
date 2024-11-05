using AutoMapper;
using DoAnChuyenNganh.Contract.Repositories.Entity;
using DoAnChuyenNganh.Contract.Repositories.Interface;
using DoAnChuyenNganh.Contract.Services.Interface;
using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.Core.Store;
using DoAnChuyenNganh.Core.Utils;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using DoAnChuyenNganh.ModelViews.StudentExpectationsModelViews;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DoAnChuyenNganh.Services.Service
{
    public class StudentExpectationsService : IStudentExpectationsService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinaryService;

        public StudentExpectationsService(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, IMapper mapper, ICloudinaryService cloudinaryService)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
        }

        public async Task CreateStudentExpectations(StudentExpectationsModelView studentExpectationsModelView)
        {
            string? userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            string fileUrl = null;
            if (studentExpectationsModelView.FileScanUrl != null)
            {
                // Giả sử bạn đã có ICloudinaryService
                fileUrl = await _cloudinaryService.UploadFileAsync(studentExpectationsModelView.FileScanUrl);
            }
            StudentExpectations studentExpectations = _mapper.Map<StudentExpectations>(studentExpectationsModelView);

            studentExpectations.FileScanUrl = fileUrl;
            studentExpectations.UserId = Guid.Parse(userId);
            studentExpectations.CreatedBy = userId;
            studentExpectations.CreatedTime = CoreHelper.SystemTimeNow;
            studentExpectations.LastUpdatedBy = userId;
            studentExpectations.LastUpdatedTime = CoreHelper.SystemTimeNow;

            await _unitOfWork.GetRepository<StudentExpectations>().InsertAsync(studentExpectations);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateStudentExpectations(string id, StudentExpectationsModelView studentExpectationsModelView)
        {
            string? userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }

            if (string.IsNullOrWhiteSpace(id))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã yêu cầu của sinh viên!");
            }

            StudentExpectations? studentExpectations = await _unitOfWork.GetRepository<StudentExpectations>().GetByIdAsync(id)
                ?? throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, $"Không tìm thấy yêu cầu nào với mã {id}");
            string oldFileUrl = studentExpectations.FileScanUrl;

            _mapper.Map(studentExpectationsModelView, studentExpectations);

            if (studentExpectationsModelView.FileScanUrl != null)
            {
                // Ghi đè file cũ bằng cách sử dụng publicId
                var publicId = !string.IsNullOrEmpty(oldFileUrl)
                    ? Path.GetFileNameWithoutExtension(new Uri(oldFileUrl).AbsolutePath)
                    : null;

                studentExpectations.FileScanUrl = await _cloudinaryService.UploadFileAsync(studentExpectationsModelView.FileScanUrl);
            }
            studentExpectations.UserId = Guid.Parse(userId);
            studentExpectations.LastUpdatedBy = userId;
            studentExpectations.LastUpdatedTime = CoreHelper.SystemTimeNow;

            await _unitOfWork.GetRepository<StudentExpectations>().UpdateAsync(studentExpectations);
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
            if (!string.IsNullOrWhiteSpace(requestCategory))
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
                    UserId = expectation.UserId,
                    ProcessingStatus = expectation.ProcessingStatuss.ToString(),
                    RequestDate = expectation.RequestDate,
                    CompletionDate = expectation.CompletionDate,
                    FileScanUrl = expectation.FileScanUrl,
                    //CreatedBy = expectation.CreatedBy,
                    //LastUpdatedBy = expectation.LastUpdatedBy,
                    //CreatedTime = expectation.CreatedTime,
                    //LastUpdatedTime = expectation.LastUpdatedTime
                })
                .ToListAsync();

            return new BasePaginatedList<StudentExpectationsResponseDTO>(studentExpectations, totalItems, pageIndex, pageSize);
        }
    }
}
