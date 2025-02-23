using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RollingShutterProject.Models;
using RollingShutterProject.UnitOfWork;

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
    }
}
