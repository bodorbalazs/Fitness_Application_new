using Microsoft.AspNetCore.Identity;

namespace FitnessApp.DAL.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<FitnessPlan>? fitnessPlans { get; set; }
    }
}