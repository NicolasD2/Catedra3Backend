using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PostCatedraApi.src.Dtos.Usuario;
using PostCatedraApi.src.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PostCatedraApi.src
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController:ControllerBase
    {
        private readonly UserManager<Usuario>_userManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<Usuario> userManager, SignInManager<Usuario>signInManager, IConfiguration configuration){
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }
        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto model){
            var user = new Usuario{UserName = model.Email, Email = model.Email};
            var result = await _userManager.CreateAsync(user, model.Password);

            if(result.Succeeded){
                return Ok();
            }
            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginDto model){
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
            if(result.Succeeded){
                var user = await _userManager.FindByEmailAsync(model.Email);
                if(user == null){
                    throw new ArgumentNullException(nameof(user));
                }
                var token = GenerateJwtToken(user);
                return Ok(new{Token = token});
            }
            return Unauthorized("Credenciales invalidas");
        }
        private string GenerateJwtToken(Usuario user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null when generating JWT token.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var keyString = _configuration["JWT:SigningKey"]; // Asegúrate de que la clave en appsettings.json esté bajo "JWT:SigningKey"
            
            if (string.IsNullOrWhiteSpace(keyString))
            {
                throw new InvalidOperationException("JWT Signing Key must be configured in appsettings.");
            }

            var key = Encoding.UTF8.GetBytes(keyString);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // JTI for token id
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7), // Configura la duración del token según tus necesidades
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}