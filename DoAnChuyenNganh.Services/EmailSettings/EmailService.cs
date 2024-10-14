using DoAnChuyenNganh.Contract.Services.Interface;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;

namespace DoAnChuyenNganh.Services.EmailSettings
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Quản lý học vụ", _emailSettings.SenderEmail));
            email.To.Add(new MailboxAddress("Giảng viên", toEmail));
            email.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = body };
            email.Body = bodyBuilder.ToMessageBody();

            using var smtp = new SmtpClient();
            try
            {
                // Kết nối với máy chủ SMTP sử dụng STARTTLS
                await smtp.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.SenderPassword);
                await smtp.SendAsync(email);
            }
            finally
            {
                await smtp.DisconnectAsync(true);
                smtp.Dispose();
            }
        }

        // Gửi thông tin preorder
        public async Task SendPreorderConfirmationEmailAsync(string toEmail, string productName, int quantity)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("MilkStore", _emailSettings.SenderEmail));
            email.To.Add(new MailboxAddress("Customer", toEmail));
            email.Subject = "Preorder Confirmation";

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $"<h1>Preorder Confirmation</h1><p>Product: {productName}</p><p>Quantity: {quantity}</p><p>Thank you for your preorder!</p>"
            };
            email.Body = bodyBuilder.ToMessageBody();

            using var smtp = new SmtpClient();
            try
            {
                await smtp.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.SenderPassword);
                await smtp.SendAsync(email);
            }
            finally
            {
                await smtp.DisconnectAsync(true);
                smtp.Dispose();
            }
        }
    }
    }
