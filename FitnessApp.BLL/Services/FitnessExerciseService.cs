using FitnessApp.DAL.Data;
using Fitness_Application_new.Interfaces;
using FitnessApp.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Fitness_Application_new.Exceptions;

namespace Fitness_Application_new.Services
{
    public class FitnessExerciseService :IFitnessExerciseService
    {
        private readonly ApplicationDbContext _context;

        public FitnessExerciseService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task DeleteFitnessExerciseAsync(int FitnessExerciseId)
        {
            _context.fitnessExercise.Remove(new FitnessExercise { Id = FitnessExerciseId });
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if ((await _context.fitnessExercise
                    .SingleOrDefaultAsync(p => p.Id == FitnessExerciseId)) == null)
                    throw new EntityNotFoundException("Nem található az exercise");
                else throw;
            }
        }

        public async Task<FitnessExercise> GetFitnessExerciseAsync(int FitnessExerciseId)
        {
            return (await _context.fitnessExercise


               .SingleOrDefaultAsync(e => e.Id == FitnessExerciseId))
               ?? throw new EntityNotFoundException("Nem található az exercise");
        }

        public async Task<IEnumerable<FitnessExercise>> GetFitnessExercisesAsync()
        {
            var favourites = await _context.fitnessExercise
                .ToListAsync();

            return favourites;
        }

        public async Task<FitnessExercise> InsertFitnessExerciseAsync(FitnessExercise newFitnessExercise)
        {
            _context.fitnessExercise.Add(newFitnessExercise);
            await _context.SaveChangesAsync();
            return newFitnessExercise;
        }

        public async Task UpdateFitnessExerciseAsync(int FitnessExerciseId, FitnessExercise updatedFitnessExercise)
        {
            updatedFitnessExercise.Id = FitnessExerciseId;
            var entry = _context.Attach(updatedFitnessExercise);
            entry.State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if ((await _context.fitnessExercise
                        .SingleOrDefaultAsync(p => p.Id == FitnessExerciseId)) == null)
                    throw new EntityNotFoundException("Nem található az exercise");
                else throw;
            }
        }
    }
}
