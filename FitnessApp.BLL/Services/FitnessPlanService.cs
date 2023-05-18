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
        private readonly IFitnessExerciseService _exerciseService;
        private readonly IRatingService _ratingService;
        private readonly IFavouriteItemService _favouriteItemService;

        public FitnessPlanService(ApplicationDbContext context,IFitnessExerciseService exerciseService, IRatingService ratingService, IFavouriteItemService favouriteItemService)
        {
            _context = context;
            _exerciseService = exerciseService;
            _ratingService = ratingService;
            _favouriteItemService = favouriteItemService;
        }

        public async Task DeleteFitnessPlanAsync(int FitnessPlanId)
        {
            //delete associated fitness exercises
            /*var exerciselist= _context.fitnessExercise.Where(e => e.FitnessPlanId == FitnessPlanId).ToList();
            exerciselist.ForEach(async e =>
            {
                await _exerciseService.DeleteFitnessExerciseAsync(e.Id);
            });

            //delete associated ratings
            var ratingList = _context.rating.Where(e => e.FitnessPlanId == FitnessPlanId).ToList();
            ratingList.ForEach(async e =>
            {
                await _ratingService.DeleteRatingAsync(e.Id);
            });

            //delete favourites
            var favouriteList = _context.favouriteItems.Where(e => e.FitnessPlanId == FitnessPlanId).ToList();
            favouriteList.ForEach(async e =>
            {
                await _favouriteItemService.DeleteFavouriteItemAsync(e.Id);
            });*/

            //delete the fitness plan
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
        public async Task<IEnumerable<FitnessPlan>> GetUserFitnessPlansAsync(string UserId)
        {
            return (await _context.fitnessPlan
               .Where(e => e.ApplicationUserId == UserId).ToListAsync())
               ?? throw new EntityNotFoundException("Nem található ilyen fitness plan");
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
