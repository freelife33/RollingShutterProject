using Microsoft.AspNetCore.Mvc;
using RollingShutterMVC.Models.ViewsModels;
using RollingShutterProject.Models;
using RollingShutterProject.UnitOfWork;
using System.Security.Cryptography;
using System.Text;

namespace RollingShutterMVC.Controllers
{
    public class WizardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public WizardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        private async Task<bool> IsConfigured()
        {
            var settings = await _unitOfWork.SystemSettings.GetSettingsAsync();
            return settings != null && settings.IsConfigured;
        }

        public async Task<IActionResult> Index()
        {
            if (await IsConfigured())
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Step1");

        }

        public async Task<IActionResult> Step1()
        {
            if (await IsConfigured())
            {
                return RedirectToAction("Index", "Home");
            }
            return View(new UserCreateViewModel());
        }
            

        [HttpPost]
        public async Task<IActionResult> Step1(UserCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); 
            }

            
            var user = new User
            {
                Username = model.Username,
                PasswordHash = HashPassword(model.Password),
                Role="Admin"
                
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CompleteAsync();

            return RedirectToAction("Step2");
        }
        public async Task<IActionResult> Step2()
        {
            if (await IsConfigured())
            {
                return RedirectToAction("Index", "Home");
            }
            return View(new DeviceCreateViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Step2(DeviceCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var device = new Device
            {
                Name = model.DeviceName,
                Status = "Aktif" 
            };

            await _unitOfWork.Devices.AddAsync(device);
            await _unitOfWork.CompleteAsync();

            return RedirectToAction("Step3");
        }
        public async Task<IActionResult> Step3()
        {
            if (await IsConfigured())
            {
                return RedirectToAction("Index", "Home");
            }
            return View(new UserSettingsViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Step3(UserSettingsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userSettings = new UserSettings
            {
                LoggingIntervalHours = model.LoggingIntervalHours,
                DetectAnomalies = model.DetectAnomalies,
                AutoOpenShutter = model.AutoOpenShutter
            };

            await _unitOfWork.UserSettings.AddAsync(userSettings);

            var systemSettings = await _unitOfWork.SystemSettings.GetSettingsAsync();
            if (systemSettings == null)
            {
                systemSettings = new SystemSettings { IsConfigured = true };
                await _unitOfWork.SystemSettings.AddAsync(systemSettings);
            }
            else
            {
                systemSettings.IsConfigured = true;
            }

            await _unitOfWork.CompleteAsync();

            return RedirectToAction("Success");
        }
        public IActionResult Success()
        {
            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> CompleteSetup()
        //{
        //    var settings = new SystemSettings { IsConfigured = true };
        //    await _unitOfWork.SystemSettings.AddAsync(settings);
        //    await _unitOfWork.CompleteAsync();

        //    return RedirectToAction("Login", "Account");
        //}


        private string? HashPassword(string? password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password!));
                return Convert.ToBase64String(bytes);
            }
        }
    }
}
