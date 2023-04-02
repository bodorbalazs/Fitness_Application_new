namespace FitnessApp.DAL.Models
{
    public class FavouriteItem
    {
        public int Id { get; set; }
        public string? ApplicationUserId { get; set; }
        public ApplicationUser? User { get; set; }
        public int? FitnessPlanId { get; set; }
        public FitnessPlan? FitnessPlan { get; set; }

    }
}
