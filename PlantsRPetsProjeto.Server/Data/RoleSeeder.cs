using Microsoft.AspNetCore.Identity;

namespace PlantsRPetsProjeto.Server.Data
{
    /// <summary>
    /// Seeder responsável pela criação de papéis (roles) iniciais no sistema de autenticação e autorização.
    /// Garante que os papéis essenciais estão disponíveis na base de dados.
    /// </summary>
    public class RoleSeeder
    {
        /// <summary>
        /// Cria os papéis predefinidos, caso ainda não existam na base de dados.
        /// </summary>
        /// <param name="roleManager">Instância do <see cref="RoleManager{IdentityRole}"/> utilizada para gerir papéis no sistema.</param>
        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Admin" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
    }
}
