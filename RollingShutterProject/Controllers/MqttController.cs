using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RollingShutterProject.Interfaces;

namespace RollingShutterProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class MqttController : ControllerBase
    {
        private readonly IMqttService _mqttService;

        public MqttController(IMqttService mqttService)
        {
            _mqttService = mqttService;
        }

        [HttpPost("publish")]
        public async Task<IActionResult> PublishMessage([FromBody] MqttMessageRequest request)
        {
            if (string.IsNullOrEmpty(request.Topic) || string.IsNullOrEmpty(request.Message))
                return BadRequest("Topic ve Message zorunludur.");

            await _mqttService.PublishMessageAsync(request.Topic, request.Message);
            return Ok(new { message = "MQTT mesajı gönderildi.", topic = request.Topic });
        }
    }
}
public class MqttMessageRequest
{
    public string? Topic { get; set; }
    public string? Message { get; set; }
}