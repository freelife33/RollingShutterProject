using RollingShutterProject.Models;

namespace RollingShutterProject.Interfaces
{
    public interface ISystemSettingsRepository: IRepository<SystemSettings>
    {
        Task<SystemSettings> GetSettingsAsync();
    }
}
