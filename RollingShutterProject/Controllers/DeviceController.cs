using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RollingShutterProject.Interfaces;
using RollingShutterProject.Models;
using RollingShutterProject.UnitOfWork;
using RollingShutterProject.Models.DTOs;

namespace RollingShutterProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DeviceController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMqttService _mqttService;
        public DeviceController(IUnitOfWork unitOfWork, IMqttService mqttService)
        {
            _unitOfWork = unitOfWork;
            _mqttService = mqttService;
        }


        [HttpPost("SendCommand")]
        public async Task<IActionResult> SendCommand([FromBody] CommandRequest command)
        {
            if (command == null || string.IsNullOrEmpty(command.Command))
            {
                return BadRequest(new { Message = "Komut boş olamaz" });
            }
            await _mqttService.PublishMessageAsync("device/command", command!.Command!);
            return Ok(new { Message = "Komut gönderildi" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDevices()
        {
            var devices = await _unitOfWork.Devices.GetAllAsync();
            return Ok(devices);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDeviceById(int id)
        {
            var device = await _unitOfWork.Devices.GetByIdAsync(id);
            if (device == null)
            {
                return NotFound(new {message="Cihaz bulunamadı"});
            }
            return Ok(device);
        }

        [HttpPost]
        public async Task<IActionResult> AddDevice([FromBody] Device device)
        {
            if (device == null) {
                return BadRequest(new { message = "Geçersiz cihaz verisi" });
            }
            await _unitOfWork.Devices.AddAsync(device);
            await _unitOfWork.CompleteAsync();
            return CreatedAtAction(nameof(GetDeviceById), new { id=device.Id }, device);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDevice(int id, [FromBody] Device updateDevice)
        {
            var existingDevice = await _unitOfWork.Devices.GetByIdAsync(id);
            if (existingDevice == null)
                return NotFound(new { message = "Cihaz Bulunamadı" });

            existingDevice.Name = updateDevice.Name;
            existingDevice.Status = updateDevice.Status;

            _unitOfWork.Devices.Update(existingDevice);
            await _unitOfWork.CompleteAsync();
            return Ok(existingDevice);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDevice(int id)
        {
            var device = await _unitOfWork.Devices.GetByIdAsync(id);
            if (device == null) { 
            return NotFound(new {message= "Cihaz Bulunamadı"});
            }
            _unitOfWork.Devices.Remove(device);
            await _unitOfWork.CompleteAsync();
            return Ok(new {message="Cihaz başarıyla silindi."});
        }
    }
}
