using RollingShutterProject.Models;

namespace RollingShutterProject.Interfaces
{
    public interface IUserRepository:IRepository<User>
    {
        Task<User> GetByUsernameAsync(String user);
    }
}
