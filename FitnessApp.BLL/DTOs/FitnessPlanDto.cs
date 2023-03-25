using FitnessApp.DAL.Models;

namespace Fitness_Application_new.DTOs
{
    public class FitnessPlanDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<FitnessExercise>? Exercises { get; set; }
        public string? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
    }
}
