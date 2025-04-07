using Microsoft.Identity.Client;

namespace PlantsRPetsProjeto.Server.Models
{
    public class Location
    {
        public int LocationId { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
