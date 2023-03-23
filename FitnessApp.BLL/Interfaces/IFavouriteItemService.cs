using FitnessApp.DAL.Models;

namespace Fitness_Application_new.Interfaces
{
    public interface IFavouriteItemService
    {
        Task<FavouriteItem> GetFavouriteItemAsync(int FavouriteItemId);
        Task<IEnumerable<FavouriteItem>> GetFavouriteItemsAsync();
        Task<FavouriteItem> InsertFavouriteItemAsync(FavouriteItem newFavouriteItem);
        Task UpdateFavouriteItemAsync(int FavouriteItemId, FavouriteItem updatedFavouriteItem);
        Task DeleteFavouriteItemAsync(int FavouriteItemId);
    }
}
