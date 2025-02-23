using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RollingShutterProject.Models
{
    public class SensorData
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [JsonPropertyName("deviceId")]
        public string? DeviceIdString { get; set; }  // JSON'dan gelen string değer

        [Required]
        public int DeviceId { get; set; }  // Artık hem okunabilir hem yazılabilir

        [Required]
        [JsonPropertyName("sensorType")]
        public string? SensorType { get; set; }

        [Required]
        [JsonPropertyName("value")]
        public float Value { get; set; }

        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;

        [ForeignKey("DeviceId")]
        public Device? Device { get; set; }
    }
}
