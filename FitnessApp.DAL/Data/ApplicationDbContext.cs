using Duende.IdentityServer.EntityFramework.Options;
using FitnessApp.DAL.Models;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;

namespace FitnessApp.DAL.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options, operationalStoreOptions)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rating>().HasData(new Rating { Id = 1, value = 0 });
            base.OnModelCreating(modelBuilder);

            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        public DbSet<FavouriteItem> favouriteItems { get; set; }
        public DbSet<FitnessExercise> fitnessExercise { get; set; }
        public DbSet<FitnessPlan> fitnessPlan { get; set; }
        public DbSet<Rating> rating { get; set; }
    }
}