namespace FitnessApp.DAL.Models
{
    public class FitnessExercise
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? PictureUrl { get; set; }
        public string? Difficulty { get; set; }
        public int? FitnessPlanId { get; set; }
        public FitnessPlan? FitnessPlan { get; set; } 
    }
}
