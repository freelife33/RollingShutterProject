using Microsoft.EntityFrameworkCore;
using RollingShutterProject.Data;
using RollingShutterProject.Interfaces;
using RollingShutterProject.Models;

namespace RollingShutterProject.Repositories
{
    public class UserCommandRepository: Repository<UserCommand>, IUserCommandRepository
    {
        public UserCommandRepository(AppDbContext context) : base(context)
        {
        }        
        public async Task<IEnumerable<UserCommand>> GetCommandsByUserIdAsync(int userId)
        {
            return await _context.UserCommands.Where(uc => uc.UserId == userId).ToListAsync();
        }

        public async Task<UserCommand?> GetLastManualUserCommandAsync()
        {
            return await _context.UserCommands
                .OrderByDescending(c => c.TimeStamp)
                .FirstOrDefaultAsync();
        }

        public async Task<List<UserCommand>> GetRecentCommandsAsync(int count)
        {
            return await _context.UserCommands
        .OrderByDescending(c => c.TimeStamp)
        .Take(count)
        .ToListAsync();
        }
    }
}
