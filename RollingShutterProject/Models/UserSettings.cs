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
        public bool AutoOpenShutterOnHighTemperature { get; set; }
        public bool AutoOpenShutterOnPoorAirQuality { get; set; }
        public float HighTemperatureThreshold { get; set; }
        public float PoorAirQualityThreshold { get; set; }
        public bool AutoOpenShutter { get; set; }


        //Zamana göre açma kapama
        public bool AutoOpenShutterOnTime { get; set; }
        public bool AtoCloseShutterOnTime { get; set; }
        public TimeSpan? OpenTime { get; set; }
        public TimeSpan? CloseTime { get; set; }


        public string? MqttServer { get; set; }
        public string? MqttClientId { get; set; }
        public string? MqttTopic { get; set; }
        public string? MqttPayload { get; set; }

        // Gelecekte eklenebilecek başka ayarlar için boş alan
    }
}
