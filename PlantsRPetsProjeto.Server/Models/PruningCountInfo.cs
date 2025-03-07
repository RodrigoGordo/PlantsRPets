using System.ComponentModel.DataAnnotations;

namespace PlantsRPetsProjeto.Server.Models
{
    public class PruningCountInfo
    {
        public int PruningCountInfoId { get; set; }
        public int Amount { get; set; }
        public string Interval { get; set; }
    }
}
