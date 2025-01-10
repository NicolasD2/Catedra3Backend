using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using PostCatedraApi.src.Dtos.Usuario;
using PostCatedraApi.src.Models;
using PostCatedraApi.src.Interfaces;

namespace PostCatedraApi.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthController(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto model)
        {
            var user = new Usuario { UserName = model.Email, Email = model.Email };
            var result = await _userRepository.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return Ok();
            }
                
            var errorMessage = "No se pudo registrar al usuario. ";

            
            if (result.Errors.Any(e => e.Code == "DuplicateUserName"))
            {
                errorMessage += "El email ya está en uso.";
            }
            else
            {
                errorMessage += "Por favor, verifica los datos ingresados.";
            }

            // Devolver BadRequest con el mensaje personalizado.
            return BadRequest(new { Message = errorMessage, Errors = result.Errors });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var result = await _userRepository.PasswordSignInAsync(model.Email, model.Password, false, false);
            if (result.Succeeded)
            {
                var user = await _userRepository.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return Unauthorized("No such user.");
                }
                var token = GenerateJwtToken(user);
                return Ok(new { Token = token });
            }
            return Unauthorized("Invalid credentials.");
        }

        private string GenerateJwtToken(Usuario user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null when generating JWT token.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var keyString = _configuration["JWT:SigningKey"];
            if (string.IsNullOrWhiteSpace(keyString))
            {
                throw new InvalidOperationException("JWT Signing Key must be configured in appsettings.");
            }

            var key = Encoding.UTF8.GetBytes(keyString);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
