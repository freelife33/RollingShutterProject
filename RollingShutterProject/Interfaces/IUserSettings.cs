using RollingShutterProject.Models;

namespace RollingShutterProject.Interfaces
{
    public interface IUserSettings: IRepository<UserSettings>
    {
        Task<UserSettings?> GetUserSettings(int userId);
    }
}
