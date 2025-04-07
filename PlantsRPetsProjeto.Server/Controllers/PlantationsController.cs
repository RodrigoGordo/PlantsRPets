using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PlantsRPetsProjeto.Server.Data;
using PlantsRPetsProjeto.Server.Models;
using PlantsRPetsProjeto.Server.Services;

namespace PlantsRPetsProjeto.Server.Controllers
{

    /// <summary>
    /// Controlador responsável pela gestão das plantações dos utilizadores.
    /// Inclui funcionalidades como criação, visualização, edição e remoção de plantações,
    /// bem como adição e gestão das plantas em cada plantação.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/plantations")]
    public class PlantationsController : ControllerBase
    {
        private readonly PlantsRPetsProjetoServerContext _context;
        private readonly PlantInfoService _plantInfoService;
        private readonly MetricsService _metricsService;

        /// <summary>
        /// Construtor do controlador de plantações.
        /// </summary>
        /// <param name="context">Contexto da base de dados da aplicação.</param>
        /// <param name="plantInfoService">Serviço responsável por popular informações sobre tipos de plantas.</param>

        public PlantationsController(PlantsRPetsProjetoServerContext context, PlantInfoService plantInfoService, MetricsService metricsService)
        {
            _context = context;
            _plantInfoService = plantInfoService;
            _metricsService = metricsService;
        }

        /// <summary>
        /// Obtém todas as plantações associadas ao utilizador autenticado.
        /// </summary>
        /// <returns>Lista de plantações com informações básicas do tipo de planta e progresso.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetUserPlantations()
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
                return NotFound(new { message = "User not found." });

            await _plantInfoService.PopulatePlantTypesAsync();

            var plantations = await _context.Plantation
                .Where(p => p.OwnerId == userId)
                .Join(
                    _context.PlantType,
                    plantation => plantation.PlantTypeId,
                    plantType => plantType.PlantTypeId,
                    (plantation, plantType) => new
                    {
                        plantation.PlantationId,
                        plantation.OwnerId,
                        plantation.PlantationName,
                        plantation.PlantTypeId,
                        plantType.PlantTypeName,
                        plantation.PlantingDate,
                        plantation.ExperiencePoints,
                        plantation.Level,
                        plantation.BankedLevelUps,
                        plantation.Location,
                        plantation.PlantationPlants
                    }
                )
                .ToListAsync();

            return Ok(plantations);
        }

        /// <summary>
        /// Obtém os detalhes de uma plantação específica pelo seu ID.
        /// </summary>
        /// <param name="id">Identificador da plantação.</param>
        /// <returns>Dados detalhados da plantação, incluindo tipo de planta e experiência.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Plantation>> GetPlantation(int id)
        {
            var plantation = await _context.Plantation
                .Where(p => p.PlantationId == id)
                .Join(
                    _context.PlantType,
                    plantation => plantation.PlantTypeId,
                    plantType => plantType.PlantTypeId,
                    (plantation, plantType) => new
                    {
                        plantation.PlantationId,
                        plantation.OwnerId,
                        plantation.PlantationName,
                        plantation.PlantTypeId,
                        plantType.PlantTypeName,
                        plantation.PlantingDate,
                        plantation.ExperiencePoints,
                        plantation.Level,
                        plantation.BankedLevelUps,
                        plantation.Location
                    }
                )
                .FirstOrDefaultAsync();

            if (plantation == null)
                return NotFound(new { message = "Plantation not found." });

            return Ok(plantation);
        }

        /// <summary>
        /// Cria uma nova plantação para o utilizador autenticado.
        /// </summary>
        /// <param name="model">Modelo com o nome da plantação e o tipo de planta a cultivar.</param>
        /// <returns>Retorna a plantação criada com o respetivo identificador.</returns>
        [HttpPost]
        public async Task<ActionResult<Plantation>> CreatePlantation([FromBody] CreatePlantationModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
                return NotFound(new { message = "User not found." });

            var plantType = await _context.PlantType.FindAsync(model.PlantTypeId);
            if (plantType == null)
                return NotFound(new { message = "Plant type not found." });

            var plantation = new Plantation
            {
                OwnerId = userId,
                PlantationName = model.PlantationName,
                PlantTypeId = model.PlantTypeId,
                PlantingDate = DateTime.UtcNow,
                ExperiencePoints = 0,
                Level = 1,
                BankedLevelUps = 0,
                Location = null,
                PlantationPlants = []
            };

            _context.Plantation.Add(plantation);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPlantation), new { id = plantation.PlantationId }, plantation);
        }

        /// <summary>
        /// Atualiza os dados de uma plantação existente pertencente ao utilizador autenticado.
        /// Permite alterar o nome, tipo de planta, experiência, nível e melhorias guardadas.
        /// </summary>
        /// <param name="id">Identificador da plantação a atualizar.</param>
        /// <param name="model">Modelo com os campos editáveis da plantação.</param>
        /// <returns>Retorna 204 (sem conteúdo) em caso de sucesso, ou mensagem de erro adequada.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlantation(int id, [FromBody] UpdatePlantationModel model)
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
                return NotFound(new { message = "User not found." });

            var existingPlantation = await _context.Plantation.FindAsync(id);
            if (existingPlantation == null)
                return NotFound(new { message = "Plantation not found." });

            if (existingPlantation.OwnerId != userId)
                return Forbid("You do not have permission to update this plantation.");

            if (model.PlantationName != null)
                existingPlantation.PlantationName = model.PlantationName;

            if (model.PlantTypeId.HasValue)
            {
                var plantType = await _context.PlantType.FindAsync(model.PlantTypeId.Value);
                if (plantType == null)
                    return NotFound(new { message = "Plant type not found." });

                existingPlantation.PlantTypeId = model.PlantTypeId.Value;
            }

            if (model.ExperiencePoints.HasValue)
                existingPlantation.ExperiencePoints = model.ExperiencePoints.Value;

            if (model.Level.HasValue)
                existingPlantation.Level = model.Level.Value;

            if (model.BankedLevelUps.HasValue)
                existingPlantation.BankedLevelUps = model.BankedLevelUps.Value;

            if (model.Location != null)
            {
                if (existingPlantation.Location == null)
                {
                    existingPlantation.Location = new Location
                    {
                        City = model.Location.City,
                        Region = model.Location.Region,
                        Country = model.Location.Country,
                        Latitude = model.Location.Latitude,
                        Longitude = model.Location.Longitude
                    };
                }
                else
                {
                    existingPlantation.Location.City = model.Location.City;
                    existingPlantation.Location.Region = model.Location.Region;
                    existingPlantation.Location.Country = model.Location.Country;
                    existingPlantation.Location.Latitude = model.Location.Latitude;
                    existingPlantation.Location.Longitude = model.Location.Longitude;
                }
            }

            _context.Entry(existingPlantation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict(new { message = "Conflict updating plantation, please try again." });
            }

            return NoContent();
        }


        /// <summary>
        /// Remove uma plantação pertencente ao utilizador autenticado.
        /// </summary>
        /// <param name="id">Identificador da plantação a eliminar.</param>
        /// <returns>Mensagem de sucesso ou erro consoante o resultado da operação.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlantation(int id)
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
                return NotFound(new { message = "User not found." });

            var plantation = await _context.Plantation.FindAsync(id);
            if (plantation == null)
                return NotFound(new { message = "Plantation not found." });

            if (plantation.OwnerId != userId)
                return Forbid("You do not have permission to delete this plantation.");

            try
            {
                _context.Plantation.Remove(plantation);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Plantation deleted successfully." });
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict(new { message = "Plantation was already removed or updated by another process." });
            }
        }

        /// <summary>
        /// Adiciona uma planta a uma plantação específica.
        /// Valida o tipo da planta e acumula a quantidade caso a planta já exista na plantação.
        /// </summary>
        /// <param name="plantationId">Identificador da plantação.</param>
        /// <param name="model">Modelo com o ID da planta e quantidade a adicionar.</param>
        /// <returns>Mensagem de sucesso ou erro consoante a validação da operação.</returns>
        [HttpPost("{plantationId}/add-plant")]
        public async Task<IActionResult> AddPlantToPlantation(int plantationId, [FromBody] AddPlantToPlantationModel model)
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
                return NotFound(new { message = "User not found." });

            var plantation = await _context.Plantation.FindAsync(plantationId);
            if (plantation == null)
                return NotFound(new { message = "Plantation not found." });

            var plant = await _context.PlantInfo.FindAsync(model.PlantInfoId);
            if (plant == null)
                return NotFound(new { message = "Plant not found." });

            var plantType = await _context.PlantType.FindAsync(plantation.PlantTypeId);
            if (plantType == null)
                return NotFound(new { message = "Plant Type not found." });

            if (plant.PlantType.ToLower() != plantType.PlantTypeName.ToLower())
                return BadRequest(new { message = "This plant type is not allowed in this plantation." });

            if (model.Quantity <= 0)
                return BadRequest(new { message = "Quantity must be greater than zero." });

            var existingPlant = await _context.PlantationPlants
                .FirstOrDefaultAsync(pp => pp.PlantationId == plantationId && pp.PlantInfoId == model.PlantInfoId);

            if (existingPlant != null)
            {
                existingPlant.Quantity += model.Quantity;
            }
            else
            {
                var plantingDate = DateTime.UtcNow;

                var firstHarvestDate = PlantingAdvisor.GetNextHarvestDate(
                    plantingDate,
                    plant.PlantType,
                    plant.GrowthRate,
                    isRecurring: false
                );

                Console.WriteLine($"HarvestDate Calculated: {firstHarvestDate}");

                var plantationPlant = new PlantationPlants
                {
                    PlantationId = plantationId,
                    PlantInfoId = model.PlantInfoId,
                    Quantity = model.Quantity,
                    GrowthStatus = "Growing",
                    PlantingDate = plantingDate,
                    LastWatered = null,
                    LastHarvested = null,
                    HarvestDate = firstHarvestDate
                };

                _context.PlantationPlants.Add(plantationPlant);
            }

            await _context.SaveChangesAsync();

            await _metricsService.RecordPlantingEventAsync(userId, plantationId, model.PlantInfoId, DateTime.UtcNow);

            return Ok(new { message = "Plant added to plantation successfully." });
        }

        /// <summary>
        /// Remove uma planta específica de uma plantação, parcial ou totalmente.
        /// </summary>
        /// <param name="plantationId">ID da plantação de onde remover a planta.</param>
        /// <param name="plantId">ID da planta a remover.</param>
        /// <param name="model">Modelo com a quantidade a remover (ou null para remover tudo).</param>
        /// <returns>Mensagem de sucesso ou erro dependendo da ação realizada.</returns>
        [HttpDelete("{plantationId}/remove-plant/{plantId}")]
        public async Task<IActionResult> RemovePlantFromPlantation(int plantationId, int plantId, [FromBody] RemovePlantFromPlantationModel model)
        {
            var plantation = await _context.Plantation.FindAsync(plantationId);
            if (plantation == null)
                return NotFound(new { message = "Plantation not found." });

            var plant = await _context.PlantInfo.FindAsync(plantId);
            if (plant == null)
                return NotFound(new { message = "Plant not found." });

            var plantationPlant = await _context.PlantationPlants
                .FirstOrDefaultAsync(pp => pp.PlantationId == plantationId && pp.PlantInfoId == plantId);

            if (plantationPlant == null)
                return NotFound(new { message = "Plant not found in this plantation." });

            if (model.Quantity.HasValue)
            {
                if (model.Quantity.Value <= 0)
                    return BadRequest(new { message = "Quantity must be greater than zero." });

                plantationPlant.Quantity -= model.Quantity.Value;

                if (plantationPlant.Quantity <= 0)
                {
                    _context.PlantationPlants.Remove(plantationPlant);
                }
            }
            else
            {
                _context.PlantationPlants.Remove(plantationPlant);
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Plant removed from plantation successfully." });
        }

        /// <summary>
        /// Obtém todas as plantas associadas a uma plantação.
        /// </summary>
        /// <param name="plantationId">Identificador da plantação.</param>
        /// <returns>Lista de objetos com informação das plantas cultivadas.</returns>
        [HttpGet("{plantationId}/plants")]
        public async Task<ActionResult<IEnumerable<PlantationPlants>>> GetPlantsInPlantation(int plantationId)
        {
            var plants = await _context.PlantationPlants
                .Where(pp => pp.PlantationId == plantationId)
                .Include(pp => pp.ReferencePlant)
                .ToListAsync();

            return Ok(plants);
        }

        /// <summary>
        /// Obtém os detalhes de uma planta específica dentro de uma plantação.
        /// </summary>
        /// <param name="plantationId">ID da plantação.</param>
        /// <param name="plantInfoId">ID da planta.</param>
        /// <returns>Informações da planta ou erro caso não exista.</returns>
        [HttpGet("{plantationId}/plant/{plantInfoId}")]
        public async Task<ActionResult<PlantationPlants>> GetPlantInPlantation(int plantationId, int plantInfoId)
        {
            var plant = await _context.PlantationPlants
                .Include(pp => pp.ReferencePlant)
                .FirstOrDefaultAsync(pp =>
                    pp.PlantationId == plantationId &&
                    pp.PlantInfoId == plantInfoId
                );

            if (plant == null) return NotFound();
            return Ok(plant);
        }

        /// <summary>
        /// Atualiza a data da última rega de uma planta específica numa plantação.
        /// </summary>
        /// <param name="plantationId">ID da plantação.</param>
        /// <param name="plantInfoId">ID da planta a ser regada.</param>
        /// <returns>Objeto atualizado com a data da última rega ou mensagem de erro.</returns>
        [HttpPost("{plantationId}/water-plant/{plantInfoId}")]
        public async Task<IActionResult> WaterPlant(int plantationId, int plantInfoId)
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
                return NotFound(new { message = "User not found." });

            var plantationPlant = await _context.PlantationPlants
                .FirstOrDefaultAsync(pp => pp.PlantationId == plantationId && pp.PlantInfoId == plantInfoId);

            if (plantationPlant == null)
                return NotFound("Plant not found in plantation");

            plantationPlant.LastWatered = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();

                await _metricsService.RecordWateringEventAsync(userId, plantationId, plantInfoId, DateTime.UtcNow);

                return Ok(plantationPlant);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Error updating watering time");
            }
        }

        /// <summary>
        /// Realiza a colheita de uma planta, com suporte para colheitas recorrentes ou únicas.
        /// </summary>
        /// <param name="plantationId">ID da plantação.</param>
        /// <param name="plantInfoId">ID da planta a ser colhida.</param>
        /// <returns>Mensagem de sucesso ou erro, incluindo próxima data de colheita, se aplicável.</returns>
        [HttpPost("{plantationId}/harvest-plant/{plantInfoId}")]
        public async Task<IActionResult> HarvestPlant(int plantationId, int plantInfoId)
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
                return NotFound(new { message = "User not found." });

            var plantationPlant = await _context.PlantationPlants
                .Include(pp => pp.ReferencePlant)
                .FirstOrDefaultAsync(pp => pp.PlantationId == plantationId && pp.PlantInfoId == plantInfoId);

            if (plantationPlant == null)
                return NotFound("Plant not found in plantation");

            var plantType = await _context.PlantType
                .FirstOrDefaultAsync(pt => pt.PlantTypeName.ToLower() == plantationPlant.ReferencePlant.PlantType.ToLower());

            if (plantType == null)
                return NotFound("Plant type not found");

            if (plantationPlant.HarvestDate == null)
                return BadRequest("Harvest date not set for this plant.");

            var now = DateTime.UtcNow;
            if (now < plantationPlant.HarvestDate.Value)
            {
                var timeRemaining = plantationPlant.HarvestDate.Value - now;
                return BadRequest(new
                {
                    message = "Plant is not ready for harvest",
                    timeRemainingDays = (int)Math.Ceiling(timeRemaining.TotalDays)
                });
            }

            if (plantType.HasRecurringHarvest)
            {
                plantationPlant.LastHarvested = now;
                plantationPlant.HarvestDate = PlantingAdvisor.GetNextHarvestDate(
                    plantationPlant.PlantingDate,
                    plantationPlant.ReferencePlant.PlantType,
                    plantationPlant.ReferencePlant.GrowthRate,
                    isRecurring: true,
                    lastHarvestDate: now
                );
                plantationPlant.GrowthStatus = "Growing";
                await _context.SaveChangesAsync();

                await _metricsService.RecordHarvestEventAsync(userId, plantationId, plantInfoId, DateTime.UtcNow);

                return Ok(new
                {
                    message = "Plant harvested successfully. Next harvest in progress.",
                    nextHarvestDate = plantationPlant.HarvestDate,
                    lastHarvested = plantationPlant.LastHarvested
                });
            }
            else
            {
                _context.PlantationPlants.Remove(plantationPlant);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Plant harvested and removed (non-recurring)." });
            }
        }

        /// <summary>
        /// Verifica se uma planta está pronta para ser colhida.
        /// </summary>
        /// <param name="plantationId">ID da plantação.</param>
        /// <param name="plantInfoId">ID da planta a verificar.</param>
        /// <returns>Estado da colheita, dias restantes e data prevista da próxima colheita.</returns>
        [HttpGet("{plantationId}/plant/{plantInfoId}/check-harvest")]
        public async Task<IActionResult> CheckHarvest(int plantationId, int plantInfoId)
        {
            var plantationPlant = await _context.PlantationPlants
                .Include(pp => pp.ReferencePlant)
                .FirstOrDefaultAsync(pp => pp.PlantationId == plantationId && pp.PlantInfoId == plantInfoId);

            if (plantationPlant == null)
                return NotFound("Plant not found in plantation");

            var plantType = await _context.PlantType
                .FirstOrDefaultAsync(pt => pt.PlantTypeName.ToLower() == plantationPlant.ReferencePlant.PlantType.ToLower());

            if (plantType == null)
                return NotFound("Plant type not found");

            var currentHarvestDate = plantationPlant.HarvestDate;

            if (currentHarvestDate == null)
                return BadRequest("Harvest date not set for this plant.");

            var now = DateTime.UtcNow;
            bool canHarvest = now >= currentHarvestDate;
            var timeRemaining = currentHarvestDate.Value - now;

            return Ok(new
            {
                canHarvest,
                timeRemainingDays = canHarvest ? 0 : (int)Math.Ceiling(timeRemaining.TotalDays),
                nextHarvestDate = currentHarvestDate
            });
        }

        /// <summary>
        /// Utiliza uma melhoria (level-up) acumulada da plantação.
        /// Reduz o número de melhorias disponíveis e atualiza a plantação.
        /// </summary>
        /// <param name="id">ID da plantação.</param>
        /// <returns>Estado atualizado da plantação ou mensagem de erro.</returns>
        [HttpPost("{id}/use-banked-levelup")]
        public async Task<IActionResult> UseBankedLevelUp(int id)
        {
            var plantation = await _context.Plantation.FindAsync(id);
            if (plantation == null)
                return NotFound(new { message = "Plantation not found." });

            plantation.BankedLevelUps -= 1;
            await _context.SaveChangesAsync();

            return Ok(plantation);
        }

        /// <summary>
        /// Atualiza manualmente a data de colheita de uma planta específica numa plantação.
        /// Pode ser usado para ajustes administrativos ou eventos especiais.
        /// </summary>
        /// <param name="plantationId">ID da plantação.</param>
        /// <param name="plantInfoId">ID da planta a atualizar.</param>
        /// <param name="newHarvestDate">Nova data de colheita a definir.</param>
        /// <returns>Mensagem de confirmação com a nova data de colheita.</returns>
        [HttpPost("{plantationId}/plant/{plantInfoId}/set-harvest-date")]
        public async Task<IActionResult> SetHarvestDate(int plantationId, int plantInfoId, [FromBody] DateTime newHarvestDate)
        {
            var plantationPlant = await _context.PlantationPlants
                .FirstOrDefaultAsync(pp => pp.PlantationId == plantationId && pp.PlantInfoId == plantInfoId);

            if (plantationPlant == null)
                return NotFound("Plant not found in plantation");

            plantationPlant.HarvestDate = newHarvestDate;
            await _context.SaveChangesAsync();

            return Ok(new { message = "HarvestDate updated", newHarvestDate });
        }

        /// <summary>
        /// Atribui pontos de experiência à plantação com base em ações realizadas (rega ou colheita).
        /// Se a experiência acumulada ultrapassar o limiar, são atribuídos níveis automaticamente.
        /// </summary>
        /// <param name="id">ID da plantação.</param>
        /// <param name="plantInfoId">ID da planta relacionada à ação.</param>
        /// <param name="isHarvesting">Define se a ação é colheita (true) ou rega (false).</param>
        /// <returns>204 se a atualização for bem-sucedida, ou erro correspondente.</returns>
        [HttpPut("{id}/gain-xp/{plantInfoId}")]
        public async Task<IActionResult> GainExperience(int id, int plantInfoId, bool isHarvesting)
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
                return NotFound(new { message = "User not found." });

            var existingPlantation = await _context.Plantation.FindAsync(id);
            if (existingPlantation == null)
                return NotFound(new { message = "Plantation not found." });

            var plantationPlant = await _context.PlantationPlants
                .Include(pp => pp.ReferencePlant)
                .FirstOrDefaultAsync(pp => pp.PlantationId == id && pp.PlantInfoId == plantInfoId);

            int experienceAmount = 0;

            string plantationPlantTypeString = plantationPlant!.ReferencePlant.PlantType;
            string plantationPlantGrowthRate = plantationPlant!.ReferencePlant.GrowthRate;
            string plantationPlantWateringFrequency = plantationPlant!.ReferencePlant.Watering;
            int plantationPlantQuantity = plantationPlant!.Quantity;

            DateTime? plantationPlantLastHarvest = plantationPlant!.LastHarvested;

            if (isHarvesting)
            {
                experienceAmount = LevelUpService.GetHarvestExperienceAmount(plantationPlantTypeString, plantationPlantGrowthRate, true, plantationPlantLastHarvest);
            } else
            {
                experienceAmount = LevelUpService.GetWateringExperience(plantationPlantTypeString, plantationPlantWateringFrequency);
            }

            existingPlantation.ExperiencePoints += experienceAmount * plantationPlantQuantity;

            int plantationExperienceThreshold = 500;

            if (existingPlantation.ExperiencePoints >= plantationExperienceThreshold)
            {
                int leftoverExperience = existingPlantation.ExperiencePoints;

                int numberOfLevels = 0;
                while (leftoverExperience >= plantationExperienceThreshold)
                {
                    numberOfLevels++;
                    leftoverExperience -= plantationExperienceThreshold;
                }

                existingPlantation.Level += numberOfLevels;
                existingPlantation.ExperiencePoints = leftoverExperience;
                existingPlantation.BankedLevelUps = numberOfLevels;
            }

            _context.Entry(existingPlantation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(existingPlantation);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict(new { message = "Conflict updating plantation when increasing experience, please try again." });
            }

        }
    }

    /// <summary>
    /// Modelo utilizado na criação de uma nova plantação.
    /// </summary>
    public class CreatePlantationModel
    {
        public required string PlantationName { get; set; }
        public required int PlantTypeId { get; set; }
    }

    /// <summary>
    /// Modelo utilizado para atualizar os dados de uma plantação existente.
    /// Todos os campos são opcionais e aplicados apenas se fornecidos.
    /// </summary>
    public class UpdatePlantationModel
    {
        public string? PlantationName { get; set; }
        public DateTime? LastWatered { get; set; }
        public DateTime? HarvestDate { get; set; }
        public int? PlantTypeId { get; set; }
        public string? GrowthStatus { get; set; }
        public int? ExperiencePoints { get; set; }
        public int? Level { get; set; }
        public int? BankedLevelUps {  get; set; }
        public Location? Location { get; set; }
    }

    /// <summary>
    /// Modelo utilizado para adicionar uma planta a uma plantação.
    /// </summary>
    public class AddPlantToPlantationModel
    {
        public int PlantInfoId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }
    }

    /// <summary>
    /// Modelo utilizado para remover uma quantidade de uma planta de uma plantação.
    /// Se a quantidade não for fornecida, a planta é totalmente removida.
    /// </summary>
    public class RemovePlantFromPlantationModel
    {
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int? Quantity { get; set; }
    }
}