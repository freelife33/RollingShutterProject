using Microsoft.EntityFrameworkCore;
using RollingShutterProject.Data;
using RollingShutterProject.Interfaces;
using RollingShutterProject.Models;

namespace RollingShutterProject.Repositories
{
    public class DeviceRepository : Repository<Device>, IDeviceRepository
    {
        public DeviceRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Device?> GetDeviceBySensorType(string sensorType)
        {
            return await _context.Devices
        .Where(d => d.Name == sensorType)
        .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Device?>> GetDevicesByStatus(string status)
        {
            return await _context.Devices.Where(d => d.Status == status).ToListAsync();
        }
    }
}
