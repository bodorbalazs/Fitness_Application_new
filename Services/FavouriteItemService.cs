using Fitness_Application_new.Data;
using Fitness_Application_new.Interfaces;
using Fitness_Application_new.Models;
using Microsoft.EntityFrameworkCore;
using Project_laboratory_2.Exceptions;

namespace Fitness_Application_new.Services
{
    public class FavouriteItemService :IFavouriteItemService
    {
        private readonly ApplicationDbContext _context;

        public FavouriteItemService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task DeleteFavouriteItemAsync(int FavouriteItemId)
        {
            _context.favouriteItems.Remove(new FavouriteItem { Id = FavouriteItemId });
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if ((await _context.favouriteItems
                    .SingleOrDefaultAsync(p => p.Id == FavouriteItemId)) == null)
                    throw new EntityNotFoundException("Nem található a favourite");
                else throw;
            }
        }

        public async Task<FavouriteItem> GetFavouriteItemAsync(int FavouriteItemId)
        {
            return (await _context.favouriteItems


               .SingleOrDefaultAsync(e => e.Id == FavouriteItemId))
               ?? throw new EntityNotFoundException("Nem található a favourite");
        }

        public async Task<IEnumerable<FavouriteItem>> GetFavouriteItemsAsync()
        {
            var favourites = await _context.favouriteItems
                .ToListAsync();

            return favourites;
        }

        public async Task<FavouriteItem> InsertFavouriteItemAsync(FavouriteItem newFavouriteItem)
        {
            _context.favouriteItems.Add(newFavouriteItem);
            await _context.SaveChangesAsync();
            return newFavouriteItem;
        }

        public async Task UpdateFavouriteItemAsync(int FavouriteItemId, FavouriteItem updatedFavouriteItem)
        {
            updatedFavouriteItem.Id = FavouriteItemId;
            var entry = _context.Attach(updatedFavouriteItem);
            entry.State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if ((await _context.favouriteItems
                        .SingleOrDefaultAsync(p => p.Id == FavouriteItemId)) == null)
                    throw new EntityNotFoundException("Nem található a favourite");
                else throw;
            }
        }
    }
}
