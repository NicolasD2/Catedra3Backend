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
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["JwtKey"] ?? throw new InvalidOperationException("JWT Key must be configured in appsettings."));
            var emailClaim = new Claim(ClaimTypes.Email, user.Email ?? string.Empty);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Email, user.Email),
                    // Agrega más claims si es necesario
                }),
                Expires = DateTime.UtcNow.AddDays(7), // Configura la expiración según necesidades
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}