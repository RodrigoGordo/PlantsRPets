using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PlantsRPetsProjeto.Server.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PlantsRPetsProjeto.Server.Controllers
{
    [ApiController]
    public class UserAccountsController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public UserAccountsController(UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost("api/signup")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationModel model)
        {
            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
                return BadRequest(new { message = "User already exists." });

            var user = new User { UserName = model.Email, Email = model.Email, Nickname = model.Nickname, RegistrationDate = DateTime.UtcNow};

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return Ok(new { message = "User registered successfully." });
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("api/signin")]
        public async Task<IActionResult> Login([FromBody] UserLoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest(new { message = "Invalid email or password." });
            }
            var passwordValid = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!passwordValid)
            {
                return BadRequest(new { message = "Invalid email or password." });
            }

            // Gerar os claims do utilizador
            var claims = new List<Claim>
            {
                new Claim("Nickname", user.Nickname),
                new Claim("Email", user.Email),
                new Claim("UserId", user.Id)
            };

            // Gerar o token JWT
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1), // Definir o tempo de expiração
                signingCredentials: creds);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            // Retornar o token ao cliente
            return Ok(new
            {
                token = tokenString,
                expiration = token.ValidTo
            });
        }

    }

    public class UserLoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }


    public class UserRegistrationModel
    {
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }
}

