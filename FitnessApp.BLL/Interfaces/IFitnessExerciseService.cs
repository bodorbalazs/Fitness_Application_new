using FitnessApp.DAL.Models;

namespace Fitness_Application_new.Interfaces
{
    public interface IFitnessExerciseService
    {
        Task<FitnessExercise> GetFitnessExerciseAsync(int FitnessExerciseId);
        Task<IEnumerable<FitnessExercise>> GetFitnessExercisesAsync();
        Task<FitnessExercise> InsertFitnessExerciseAsync(FitnessExercise newFitnessExercise);
        Task UpdateFitnessExerciseAsync(int FitnessExerciseId, FitnessExercise updatedFitnessExercise);
        Task DeleteFitnessExerciseAsync(int FitnessExerciseId);
    }
}
