using Microsoft.AspNetCore.Mvc;
using RollingShutterProject.UnitOfWork;

namespace RollingShutterMVC.Controllers
{
    public class SensorController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public SensorController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var sensorData = await _unitOfWork.SensorData.GetAllAsync();
            return View(sensorData);
        }
        public async Task<IActionResult> PartialSensorData()
        {
            var sensorData = await _unitOfWork.SensorData.GetAllAsync();
            return PartialView(sensorData);
        }
    }
}
