using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NuGet.Common;
using PlantsRPetsProjeto.Server.Data;
using PlantsRPetsProjeto.Server.Models;
using PlantsRPetsProjeto.Server.Services;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace PlantsRPetsProjeto.Server.Controllers
{
    /// <summary>
    /// Controlador responsável pela gestão de contas de utilizador.
    /// Inclui funcionalidades de registo, autenticação, recuperação e redefinição de palavra-passe.
    /// </summary>
    [ApiController]
    public class UserAccountsController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly PlantsRPetsProjetoServerContext _context;

        /// <summary>
        /// Inicializa uma nova instância do <see cref="UserAccountsController"/>.
        /// </summary>
        /// <param name="userManager">Serviço de gestão de utilizadores.</param>
        /// <param name="configuration">Configurações da aplicação, incluindo chaves JWT.</param>
        /// <param name="emailService">Serviço para envio de e-mails.</param>
        public UserAccountsController(UserManager<User> userManager, IConfiguration configuration, IEmailService emailService, PlantsRPetsProjetoServerContext context)
        {
            _userManager = userManager;
            _configuration = configuration;
            _emailService = emailService;
            _context = context;
        }

        /// <summary>
        /// Regista um novo utilizador na aplicação.
        /// </summary>
        /// <param name="model">Modelo contendo o e-mail, nickname e palavra-passe do novo utilizador.</param>
        /// <returns>Retorna um código HTTP 200 se o registo for bem-sucedido ou 400 em caso de erro.</returns>
        [HttpPost("api/signup")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationModel model)
        {
            try
            {
                var userExists = await _userManager.FindByEmailAsync(model.Email);
                if (userExists != null)
                    return BadRequest(new { message = "This email is already in use." });

                var user = new User { UserName = model.Email, Email = model.Email, Nickname = model.Nickname, RegistrationDate = DateTime.UtcNow };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return Ok(new { message = "Registration Sucessful!" });
                }

                return BadRequest(new { message = "An unexpected error occurred, try again later!" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] {ex.Message}");
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }

        /// <summary>
        /// Autentica um utilizador e gera um token JWT para sessões autenticadas.
        /// </summary>
        /// <param name="model">Modelo contendo o e-mail e a palavra-passe do utilizador.</param>
        /// <returns>Token JWT para autenticação ou um código de erro em caso de falha.</returns>
        [HttpPost("api/signin")]
        public async Task<IActionResult> Login([FromBody] UserLoginModel model)
        {
            try
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

                var profile = await _context.Profile
                    .FirstOrDefaultAsync(p => p.UserId == user.Id);

                var roles = await _userManager.GetRolesAsync(user);
                var claims = new List<Claim>
                {
                    new Claim("Nickname", user.Nickname),
                    new Claim("UserId", user.Id),
                    new Claim("Email", user.Email),
                    new Claim("Profile", JsonConvert.SerializeObject(profile))
                };

                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                // Generate the JWT token
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(1),
                    signingCredentials: creds);

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(new
                {
                    token = tokenString,
                    expiration = token.ValidTo
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] {ex.Message}");
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }

        /// <summary>
        /// Inicia o processo de recuperação de palavra-passe, enviando um link para o e-mail do utilizador.
        /// </summary>
        /// <param name="model">Modelo contendo o e-mail do utilizador.</param>
        /// <returns>Mensagem de sucesso ou erro consoante o resultado do envio do e-mail.</returns>
        [HttpPost("api/forgot-password")]
        [EnableCors("AllowAll")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                    return BadRequest(new { message = "The email doesn't exist." });

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var encodedToken = WebUtility.UrlEncode(token);

                var resetLink = $"{_configuration["Frontend:BaseUrl"]}/reset-password?email={user.Email}&token={encodedToken}";
                Console.WriteLine($"[DEBUG] Password Reset Token: {token}");// Pode ser removido depois (DEBUG)

                await _emailService.SendEmailAsync(
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
                Console.WriteLine($"[ERROR] {ex.Message}");
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }


        /// <summary>
        /// Redefine a palavra-passe do utilizador com base num token de recuperação enviado por e-mail.
        /// </summary>
        /// <param name="model">Modelo contendo o e-mail do utilizador, token de redefinição e nova palavra-passe.</param>
        /// <returns>Mensagem de confirmação em caso de sucesso ou erro detalhado em caso de falha.</returns>
        [HttpPost("api/reset-password")]
        [EnableCors("AllowAll")]
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
                    errors = result.Errors.Select(e => e.Description) // Pode ser removido depois (DEBUG)
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] {ex.Message}");
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }

        [HttpGet("api/user-profile")]
        [Authorize]
        public IActionResult GetUserProfile()
        {
            try
            {
                // Get the claims from the token
                var nickname = User.FindFirstValue("Nickname");
                var profileJson = User.FindFirstValue("Profile");

                if (string.IsNullOrEmpty(nickname))
                {
                    return Unauthorized(new { message = "User not authenticated." });
                }

                // Deserialize the profile
                var profile = profileJson != null ? JsonConvert.DeserializeObject<Profile>(profileJson) : null;

                return Ok(new
                {
                    nickname,
                    profile
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] {ex.Message}");
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }

    }

    /// <summary>
    /// Modelo de dados para o login de utilizadores.
    /// </summary>
    public class UserLoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }


    /// <summary>
    /// Modelo de dados para o registo de novos utilizadores.
    /// </summary>
    public class UserRegistrationModel
    {
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }

    /// <summary>
    /// Modelo de dados utilizado para o pedido de recuperação de palavra-passe.
    /// </summary>
    public class ForgotPasswordModel
    {
        public string Email { get; set; }
    }

    /// <summary>
    /// Modelo de dados para redefinir a palavra-passe de um utilizador.
    /// </summary>
    public class ResetPasswordModel
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}

