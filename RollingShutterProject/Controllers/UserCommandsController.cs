using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RollingShutterProject.UnitOfWork;

namespace RollingShutterProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserCommandsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _apiBaseUrl;
        public UserCommandsController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _apiBaseUrl = configuration["BackendApiUrl"]!;
        }

        [HttpGet("logs")]
        public async Task<IActionResult> GetUserLogs()
        {
            var logs = await _unitOfWork.UserCommands.GetRecentCommandsAsync(10);

            return Ok(logs);
        }
    }
}
