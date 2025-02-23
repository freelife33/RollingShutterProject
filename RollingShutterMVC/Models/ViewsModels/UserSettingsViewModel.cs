using System.ComponentModel.DataAnnotations;

namespace RollingShutterMVC.Models.ViewsModels
{
    public class UserSettingsViewModel
    {
        [Required]
        public int LoggingIntervalHours { get; set; }

        public bool DetectAnomalies { get; set; }

        public bool AutoOpenShutter { get; set; }
    }
}
