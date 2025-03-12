using Microsoft.EntityFrameworkCore;
using RollingShutterProject.Data;
using RollingShutterProject.Interfaces;
using RollingShutterProject.Models;

namespace RollingShutterProject.Repositories
{
    public class SensorDataRepository : Repository<SensorData>, ISensorDataRepository
    {
        private readonly AppDbContext _context;
        public SensorDataRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<SensorData?> GetLastSensorData(string sensorType, int? deviceId )
        {
            return await _context.SensorData
            .Where(s => s.SensorType == sensorType && (!deviceId.HasValue || s.DeviceId == deviceId))
            .OrderByDescending(s => s.TimeStamp)
            .FirstOrDefaultAsync();
        }


        public async Task<IEnumerable<SensorData>> GetSensorDataByDeviceIdAsync(int deviceId)
        {
            return await _context.SensorData.Where(s => s.DeviceId == deviceId).ToListAsync();
        }

        public async Task<SensorData?> GetSensorDataByIdAsync(int id)
        {
            return await _context.SensorData.FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}
