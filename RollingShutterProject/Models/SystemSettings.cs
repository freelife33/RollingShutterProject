using System.ComponentModel.DataAnnotations;

namespace RollingShutterProject.Models
{
    public class SystemSettings
    {
        [Key]
        public int Id { get; set; }
        public bool IsConfigured { get; set; } = false;
    }
}
