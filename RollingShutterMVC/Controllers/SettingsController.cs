using Microsoft.AspNetCore.Mvc;
using RollingShutterMVC.Filters;
using RollingShutterProject.Models;
using System.Text;
using System.Text.Json;

namespace RollingShutterMVC.Controllers
{
    [JwtAuthFilter]
    public class SettingsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public SettingsController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiBaseUrl = configuration["BackendApiUrl"]!;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}/settings/1");
                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError(string.Empty, "Ayarlar alınamadı.");
                    return View(new UserSettings());
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var userSettings = JsonSerializer.Deserialize<UserSettings>(jsonResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return View(userSettings);
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, $"API bağlantı hatası: {ex.Message}");
                return View(new UserSettings());
            }            
        }

        [HttpPost]
        public async Task<IActionResult> Save(UserSettings model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            try
            {
                var json = JsonSerializer.Serialize(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"{_apiBaseUrl}/settings/1", content);

                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError(string.Empty, "Ayarlar kaydedilemedi.");
                    return View("Index", model);
                }

                return RedirectToAction("Index");
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, $"API bağlantı hatası: {ex.Message}");
                return View("Index", model);
            }
        }
    }
}
