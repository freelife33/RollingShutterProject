using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace RollingShutterMVC.Controllers
{
    public class AccountController : Controller
    {

        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;
        private readonly ILogger<AccountController> _logger;

        public AccountController(HttpClient httpClient, IConfiguration configuration, ILogger<AccountController> logger)
        {
            _httpClient = httpClient;
            _apiBaseUrl = configuration["BackendApiUrl"]!;
            _logger = logger;
        }
        public IActionResult Login()
        {
            
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
           
               
            var loginRequest = new { Username = username, PasswordHash = password };
            var json = JsonSerializer.Serialize(loginRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_apiBaseUrl}/auth/login", content);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Geçersiz kullanıcı adı veya şifre.";
                return View();
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var token = JsonSerializer.Deserialize<JwtResponse>(responseContent)?.token;

            if (token != null)
            {
                HttpContext.Session.SetString("JWTToken", token);
                _logger.LogInformation("JWT alındı ve HttpOnly Cookie'ye kaydedildi.");
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Bir hata oluştu.";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); 
            return RedirectToAction("Login", "Auth");
        }
    }

    public class JwtResponse
    {
        public string? token { get; set; }
    }
}
