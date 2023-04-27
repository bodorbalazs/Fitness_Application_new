using FitnessApp.DAL.Data;
using Fitness_Application_new.Interfaces;
using FitnessApp.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Fitness_Application_new.Exceptions;
using System.Reflection.Metadata.Ecma335;

namespace Fitness_Application_new.Services
{
    public class RatingService : IRatingService
    {
        private readonly ApplicationDbContext _context;

        public RatingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task DeleteRatingAsync(int RatingId)
        {
            _context.rating.Remove(new Rating { Id = RatingId });
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if ((await _context.rating
                    .SingleOrDefaultAsync(p => p.Id == RatingId)) == null)
                    throw new EntityNotFoundException("Nem található a rating");
                else throw;
            }
        }

        public async Task<Rating> GetRatingAsync(int RatingId)
        {
            return (await _context.rating


               .SingleOrDefaultAsync(e => e.Id == RatingId))
               ?? throw new EntityNotFoundException("Nem található a rating");
        }

        public async Task<Rating> GetUserSpecificEventRating(int planId,string userId)
        {
            return (await _context.rating
                .OrderBy(e=>e.Id)
               .LastOrDefaultAsync(e => e.FitnessPlanId == planId && e.ApplicationUserId == userId))
               ??  new Rating();
        }

        public async Task<IEnumerable<Rating>> GetRatingsAsync()
        {
            var favourites = await _context.rating
                .ToListAsync();

            return favourites;
        }

        public async Task<Rating> InsertRatingAsync(Rating newRating)
        {
            _context.rating.Add(newRating);
            await _context.SaveChangesAsync();
            return newRating;
        }

        public async Task UpdateRatingAsync(int RatingId, Rating updatedRating)
        {
            updatedRating.Id = RatingId;
            var entry = _context.Attach(updatedRating);
            entry.State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if ((await _context.rating
                        .SingleOrDefaultAsync(p => p.Id == RatingId)) == null)
                    throw new EntityNotFoundException("Nem található a rating");
                else throw;
            }
        }
    }
}
