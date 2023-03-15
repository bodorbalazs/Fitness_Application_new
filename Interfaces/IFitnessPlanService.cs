using Fitness_Application_new.Models;

namespace Fitness_Application_new.Interfaces
{
    public interface IFitnessPlanService
    {
        Task<FitnessPlan> GetFitnessPlanAsync(int FitnessPlanId);
        Task<IEnumerable<FitnessPlan>> GetFitnessPlansAsync();
        Task<FitnessPlan> InsertFitnessPlanAsync(FitnessPlan newFitnessPlan);
        Task UpdateFitnessPlanAsync(int FitnessPlanId, FitnessPlan updatedFitnessPlan);
        Task DeleteFitnessPlanAsync(int FitnessPlanId);
    }
}
