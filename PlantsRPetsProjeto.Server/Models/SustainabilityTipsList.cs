using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PlantsRPetsProjeto.Server.Models
{
    public class SustainabilityTipsList
    {
        public int SustainabilityTipsListId { get; set; }
        public int PlantInfoId { get; set; }
        public string PlantName { get; set; }
        public IList<string> PlantScientificName { get; set; }
        public IList<SustainabilityTip> SustainabilityTip { get; set; }
    }
}
