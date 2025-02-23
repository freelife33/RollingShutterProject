using Microsoft.AspNetCore.Mvc;
using RollingShutterMVC.Filters;
using RollingShutterProject.Models.DTOs;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace RollingShutterMVC.Controllers
{
    [Route("[controller]")]
    [JwtAuthFilter]
    public class DeviceController : Controller
    {

        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public DeviceController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiBaseUrl = configuration["BackendApiUrl"]!;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("Control")]
        public IActionResult Control()
        {
           
            return View();
        }
        [HttpPost("send-command-js")]
        public async Task<IActionResult> SendCommand([FromBody] CommandRequest command)
        {
            string? token = HttpContext.Session.GetString("JWTToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            if (command == null || string.IsNullOrEmpty(command.Command))
            {
                return BadRequest("Komut boş olamaz.");
            }

            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var payload = new { Command = command.Command };
            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_apiBaseUrl}/device/SendCommand", content);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Komut gönderme başarısız.");
            }

            return Ok(new { Message = "Komut gönderildi" });
        }

    }
}
