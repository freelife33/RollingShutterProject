using System.Net.Mail;
using System.Net;
using RollingShutterProject.Interfaces;

namespace RollingShutterProject.Services
{
    public class NotificationService: INotificationService
    {
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(ILogger<NotificationService> logger)
        {
            _logger = logger;
        }

        public async Task SendEmailAsync(string subject, string body)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("ademdigil33@gmail.com", "ifyp bpyw asxe kmrc"),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("ademdigil33@gmail.com"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add("a.digil@hotmail.com");

                await smtpClient.SendMailAsync(mailMessage);
                _logger.LogInformation($"E-Posta gönderildi: {subject}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"E-Posta gönderme hatası: {ex.Message}");
            }
        }
    }
}
