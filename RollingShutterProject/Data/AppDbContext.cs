using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using RollingShutterProject.Models;

namespace RollingShutterProject.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base (options)
        {
            
        }

        public DbSet<Device> Devices { get; set; }
        public DbSet<SensorData> SensorData { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserSettings> UserSettings { get; set; }
        public DbSet<SystemSettings> SystemSettings { get; set; }
        public DbSet<UserCommand> UserCommands { get; set; }

    }
}
