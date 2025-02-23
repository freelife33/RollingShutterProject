using Microsoft.AspNetCore.Mvc;
using RollingShutterMVC.Filters;
using RollingShutterMVC.Models;
using RollingShutterMVC.Models.DTOs;
using RollingShutterProject.Models;

//using RollingShutterProject.Models;
using RollingShutterProject.UnitOfWork;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;

namespace RollingShutterMVC.Controllers
{

    [JwtAuthFilter]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, HttpClient httpClient, IConfiguration configuration)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _httpClient = httpClient;
            _apiBaseUrl = configuration["BackendApiUrl"];
        }

        public async Task<IActionResult> Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("JWTToken")))
            {
                return RedirectToAction("Login", "Account");
            }
            var settings = await _unitOfWork.SystemSettings.GetSettingsAsync();

            if (settings == null || !settings.IsConfigured)
            {
                return RedirectToAction("Index", "Wizard");
            }


            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public async Task<IActionResult> GetDevices()
        {
            var token = HttpContext.Session.GetString("JWTToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("JWTToken"));

            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/device");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Yetkilendirme baþarýsýz veya API eriþilemez.";
                return View("Error");
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            var devices = JsonSerializer.Deserialize<List<Models.Device>>(responseBody);

            return View(devices);
        }

        [HttpGet]
        public IActionResult GetSensorData()
        {
            var sensorData = new List<SensorDataDto>
    {
        new SensorDataDto { SensorType = "Sýcaklýk", Value = 25.3f, TimeStamp = DateTime.Now.AddMinutes(-5) },
        new SensorDataDto { SensorType = "Nem", Value = 45.8f, TimeStamp = DateTime.Now.AddMinutes(-4) },
        new SensorDataDto { SensorType = "Basýnç", Value = 1013.5f, TimeStamp = DateTime.Now.AddMinutes(-3) },
        new SensorDataDto { SensorType = "HavaKalitesi", Value = 85.2f, TimeStamp = DateTime.Now.AddMinutes(-2) }
    };


            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // JavaScript tarafýnda camelCase kullanýlýr
                WriteIndented = false, // JSON'u sýkýþtýrýr
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping // Türkçe karakter desteði
            };
            return Json(sensorData, options);
        }
    }
}
