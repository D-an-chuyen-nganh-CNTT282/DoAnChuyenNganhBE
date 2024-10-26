using AutoMapper;
using DoAnChuyenNganh.Contract.Repositories.Entity;
using DoAnChuyenNganh.Contract.Repositories.Interface;
using DoAnChuyenNganh.Contract.Services.Interface;
using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.Core.Store;
using DoAnChuyenNganh.Core.Utils;
using DoAnChuyenNganh.ModelViews.IncomingDocumentModelViews;
using DoAnChuyenNganh.ModelViews.ResponseDTO;
using DoAnChuyenNganh.Repositories.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
namespace DoAnChuyenNganh.Services.Service
{
    public class IncomingDocumentService : IIncomingDocumentService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly UserManager<ApplicationUser> _userManager;
        public IncomingDocumentService(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService, UserManager<ApplicationUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _emailService = emailService;
            _userManager = userManager;
        }
        public async Task Create(IncomingDocumentModelViews incomingdocumentView)
        {
            Department? department = await _unitOfWork.GetRepository<Department>()
                .Entities
                .FirstOrDefaultAsync(d => d.Id == incomingdocumentView.DepartmentId);

            if (department == null)
            {
                throw new KeyNotFoundException($"Phòng ban với mã {incomingdocumentView.DepartmentId} không tìm thấy.");
            }
            string? UserId =  _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            IncomingDocument newincomingdocument = _mapper.Map<IncomingDocument>(incomingdocumentView);

            newincomingdocument.UserId = Guid.Parse(UserId);
            newincomingdocument.CreatedTime = CoreHelper.SystemTimeNow;
            newincomingdocument.CreatedBy = UserId;

            await _unitOfWork.GetRepository<IncomingDocument>().InsertAsync(newincomingdocument);
            await _unitOfWork.SaveAsync();

            ApplicationUser? user = await _userManager.FindByIdAsync(UserId);
            if (user is null)
            {
                throw new KeyNotFoundException($"Người dùng với mã {UserId} không tìm thấy.");
            }
            string toEmail = user.Email;
            string subject = $"{incomingdocumentView.IncomingDocumentTitle}";
            string logoUrl = "https://drive.google.com/uc?export=view&id=1i49oPfikilcn0r01zkJGcSJuBg-gJHbY";
            string body = $@"
                <p>Kính gửi giảng viên {user.Name},</p>
                <p>Bạn vừa nhận được một công văn mới từ {department.DepartmentName} với nội dung về {incomingdocumentView.IncomingDocumentContent}. Vui lòng xem chi tiết công văn theo link đính kèm bên dưới.</p>
                <p>Link đính kèm: {incomingdocumentView.FileScanUrl}.
                <p>Vui lòng phản hồi công văn này với {department.DepartmentName} trước ngày <strong>{incomingdocumentView.DueDate:dd/MM/yyyy}</strong> trong giờ làm việc.</p>
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

        public async Task Delete(string id)
        {
            string? UserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã công văn đến!");
            }
            IncomingDocument? incomingDocument = await _unitOfWork.GetRepository<IncomingDocument>().GetByIdAsync(id)
                ?? throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, $"Không tìm công văn đến nào với mã {id}!");
            if (incomingDocument.DeletedTime != null)
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, "Thông tin công văn đến đã bị xóa!");
            }
            incomingDocument.DeletedBy = UserId;
            incomingDocument.DeletedTime = CoreHelper.SystemTimeNow;
            await _unitOfWork.GetRepository<IncomingDocument>().UpdateAsync(incomingDocument);
            await _unitOfWork.SaveAsync();
        }
        
        public async Task<BasePaginatedList<IncomingDocumentResponseDTO>> Get(string? id, string? Title, Guid? userid, DateTime? duedate, int pageSize, int pageIndex)
        {
            IQueryable<IncomingDocument>? query = _unitOfWork.GetRepository<IncomingDocument>().Entities.Where(doc => doc.DeletedTime == null);
            if (!string.IsNullOrWhiteSpace(id))
            {
                query = query.Where(incomingDocument => incomingDocument.Id == id);
            }
            if (!string.IsNullOrWhiteSpace(Title))
            {
                query = query.Where(incomingDocument => incomingDocument.IncomingDocumentTitle == Title);
            }
            if (userid != null)
            {
                query = query.Where(incomingDocument => incomingDocument.UserId == userid);
            }
            if (duedate != null)
            {
                query = query.Where(incomingDocument => incomingDocument.DueDate == duedate);
            }
            int totalItems = await query.CountAsync();

            List<IncomingDocumentResponseDTO>? outgoingDocuments = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(doc => new IncomingDocumentResponseDTO
                {
                    Id = doc.Id,
                    IncomingDocumentContent = doc.IncomingDocumentContent,
                    IncomingDocumentTitle = doc.IncomingDocumentTitle,
                    IncomingDocumentProcessingStatuss = doc.IncomingDocumentProcessingStatuss.ToString(),
                    UserId = doc.UserId,
                    DepartmentId = doc.DepartmentId,
                    FileScanUrl = doc.FileScanUrl,
                    ReceivedDate = doc.ReceivedDate,
                    DueDate = doc.DueDate,
                    LastUpdatedBy = doc.LastUpdatedBy,
                    LastUpdatedTime = doc.LastUpdatedTime,
                    CreatedBy = doc.CreatedBy,
                    CreatedTime = doc.CreatedTime,
                })
                .ToListAsync();

            return new BasePaginatedList<IncomingDocumentResponseDTO>(outgoingDocuments, totalItems, pageIndex, pageSize);
        }


        public async Task Update(string id, IncomingDocumentModelViews incomingdocumentView)
        {
            string? UserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrWhiteSpace(UserId))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.Unauthorized, ErrorCode.Unauthorized, "Vui lòng đăng nhập vào tài khoản!");
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Xin hãy nhập mã công văn đến!");
            }
            IncomingDocument? incomingDocument = await _unitOfWork.GetRepository<IncomingDocument>().GetByIdAsync(id)
                ?? throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, ErrorCode.NotFound, $"Không tìm thấy công văn đến nào với mã {id}!");
            _mapper.Map(incomingdocumentView, incomingDocument);
            incomingDocument.LastUpdatedTime = CoreHelper.SystemTimeNow;
            incomingDocument.LastUpdatedBy = UserId;
            incomingDocument.UserId = Guid.Parse(UserId);
            await _unitOfWork.GetRepository<IncomingDocument>().UpdateAsync(incomingDocument);
            await _unitOfWork.SaveAsync();
        }
    }
}
