using System.ComponentModel.DataAnnotations;

namespace RollingShutterMVC.Models.ViewsModels
{
    public class DeviceCreateViewModel
    {
        [Required(ErrorMessage = "Cihaz adı gereklidir")]
        public string? DeviceName { get; set; }
    }
}
