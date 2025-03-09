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
    }
}
