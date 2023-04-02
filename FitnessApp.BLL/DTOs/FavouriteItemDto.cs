using FitnessApp.DAL.Models;

namespace Fitness_Application_new.DTOs
{
    public class FavouriteItemDto
    {
        public int Id { get; set; }
        public string? ApplicationUserId { get; set; }
        public ApplicationUser? User { get; set; }
        public int? FitnessPlanId { get; set; }
        public FitnessPlanDto? FitnessPlan { get; set; }
    }
}
