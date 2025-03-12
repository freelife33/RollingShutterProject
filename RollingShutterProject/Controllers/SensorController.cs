using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RollingShutterProject.Models;
using RollingShutterProject.UnitOfWork;
using System.Text.RegularExpressions;

namespace RollingShutterProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SensorController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public SensorController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSensorData()
        {
            var sensorData= await _unitOfWork.SensorData.GetAllAsync();
            return Ok(sensorData);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSensorDataById(int id)
        {
            var sensor = await _unitOfWork.SensorData.GetByIdAsync(id);
            if (sensor==null)
            {
                return NotFound(new { message = "Sensör verisi bulunamadı." });

            }

            return Ok(sensor);
        }

        [HttpPost]
        public async Task<IActionResult> AddSensorData([FromBody] SensorData sensorData)
        {
            if (sensorData == null)
                return BadRequest(new { message = "Geçersiz Sensör verisi" });
            await _unitOfWork.SensorData.AddAsync(sensorData);
            await _unitOfWork.CompleteAsync();
            return CreatedAtAction(nameof(GetSensorDataById), new {id=sensorData.Id}, sensorData);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSensorData(int id)
        {
            var sensor =await _unitOfWork.SensorData.GetByIdAsync(id);
            if (sensor == null)
                return NotFound(new { message = "Sensör verisi bulunamadı" });

            _unitOfWork.SensorData.Remove(sensor);
            await _unitOfWork.CompleteAsync();
            return Ok(new {message="Sensör verisi başarıyla silindi"});
        }
        [AllowAnonymous]
        [HttpGet("latest")]
        public async Task<IActionResult> GetLatestSensorData()
        {
           

            var latestTemperature = await _unitOfWork.SensorData.GetLastSensorData("Sıcaklık",null);
            var latestAirQuality = await _unitOfWork.SensorData.GetLastSensorData("Hava Kalitesi",null);

            return Ok(new
            {
                temperature = latestTemperature?.Value,
                airQuality = latestAirQuality?.Value,
                timestamp = DateTime.UtcNow
            });
        }
        [AllowAnonymous]
        [HttpGet("shutter/status")]
        public async Task<IActionResult> GetShutterStatus()
        {
            var lastCommand = await _unitOfWork.UserCommands.GetLastManualUserCommandAsync();

            if (lastCommand == null)
            {
                return Ok(new
                {
                    status = "Bilinmiyor",
                    openPercentage = 0,
                    isAuto = false
                });
            }

            int percentage = 0;

            
            if (int.TryParse(lastCommand.Command, out int parsedValue))
            {
                percentage = parsedValue;
            }
            else if (lastCommand.Command!.Contains("CLOSE", StringComparison.OrdinalIgnoreCase) ||
                     lastCommand.Command.Contains("kapandı", StringComparison.OrdinalIgnoreCase))
            {
                percentage = 0;
            }
            else if (lastCommand.Command.Contains("OPEN", StringComparison.OrdinalIgnoreCase) ||
                     lastCommand.Command.Contains("açıldı", StringComparison.OrdinalIgnoreCase))
            {
                percentage = 100;
            }

            string statusText = percentage switch
            {
                0 => "Kapalı",
                100 => "Tam Açık",
                _ => $"%{percentage} Açık"
            };

            
            if (lastCommand.Command.Contains("Ortam koşulu nedeniyle", StringComparison.OrdinalIgnoreCase))
            {
                statusText += " (Otomatik)";
            }
            else
            {
                statusText += " (Manuel)";
            }

            return Ok(new
            {
                status = statusText,
                openPercentage = percentage,
                isAuto = lastCommand.IsAuto
            });
        }






    }
}
