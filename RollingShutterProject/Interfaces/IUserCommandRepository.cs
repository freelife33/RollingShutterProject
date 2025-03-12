using RollingShutterProject.Models;
using RollingShutterProject.Repositories;

namespace RollingShutterProject.Interfaces
{
    public interface IUserCommandRepository: IRepository<UserCommand>
    {
        //Task AddAsync(UserCommand userCommand);
        Task<IEnumerable<UserCommand>> GetCommandsByUserIdAsync(int userId);
        Task<UserCommand?> GetLastManualUserCommandAsync();
        Task<List<UserCommand>> GetRecentCommandsAsync(int count);
    }
}
