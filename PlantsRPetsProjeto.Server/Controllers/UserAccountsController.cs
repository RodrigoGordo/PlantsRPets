using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PlantsRPetsProjeto.Server.Models;
using PlantsRPetsProjeto.Server.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
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

        [HttpPost("api/forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model, [FromServices] IEmailService emailService)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null)
                    return BadRequest(new { message = "The email doesn't exist." });

                // Gera o token de recuperação da pass
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                // Encode seguro do token
                var encodedToken = WebUtility.UrlEncode(token);

                var resetLink = $"{_configuration["Frontend:BaseUrl"]}/reset-password?email={user.Email}&token={encodedToken}";

                // Enviar email com o link de redefinição de senha
                await emailService.SendEmailAsync(
                    user.Email!,
                    "Password Reset Request",
                    $"<p>Hello {user.Nickname},</p>" +
                    $"<p>We received a request to reset your password. Click the link below to proceed:</p>" +
                    $"<p><a href='{resetLink}'>Reset Password</a></p>" +
                    $"<p>If you didn't request this, please ignore this email.</p>"
                );

                return Ok(new { message = "A password reset link has been sent to your email." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] {ex.ToString()}");
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }

        [HttpPost("api/reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                    return BadRequest(new { message = "Invalid request." });

                var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);

                if (result.Succeeded)
                    return Ok(new { message = "Password reset successfully." });

                return BadRequest(new
                {
                    message = "Failed to reset password.",
                    errors = result.Errors.Select(e => e.Description) //pode-se apagar depois
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] {ex.Message}");
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
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

    public class ForgotPasswordModel
    {
        public string Email { get; set; }
    }

    public class ResetPasswordModel
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}

