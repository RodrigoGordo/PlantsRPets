using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PlantsRPetsProjeto.Server.Models
{
    public class SustainabilityTip
    {
        public int SustainabilityTipId { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
    }

}
