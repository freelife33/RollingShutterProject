namespace RollingShutterProject.Interfaces
{
    public interface INotificationService
    {
        Task SendEmailAsync(string subject, string body);
    }
}
