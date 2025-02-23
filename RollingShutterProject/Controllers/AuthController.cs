using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RollingShutterProject.Models;
using RollingShutterProject.Services;
using RollingShutterProject.UnitOfWork;
using System.Security.Cryptography;
using System.Text;

namespace RollingShutterProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtService _jwtService;

        public AuthController(IUnitOfWork unitOfWork, JwtService jwtService)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (await _unitOfWork.Users.GetByUsernameAsync(user.Username!) != null)
            {
                return BadRequest(new { message = "Bu kullanıcı adı zaten alınmış." });
            }

            user.PasswordHash = HashPassword(user.PasswordHash);
            user.Role = "User";

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CompleteAsync();

            return Ok(new {message="Kullanıcı kaydı başarılı."});
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {

            var existingUser= await _unitOfWork.Users.GetByUsernameAsync(user.Username!);
            if (existingUser == null || existingUser.PasswordHash !=HashPassword(user.PasswordHash)) 
            { 
            
                return Unauthorized(new {message="Geçersiz kullanıcı adı veya şifre."});
            
            }
            var token = _jwtService.GenerateToken(existingUser);
            return Ok(new { token });
        }

        private string? HashPassword(string? password)
        {
            using (var sha256 = SHA256.Create()) { 
                var bytes= sha256.ComputeHash(Encoding.UTF8.GetBytes(password!));
                return Convert.ToBase64String(bytes);
            }
        }
    }
}
