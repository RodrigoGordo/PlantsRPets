using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlantsRPetsProjeto.Server.Models
{
    /// <summary>
    /// Representa uma planta com todas as suas características relevantes,
    /// recolhidas a partir de fontes externas (ex: Perenual API) ou introduzidas pelo utilizador.
    /// </summary>
    public class PlantInfo
    {
        /// <summary>
        /// Identificador único da planta.
        /// </summary>
        public int PlantInfoId { get; set; }

        /// <summary>
        /// Nome comum da planta.
        /// </summary>
        public string PlantName { get; set; }

        /// <summary>
        /// Tipo da planta (ex: "árvore", "flor", "vegetal").
        /// </summary>
        public string PlantType { get; set; }

        /// <summary>
        /// Ciclo de vida da planta (ex: "anual").
        /// </summary>
        public string Cycle { get; set; }

        /// <summary>
        /// Frequência de rega recomendada (ex: "frequente", "média", "mínima").
        /// </summary>
        public string Watering { get; set; }

        /// <summary>
        /// Meses recomendados para poda da planta (em texto, ex: "Janeiro", "Março").
        /// </summary>
        public List<string> PruningMonth { get; set; }

        /// <summary>
        /// Informação adicional sobre a quantidade e intervalo de podas.
        /// </summary>
        public PruningCountInfo? PruningCount { get; set; }

        /// <summary>
        /// Taxa de crescimento da planta (ex: "Alta", "Média", "Baixa").
        /// </summary>
        public string GrowthRate { get; set; }

        /// <summary>
        /// Níveis de exposição solar ideais para a planta (ex: "Sol pleno", "Meia-sombra").
        /// </summary>
        public List<string> Sunlight { get; set; }

        /// <summary>
        /// Indica se a planta tem partes comestíveis ("Yes" ou "No").
        /// </summary>
        public string Edible { get; set; }

        /// <summary>
        /// Nível de dificuldade de cuidado (ex: "fácil", "médio", "difícil").
        /// </summary>
        public string CareLevel { get; set; }

        /// <summary>
        /// Indica se a planta produz flores ("Yes" ou "No").
        /// </summary>
        public string Flowers { get; set; }

        /// <summary>
        /// Indica se a planta produz frutos ("Yes" ou "No").
        /// </summary>
        public string Fruits { get; set; }

        /// <summary>
        /// Indica se a planta possui folhas visíveis.
        /// </summary>
        public bool Leaf { get; set; }

        /// <summary>
        /// Grau de manutenção exigido pela planta (ex: "baixa", "média", "alta").
        /// </summary>
        public string Maintenance { get; set; }

        /// <summary>
        /// Indica se a planta tolera solos com salinidade elevada ("Yes" ou "No").
        /// </summary>
        public string SaltTolerant { get; set; }

        /// <summary>
        /// Indica se a planta pode ser mantida em ambientes interiores.
        /// </summary>
        public bool Indoor { get; set; }

        /// <summary>
        /// Estação ou meses em que a planta floresce (ex: "Primavera", "Março a Maio").
        /// </summary>
        public string FloweringSeason { get; set; }

        /// <summary>
        /// Descrição geral da planta, incluindo curiosidades ou conselhos de cultivo.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// URL da imagem representativa da planta.
        /// </summary>
        public string? Image { get; set; }

        /// <summary>
        /// Estação ou meses recomendados para colheita dos frutos ou partes comestíveis.
        /// </summary>
        public string HarvestSeason { get; set; }

        /// <summary>
        /// Lista de nomes científicos alternativos da planta.
        /// </summary>
        public List<string> ScientificName { get; set; }

        /// <summary>
        /// Indica se a planta é tolerante à seca.
        /// </summary>
        public bool DroughtTolerant { get; set; }

        /// <summary>
        /// Indica se a planta pode ser usada na culinária.
        /// </summary>
        public bool Cuisine { get; set; }

        /// <summary>
        /// Indica se a planta possui propriedades medicinais.
        /// </summary>
        public bool Medicinal { get; set; }
    }

}