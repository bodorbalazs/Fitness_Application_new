using FitnessApp.DAL.Data;
using Fitness_Application_new.Interfaces;
using FitnessApp.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Fitness_Application_new.Exceptions;

namespace Fitness_Application_new.Services
{
    public class FitnessPlanService :IFitnessPlanService
    {
        private readonly ApplicationDbContext _context;

        public FitnessPlanService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task DeleteFitnessPlanAsync(int FitnessPlanId)
        {
            _context.fitnessPlan.Remove(new FitnessPlan { Id = FitnessPlanId });
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if ((await _context.fitnessPlan
                    .SingleOrDefaultAsync(p => p.Id == FitnessPlanId)) == null)
                    throw new EntityNotFoundException("Nem található a fitness plan");
                else throw;
            }
        }

        public async Task<FitnessPlan> GetFitnessPlanAsync(int FitnessPlanId)
        {
            return (await _context.fitnessPlan


               .SingleOrDefaultAsync(e => e.Id == FitnessPlanId))
               ?? throw new EntityNotFoundException("Nem található a fitness plan");
        }

        public async Task<IEnumerable<FitnessPlan>> GetFitnessPlansAsync()
        {
            var favourites = await _context.fitnessPlan
                .ToListAsync();

            return favourites;
        }

        public async Task<FitnessPlan> InsertFitnessPlanAsync(FitnessPlan newFitnessPlan)
        {
            _context.fitnessPlan.Add(newFitnessPlan);
            await _context.SaveChangesAsync();
            return newFitnessPlan;
        }

        public async Task UpdateFitnessPlanAsync(int FitnessPlanId, FitnessPlan updatedFitnessPlan)
        {
            updatedFitnessPlan.Id = FitnessPlanId;
            var entry = _context.Attach(updatedFitnessPlan);
            entry.State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if ((await _context.fitnessPlan
                        .SingleOrDefaultAsync(p => p.Id == FitnessPlanId)) == null)
                    throw new EntityNotFoundException("Nem található a fitness plan");
                else throw;
            }
        }
    }
}
