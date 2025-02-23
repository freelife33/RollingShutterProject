using RollingShutterProject.Models;

namespace RollingShutterProject.Interfaces
{
    public interface IDeviceRepository : IRepository<Device>
    {
        Task<IEnumerable<Device?>> GetDevicesByStatus(string status);
        Task<Device?> GetDeviceBySensorType(string sensorType);
    }
}
