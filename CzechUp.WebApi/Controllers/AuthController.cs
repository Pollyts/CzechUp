using CzechUp.EF.Models;
using CzechUp.Services;
using CzechUp.Services.DTOs;
using CzechUp.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CzechUp.WebApi.Controllers
{  
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
        private readonly IUserService userService;
        private readonly IConfiguration _config;
        public AuthController(IConfiguration config, IUserService userService)
        {
            _config = config;
            this.userService = userService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDto request)
        {
            var user = userService.Login(request);
            if (user != null)
            {
                var token = GenerateJwtToken(user);
                return Ok(new { token });
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost("register")]
        public IActionResult Register(RegistrationRequestDto request)
        {
            var result = userService.CreateUser(request);
            return Ok(result);
        }  

        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Guid.ToString()) // Добавляем ID
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
