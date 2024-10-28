using DoAnChuyenNganh.Contract.Repositories.Entity;
using DoAnChuyenNganh.Contract.Repositories.Interface;
using DoAnChuyenNganh.Contract.Services.Interface;
using DoAnChuyenNganh.Core.Utils;
using DoAnChuyenNganh.Repositories.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DoAnChuyenNganh.Services.EmailSettings
{
    public class EmailReminderService : BackgroundService
    {
        public readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<EmailReminderService> _logger;
        public EmailReminderService(IServiceScopeFactory serviceScopeFactory, ILogger<EmailReminderService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await UpdateDeletedTimeForDocuments();
                await SendOutgoingDocumentDueDateReminders();
                await SendIncomingDocumentDueDateReminders();
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken); // Thực thi mỗi ngày một lần
            }
        }

        private async Task SendOutgoingDocumentDueDateReminders()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

            List<OutgoingDocument> documents = await unitOfWork.GetRepository<OutgoingDocument>()
                .Entities
                .Where(doc => doc.OutgoingDocumentProcessingStatuss == OutgoingDocument.OutgoingDocumentProcessingStatus.PendingResponse
                    && doc.DueDate.Date == DateTime.Now.Date.AddDays(2))
                .ToListAsync();
            
            foreach (var doc in documents)
            {
                Department? department = await unitOfWork.GetRepository<Department>()
                    .Entities
                    .FirstOrDefaultAsync(d => d.Id == doc.DepartmentId);
                string subject = "Nhắc nhở phản hồi công văn";
                string logoUrl = "https://drive.google.com/uc?export=view&id=1i49oPfikilcn0r01zkJGcSJuBg-gJHbY";
                string body = $@"
                <p>Kính gửi đại diện {department.DepartmentName},</p>
                <p>Văn bản '{doc.OutgoingDocumentTitle}' yêu cầu phản hồi trước ngày <strong style='color:red;'>{doc.DueDate:dd/MM/yyyy}</strong>.</p>
                <p>Vui lòng kiểm tra và phản hồi sớm nhất có thể.</p>
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
                            <p><span style='color:blue;'>Đại diện:</span> Quản trị viên</p>
                            <p><span style='color:blue;'>Email:</span> admin@gmail.com</p>
                            <p><span style='color:blue;'>Điện thoại:</span> 0928838171</p>
                        </td>
                    </tr>
                </table>";

                try
                {
                    await emailService.SendEmailAsync(doc.RecipientEmail, subject, body);
                    _logger.LogInformation($"Đã gửi email nhắc nhở đến: {doc.RecipientEmail} cho văn bản: {doc.OutgoingDocumentTitle}");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Lỗi khi gửi email nhắc nhở cho {doc.OutgoingDocumentTitle}: {ex.Message}");
                }
            }
        }
        private async Task SendIncomingDocumentDueDateReminders()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();


            List<IncomingDocument> documents = await unitOfWork.GetRepository<IncomingDocument>()
                .Entities
                .Where(doc => doc.IncomingDocumentProcessingStatuss == IncomingDocument.IncomingDocumentProcessingStatus.Received
                    && doc.DueDate.Date == DateTime.Now.Date.AddDays(2))
                .ToListAsync();

            foreach (var doc in documents)
            {
                Guid userId = doc.UserId;
                ApplicationUser? user = await userManager.Users
                    .FirstOrDefaultAsync(u => u.Id == doc.UserId);

                Department? department = await unitOfWork.GetRepository<Department>()
                    .Entities
                    .FirstOrDefaultAsync(d => d.Id == doc.DepartmentId);
                string subject = "Nhắc nhở phản hồi công văn";
                string logoUrl = "https://drive.google.com/uc?export=view&id=1i49oPfikilcn0r01zkJGcSJuBg-gJHbY";
                string body = $@"
                <p>Kính gửi giảng viên {user.Name},</p>
                <p>Văn bản '{doc.IncomingDocumentTitle}' yêu cầu phản hồi trước ngày <strong style='color:red;'>{doc.DueDate:dd/MM/yyyy}</strong>.</p>
                <p>Vui lòng kiểm tra và phản hồi với {department.DepartmentName} sớm nhất có thể.</p>
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
                            <p><span style='color:blue;'>Đại diện:</span> Quản trị viên</p>
                            <p><span style='color:blue;'>Email:</span> admin@gmail.com</p>
                            <p><span style='color:blue;'>Điện thoại:</span> 0928838171</p>
                        </td>
                    </tr>
                </table>";

                try
                {
                    await emailService.SendEmailAsync(user.Email, subject, body);
                    _logger.LogInformation($"Đã gửi email nhắc nhở đến: {user.Email} cho văn bản: {doc.IncomingDocumentTitle}");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Lỗi khi gửi email nhắc nhở cho {doc.IncomingDocumentTitle}: {ex.Message}");
                }
            }
        }
        private async Task UpdateDeletedTimeForDocuments()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var incomingDocs = await unitOfWork.GetRepository<IncomingDocument>()
                .Entities
                .Where(doc => doc.DueDate.Date < DateTime.Now.Date && doc.DeletedTime == null)
                .ToListAsync();

            foreach (var doc in incomingDocs)
            {
                doc.LastUpdatedTime = CoreHelper.SystemTimeNow; 
                doc.IncomingDocumentProcessingStatuss = IncomingDocument.IncomingDocumentProcessingStatus.Overdue; 
                unitOfWork.GetRepository<IncomingDocument>().Update(doc); 
                _logger.LogInformation($"Cập nhật DeletedTime và trạng thái cho Incoming document: {doc.IncomingDocumentTitle}");
            }

            
            var outgoingDocs = await unitOfWork.GetRepository<OutgoingDocument>()
                .Entities
                .Where(doc => doc.DueDate.Date < DateTime.Now.Date && doc.DeletedTime == null)
                .ToListAsync();

            foreach (var doc in outgoingDocs)
            {
                doc.LastUpdatedTime = CoreHelper.SystemTimeNow; 
                doc.OutgoingDocumentProcessingStatuss = OutgoingDocument.OutgoingDocumentProcessingStatus.Overdue; 
                unitOfWork.GetRepository<OutgoingDocument>().Update(doc); 
                _logger.LogInformation($"Cập nhật DeletedTime và trạng thái cho Outgoing document: {doc.OutgoingDocumentTitle}");
            }

            await unitOfWork.SaveAsync(); 
        }

    }
}
