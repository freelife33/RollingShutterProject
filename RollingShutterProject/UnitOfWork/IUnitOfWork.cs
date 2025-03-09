using RollingShutterProject.Interfaces;

namespace RollingShutterProject.UnitOfWork
{
    public interface IUnitOfWork:IDisposable
    {
        IDeviceRepository Devices { get; }
        ISensorDataRepository SensorData { get; }
        IUserRepository Users { get; }
        IUserSettings UserSettings { get; }
        ISystemSettingsRepository SystemSettings { get; }
        IUserCommandRepository UserCommands { get; }
        Task<int> CompleteAsync();
    }
}
