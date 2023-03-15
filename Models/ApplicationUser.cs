using Microsoft.AspNetCore.Identity;

namespace Fitness_Application_new.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<FitnessPlan> fitnessPlans { get; set; }
    }
}