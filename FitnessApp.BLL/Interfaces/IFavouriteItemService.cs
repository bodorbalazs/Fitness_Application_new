using FitnessApp.DAL.Models;

namespace Fitness_Application_new.Interfaces
{
    public interface IFavouriteItemService
    {
        Task<FavouriteItem> GetFavouriteItemAsync(int FavouriteItemId);
        Task<IEnumerable<FavouriteItem>> GetFavouriteItemsAsync();
        Task<IEnumerable<FavouriteItem>> GetUsersFavouriteItemsAsync(string userId);
        Task<FavouriteItem> InsertFavouriteItemAsync(FavouriteItem newFavouriteItem);
        Task UpdateFavouriteItemAsync(int FavouriteItemId, FavouriteItem updatedFavouriteItem);
        Task DeleteFavouriteItemAsync(int FavouriteItemId);

        Task DeletePlansFavouriteItemAsync(int PlanId);
        Task<FavouriteItem> GetPlanUsersFavouriteItemAsync(string userId, int FitnessplanId);
    }
}
