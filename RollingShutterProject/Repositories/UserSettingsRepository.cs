using Microsoft.EntityFrameworkCore;
using RollingShutterProject.Data;
using RollingShutterProject.Interfaces;
using RollingShutterProject.Models;

namespace RollingShutterProject.Repositories
{
    public class UserSettingsRepository : Repository<UserSettings>, IUserSettings
    {
        public UserSettingsRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<UserSettings?> GetUserSettings()
        {
            return await _context.UserSettings.FirstOrDefaultAsync();
        }
    }
}
