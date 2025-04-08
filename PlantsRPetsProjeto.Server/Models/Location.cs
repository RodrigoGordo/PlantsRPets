using Microsoft.Identity.Client;

namespace PlantsRPetsProjeto.Server.Models
{
    /// <summary>
    /// Representa uma localização geográfica, incluindo cidade, região, país e coordenadas.
    /// Esta classe pode ser usada para associar dados climáticos, plantações, ou utilizadores a uma localização específica.
    /// </summary>
    public class Location
    {
        /// <summary>
        /// Identificador único da localização (chave primária).
        /// </summary>
        public int LocationId { get; set; }

        /// <summary>
        /// Nome da cidade.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Região ou estado onde se encontra a cidade.
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// País ao qual pertence a cidade/região.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Latitude geográfica da localização.
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Longitude geográfica da localização.
        /// </summary>
        public double Longitude { get; set; }
    }
}
