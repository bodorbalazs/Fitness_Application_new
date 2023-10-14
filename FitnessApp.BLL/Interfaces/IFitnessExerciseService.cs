using FitnessApp.DAL.Models;
using Microsoft.AspNetCore.Http;

namespace Fitness_Application_new.Interfaces
{
    public interface IFitnessExerciseService
    {
        Task<FitnessExercise> GetFitnessExerciseAsync(int FitnessExerciseId);
        Task<IEnumerable<FitnessExercise>> GetFitnessExercisesAsync();
        Task<FitnessExercise> InsertFitnessExerciseAsync(FitnessExercise newFitnessExercise);
        Task UpdateFitnessExerciseAsync(int FitnessExerciseId, FitnessExercise updatedFitnessExercise);
        Task DeleteFitnessExerciseAsync(int FitnessExerciseId);

        Task DeletePlansFitnessExerciseAsync(int FitnessPlanId);

        Task InsertExercisePictureAsync(IFormFile file,string id);
    }
}
