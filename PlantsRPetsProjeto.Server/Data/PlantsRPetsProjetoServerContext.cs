using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PlantsRPetsProjeto.Server.Models;

namespace PlantsRPetsProjeto.Server.Data
{
    public class PlantsRPetsProjetoServerContext : IdentityDbContext<User>
    {
        public PlantsRPetsProjetoServerContext (DbContextOptions<PlantsRPetsProjetoServerContext> options)
            : base(options)
        {
        }

        public DbSet<PlantsRPetsProjeto.Server.Models.Chat> Chat { get; set; } = default!;
        public DbSet<PlantsRPetsProjeto.Server.Models.Collection> Collection { get; set; } = default!;
        public DbSet<PlantsRPetsProjeto.Server.Models.Community> Community { get; set; } = default!;
        public DbSet<PlantsRPetsProjeto.Server.Models.Dashboard> Dashboard { get; set; } = default!;
        public DbSet<PlantsRPetsProjeto.Server.Models.Message> Message { get; set; } = default!;
        public DbSet<PlantsRPetsProjeto.Server.Models.Metric> Metric { get; set; } = default!;
        public DbSet<PlantsRPetsProjeto.Server.Models.Notification> Notification { get; set; } = default!;
        public DbSet<PlantsRPetsProjeto.Server.Models.Pet> Pet { get; set; } = default!;
        public DbSet<PlantsRPetsProjeto.Server.Models.Plantation> Plantation { get; set; } = default!;
        public DbSet<PlantsRPetsProjeto.Server.Models.Profile> Profile { get; set; } = default!;
        public DbSet<PlantsRPetsProjeto.Server.Models.Settings> Settings { get; set; } = default!;
        public DbSet<PlantsRPetsProjeto.Server.Models.SustainabilityTip> SustainabilityTip { get; set; } = default!;
        public DbSet<PlantsRPetsProjeto.Server.Models.PlantationPlants> PlantationPlants { get; set; } = default!;
        public DbSet<PlantsRPetsProjeto.Server.Models.PlantInfo> PlantInfo { get; set; } = default!;
        public DbSet<PlantsRPetsProjeto.Server.Models.PlantType> PlantType { get; set; } = default!;
        public DbSet<PlantsRPetsProjeto.Server.Models.SustainabilityTipsList> SustainabilityTipsList { get; set; } = default!;
    }
}
