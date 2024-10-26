using AutoMapper;
using DoAnChuyenNganh.Contract.Repositories.Entity;
using DoAnChuyenNganh.Contract.Repositories.Interface;
using DoAnChuyenNganh.Contract.Services.Interface;
using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.Core.Store;
using DoAnChuyenNganh.Core.Utils;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using DoAnChuyenNganh.ModelViews.OutgoingDocumentModelViews;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using DoAnChuyenNganh.Repositories.Entity;
using Microsoft.AspNetCore.Identity;

namespace DoAnChuyenNganh.Services.Service
{
    public class OutgoingDocumentService : IOutgoingDocumentService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly UserManager<ApplicationUser> _userManager;
        public OutgoingDocumentService(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService, UserManager<ApplicationUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _emailService = emailService;
            _userManager = userManager;
        }

        public async Task CreateOutgoingDocument(OutgoingDocumentModelView outgoingDocumentModelView)
        {
            Department? department = await _unitOfWork.GetRepository<Department>()
                .Entities
                .FirstOrDefaultAsync(d => d.Id == outgoingDocumentModelView.DepartmentId);

            if (department == null)
            {
                throw new KeyNotFoundException($"Phòng ban với mã {outgoingDocumentModelView.DepartmentId} không tìm thấy.");
            }

            string? userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }

            OutgoingDocument outgoingDocument = _mapper.Map<OutgoingDocument>(outgoingDocumentModelView);

            outgoingDocument.UserId = Guid.Parse(userId);
            outgoingDocument.CreatedBy = userId;
            outgoingDocument.CreatedTime = CoreHelper.SystemTimeNow;

            await _unitOfWork.GetRepository<OutgoingDocument>().InsertAsync(outgoingDocument);
            await _unitOfWork.SaveAsync();

            ApplicationUser? user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                throw new KeyNotFoundException($"Người dùng với mã {userId} không tìm thấy.");
            }
            string toEmail = outgoingDocument.RecipientEmail;
            string subject = $"{outgoingDocument.OutgoingDocumentTitle}";
            string logoUrl = "https://drive.google.com/uc?export=view&id=1i49oPfikilcn0r01zkJGcSJuBg-gJHbY";
            string body = $@"
                <p>Kính gửi đại diện {department.DepartmentName},</p>
                <p>Tôi là {user.Name}, đại diện cho văn phòng Khoa Công nghệ thông tin. Xin được gửi đến văn phòng {department.DepartmentName} một văn bản về {outgoingDocument.OutgoingDocumentContent}. Vui lòng xem chi tiết công văn theo link đính kèm bên dưới.</p>
                <p>Link đính kèm: {outgoingDocument.FileScanUrl}.
                <p>Vui lòng phản hồi lại với chúng tôi trước ngày <strong>{outgoingDocument.DueDate:dd/MM/yyyy}</strong> trong giờ làm việc.</p>
                <p>Trân trọng,</p>
                <p>Văn phòng Khoa Công nghệ thông tin - HUIT.</p>
                <p><i>Email này được gửi tự động thông qua hệ thống quản lý học vụ của khoa. Mọi thông tin phản hồi vui lòng gửi qua email người đại diện bên dưới.</i></p>
                <br>
                -------------------------
                <br>
                <table style='width:100%; margin-top:20px;'>
                    <tr>
                        <td style='width:20%; vertical-align:top;'>
                            <img src='{logoUrl}' alt='System Logo' width='150' height='150' style='display:block;'/>
                        </td>
                        <td style='width:80%; vertical-align:top; padding-left:10px;'>
                            <p><strong>Thông tin liên hệ:</strong></p>
                            <p>Đại diện: {user.Name}</p>
                            <p>Email: {user.Email}</p>
                            <p>Điện thoại: {user.PhoneNumber}</p>
                        </td>
                    </tr>
                </table>";
            await _emailService.SendEmailAsync(toEmail, subject, body);
        }

        public async Task<BasePaginatedList<OutgoingDocumentResponseDTO>> GetOutgoingDocuments(string? title, string? departmentId, Guid? userId, int pageIndex, int pageSize)
        {
            IQueryable<OutgoingDocument>? query = _unitOfWork.GetRepository<OutgoingDocument>().Entities.Where(doc => doc.DeletedTime == null);

            if (!string.IsNullOrWhiteSpace(title))
            {
                query = query.Where(doc => doc.OutgoingDocumentTitle.Contains(title));
            }
            if (!string.IsNullOrWhiteSpace(departmentId))
            {
                query = query.Where(doc => doc.DepartmentId == departmentId);
            }
            if (userId.HasValue)
            {
                query = query.Where(doc => doc.UserId == userId);
            }

            int totalItems = await query.CountAsync();

            List<OutgoingDocumentResponseDTO>? outgoingDocuments = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(doc => new OutgoingDocumentResponseDTO
                {
                    Id = doc.Id,
                    OutgoingDocumentTitle = doc.OutgoingDocumentTitle,
                    OutgoingDocumentContent = doc.OutgoingDocumentContent,
                    DepartmentId = doc.DepartmentId,
                    RecipientEmail = doc.RecipientEmail,
                    UserId = doc.UserId,
                    OutgoingDocumentProcessingStatuss = doc.OutgoingDocumentProcessingStatuss.ToString(),
                    CreatedBy = doc.CreatedBy,
                    CreatedTime = doc.CreatedTime,
                })
                .ToListAsync();

            return new BasePaginatedList<OutgoingDocumentResponseDTO>(outgoingDocuments, totalItems, pageIndex, pageSize);
        }
    }
}
