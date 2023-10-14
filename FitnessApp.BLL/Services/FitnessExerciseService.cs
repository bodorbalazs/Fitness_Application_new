using FitnessApp.DAL.Data;
using Fitness_Application_new.Interfaces;
using FitnessApp.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Fitness_Application_new.Exceptions;
using Microsoft.AspNetCore.Http;
using Azure.Storage.Blobs;

namespace Fitness_Application_new.Services
{
    public class FitnessExerciseService : IFitnessExerciseService
    {
        private readonly ApplicationDbContext _context;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly BlobContainerClient _blobContainerClient;

        public FitnessExerciseService(ApplicationDbContext context, BlobServiceClient blobServiceClient)
        {
            _context = context;
            _blobServiceClient = blobServiceClient;
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient("defaultpicturecontainer");
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

        public async Task DeletePlansFitnessExerciseAsync(int FitnessPlanId)
        {

            var exerciselist = await _context.fitnessExercise.Where(e => e.FitnessPlanId == FitnessPlanId).ToListAsync();
            exerciselist.ForEach(async e =>
            {
                await DeleteFitnessExerciseAsync(e.Id);
            });
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

        public async Task InsertExercisePictureAsync(IFormFile file, string id)
        {
            var blobClient = _blobContainerClient.GetBlobClient(id);
            var status = await blobClient.UploadAsync(file.OpenReadStream());
            var fitnessExercise= _context.fitnessExercise.First(a => a.Id == int.Parse(id));
            fitnessExercise.PictureUrl = blobClient.Uri.AbsoluteUri;
            await _context.SaveChangesAsync();

            return;
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
