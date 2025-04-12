using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PlantsRPetsProjeto.Server.Models;
using System.Data.Common;
namespace PlantsRPetsProjeto.Server.Data
{
    /// <summary>
    /// Classe responsável por inserir utilizadores predefinidos na base de dados aquando da inicialização da aplicação.
    /// Garante que existem pelo menos um administrador e um utilizador comum.
    /// </summary>
    public class UserSeeder
    {
        /// <summary>
        /// Adiciona utilizadores iniciais à base de dados, caso ainda não existam.
        /// </summary>
        /// <param name="userManager">Instância do <see cref="UserManager{User}"/> utilizada para criar utilizadores e atribuir papéis.</param>
        public static async Task SeedUsersAsync(UserManager<User> userManager)
        {
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

            if (userManager.FindByEmailAsync("gega@gmail.com").Result == null)
            {
                User user = new User
                {
                    UserName = "gega@gmail.com",
                    Email = "gega@gmail.com",
                    Nickname = "gega",
                    RegistrationDate = DateTime.UtcNow,
                    EmailConfirmed = true        
                };
                var result = await userManager.CreateAsync(user, "gegassaurorex#");
            }

        }
    }
}
