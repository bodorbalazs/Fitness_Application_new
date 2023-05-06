using Fitness_Application_new.DTOs;
using FitnessApp.DAL.Models;

namespace Fitness_Application_new.Interfaces
{
    public interface IRatingService
    {
        Task<Rating> GetRatingAsync(int RatingId);
        Task<IEnumerable<Rating>> GetRatingsAsync();
        Task<Rating> InsertRatingAsync(Rating newRating);
        Task UpdateRatingAsync(int RatingId, Rating updatedRating);
        Task DeleteRatingAsync(int RatingId);
        Task<Rating> GetUserSpecificEventRating(int planId,string userId);
        Task<int> GetSpecificEventAverageScore(int planId);

    }
}
