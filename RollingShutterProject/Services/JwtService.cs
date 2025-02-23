using Microsoft.IdentityModel.Tokens;
using RollingShutterProject.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.AccessControl;
using System.Security.Claims;
using System.Text;

namespace RollingShutterProject.Services
{
    public class JwtService
    {
        private readonly IConfiguration _configuration;
        private readonly string _secretKey;
        private readonly int _expiryDuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
            _secretKey = _configuration["Jwt:Key"]!;
            _expiryDuration = int.Parse(_configuration["Jwt:ExpiryMinutes"]!);
        }

        public string GenerateToken(User user) {


            var tokenHandler = new JwtSecurityTokenHandler();
            var key=Encoding.ASCII.GetBytes(_secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject=new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Username!),
                    new Claim(ClaimTypes.Role, user.Role!)
                }),
                Expires=DateTime.UtcNow.AddMinutes(_expiryDuration),
                SigningCredentials=new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)

            };

            var token=tokenHandler.CreateToken(tokenDescriptor);


            return tokenHandler.WriteToken(token);
        }
    }
}
