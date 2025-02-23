using Microsoft.EntityFrameworkCore;
using RollingShutterProject.Data;
using RollingShutterProject.Interfaces;
using RollingShutterProject.Models;

namespace RollingShutterProject.Repositories
{
    public class SystemSettingsRepository : Repository<SystemSettings>, ISystemSettingsRepository
    {
        public SystemSettingsRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<SystemSettings?> GetSettingsAsync()
        {
            return await _context.SystemSettings.FirstOrDefaultAsync();
        }
    }
}
