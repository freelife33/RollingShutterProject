using System.ComponentModel.DataAnnotations;

namespace RollingShutterProject.Models.DTOs
{
    public class CommandRequest
    {
        [Required(ErrorMessage = "Komut zorunludur.")]
        public string? Command { get; set; }

        public int? Value { get; set; }
    }
}
