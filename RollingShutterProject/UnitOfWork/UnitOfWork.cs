using RollingShutterProject.Data;
using RollingShutterProject.Interfaces;
using RollingShutterProject.Repositories;

namespace RollingShutterProject.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Devices = new DeviceRepository(_context);
            SensorData = new SensorDataRepository(_context);
            Users = new UserRepository(_context);
            UserSettings = new UserSettingsRepository(_context);
            SystemSettings=new SystemSettingsRepository(_context);
            UserCommands = new UserCommandRepository(_context);
        }

        public IDeviceRepository Devices { get; private set; }

        public ISensorDataRepository SensorData { get; private set; }

        public IUserRepository Users { get; private set; }

        public IUserSettings UserSettings { get; private set; }
        public ISystemSettingsRepository SystemSettings { get; private set; }
        public IUserCommandRepository UserCommands { get; set; }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
