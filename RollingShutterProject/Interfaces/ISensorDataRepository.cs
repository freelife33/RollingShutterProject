using RollingShutterProject.Models;

namespace RollingShutterProject.Interfaces
{
    public interface ISensorDataRepository : IRepository<SensorData>
    {
        Task<IEnumerable<SensorData>> GetSensorDataByDeviceIdAsync(int deviceId);
        Task<SensorData> GetSensorDataByIdAsync(int id);
        Task<SensorData?> GetLastSensorData(int deviceId, string sensorType);
    }
}
