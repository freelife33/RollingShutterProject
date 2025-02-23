using System.ComponentModel.DataAnnotations;

namespace RollingShutterProject.Models
{
    public class UserSettings
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        
        public int LoggingIntervalHours { get; set; } = 3;
        public bool DetectAnomalies { get; set; } = true; 

        
        public bool NotifyOnHighTemperature { get; set; } = true; 
        public bool NotifyOnPoorAirQuality { get; set; } = true; 

        
        public bool AutoOpenShutter { get; set; } = false;

        public string? MqttServer { get; set; }
        public string? MqttClientId { get; set; }
        public string? MqttTopic { get; set; }
        public string? MqttPayload { get; set; }

        // Gelecekte eklenebilecek başka ayarlar için boş alan
    }
}
