namespace FitnessApp.DAL.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public string? ApplicationUserId { get; set; }
        public ApplicationUser? User { get; set; }
        public int? FitnessPlanId { get; set; }
        public FitnessPlan? FitnessPlan { get; set; }

        public int value { get; set; }

        public string? text { get; set; }
    }
}
