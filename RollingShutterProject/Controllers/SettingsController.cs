using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RollingShutterProject.Models;
using RollingShutterProject.UnitOfWork;

namespace RollingShutterProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public SettingsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserSettings()
        {
            var settings = await _unitOfWork.UserSettings.GetUserSettings();
            if (settings == null)
            {
                return NotFound("Kullanıcı ayarları bulunamadı.");
            }
            return Ok(settings);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateSystemSettings([FromBody] UserSettings settings)
        {
            var existingSettings = await _unitOfWork.UserSettings.GetUserSettings();
            if (existingSettings == null)
            {
                await _unitOfWork.UserSettings.AddAsync(settings);
            }
            else
            {
                existingSettings.LoggingIntervalHours = settings.LoggingIntervalHours;
                existingSettings.DetectAnomalies = settings.DetectAnomalies;
                existingSettings.NotifyOnHighTemperature = settings.NotifyOnHighTemperature;
                existingSettings.NotifyOnPoorAirQuality = settings.NotifyOnPoorAirQuality;
                existingSettings.AutoOpenShutter = settings.AutoOpenShutter;
                                
                existingSettings.AutoOpenShutterOnTime = settings.AutoOpenShutterOnTime;
                existingSettings.AtoCloseShutterOnTime = settings.AtoCloseShutterOnTime;
                existingSettings.OpenTime = settings.OpenTime;
                existingSettings.CloseTime = settings.CloseTime;
            }

            await _unitOfWork.CompleteAsync();
            return Ok(new { message = "Sistem ayarları başarıyla güncellendi." });
        }
    }
}
