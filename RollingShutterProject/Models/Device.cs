using System.ComponentModel.DataAnnotations;

namespace RollingShutterProject.Models
{
    public class Device
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }

        public string? Type { get; set; }

        [Required]
        public string? Status { get; set; }
    }
}
