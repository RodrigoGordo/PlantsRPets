using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PlantsRPetsProjeto.Server.Models;
using System.Data.Common;
namespace PlantsRPetsProjeto.Server.Data
{
    public class UserSeeder
    {
        public static async Task SeedUsersAsync(UserManager<User> userManager)
        {
            //Criar administrador
            if (userManager.FindByEmailAsync("plantsrpets@outlook.com").Result == null)
            {
                User user = new User
                {
                    UserName = "plantsrpets@outlook.com",
                    Email = "plantsrpets@outlook.com",
                    Nickname = "prpadmin",
                    RegistrationDate = DateTime.UtcNow
                };

                await userManager.CreateAsync(user, "PrP#2025");
                await userManager.AddToRoleAsync(user, "Admin");
                user.EmailConfirmed = true;
            }

            //Criar utilizador
            if (userManager.FindByEmailAsync("gega@gmail.com").Result == null)
            {
                User user = new User
                {
                    UserName = "gega@gmail.com",
                    Email = "gega@gmail.com",
                    Nickname = "gega",
                    RegistrationDate = DateTime.UtcNow
                };
                var result = await userManager.CreateAsync(user, "gegassaurorex#");
            }

        }
    }
}
