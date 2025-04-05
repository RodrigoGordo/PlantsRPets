using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PlantsRPetsProjeto.Server.Data;
using PlantsRPetsProjeto.Server.Models;


namespace PlantsRPetsProjeto.Server.Data
{
    /// <summary>
    /// Seeder responsável por inserir notificações predefinidas na base de dados.
    /// Estas notificações são utilizadas pela aplicação para interagir com os utilizadores.
    /// </summary>
    public class NotificationSeeder
    {
        /// <summary>
        /// Insere notificações base no sistema, caso ainda não existam.
        /// </summary>
        /// <param name="_context">Contexto da base de dados da aplicação.</param>
        public static async Task SeedAsync(PlantsRPetsProjetoServerContext _context)
        {
            if (await _context.Notification.AnyAsync())
            {
                return;
            }

            var notifications = new[]
            {
                new Notification { Type = "Water Reminder", Message = "Don't forget to water your plants today!" },
                new Notification { Type = "Harvest Alert", Message = "Your crops are ready to be harvested!" },
                new Notification { Type = "Level Up", Message = "Congratulations! You've leveled up!" },
                new Notification { Type = "New Reward", Message = "You earned a new pet for your collection!" },
            };

            await _context.Notification.AddRangeAsync(notifications);
            await _context.SaveChangesAsync();
        }
    }
}
