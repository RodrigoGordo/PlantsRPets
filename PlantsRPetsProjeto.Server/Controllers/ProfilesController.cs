using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PlantsRPetsProjeto.Server.Data;
using PlantsRPetsProjeto.Server.Models;

namespace PlantsRPetsProjeto.Server.Controllers
{
    public class ProfilesController : Controller
    {
        private readonly PlantsRPetsProjetoServerContext _context;
        private readonly UserManager<User> _userManager;

        public ProfilesController(PlantsRPetsProjetoServerContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPut]
        [Authorize]
        [Route("api/update-profile")]
        public async Task<IActionResult> UpdateProfile([FromForm] UpdateProfileModel model)
        {
            var userId = User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User not authenticated." });
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            var profile = await _context.Profile
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (profile == null)
            {
                return NotFound(new { message = "Profile not found." });
            }

            Console.WriteLine("UpdateProfile Model: " + JsonConvert.SerializeObject(model));

            if (model.Nickname != null)
            {
                user.Nickname = model.Nickname;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }

            if (model.Bio != null)
            {
                profile.Bio = model.Bio;
            }

            if (model.ProfilePicture != null)
            {
                var filePath = await SaveProfilePicture(model.ProfilePicture);
                profile.ProfilePicture = filePath;
            }

            if (model.FavoritePets != null)
            {
                profile.FavoritePets = model.FavoritePets;
            }

            if (model.HighlightedPlantations != null)
            {
                profile.HighlightedPlantations = model.HighlightedPlantations;
            }

            _context.Profile.Update(profile);
            await _context.SaveChangesAsync();

            return Ok(profile);
        }

        private async Task<string> SaveProfilePicture(IFormFile file)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return Path.Combine("uploads", uniqueFileName);
        }

        public class UpdateProfileModel
        {
            public string? Nickname { get; set; }
            public string? Bio { get; set; }
            public IFormFile? ProfilePicture { get; set; }
            public ICollection<int>? FavoritePets { get; set; }
            public ICollection<int>? HighlightedPlantations { get; set; }
        }
    }
}
