using Duende.IdentityServer.EntityFramework.Options;
using Fitness_Application_new.Models;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;

namespace Fitness_Application_new.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options, operationalStoreOptions)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DiptervFitness;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        /*protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<FavouriteItem>().
                HasKey(x => new {x.ApplicationUserId,x.FitnessPlanId});
            builder.Entity<FavouriteItem>()
                .HasOne(x => x.FitnessPlanId);
            base.OnModelCreating(builder);

        }*/
        //disable cascading delete
        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }*/

        public DbSet<FavouriteItem> favouriteItems { get; set; }
        public DbSet<FitnessExercise> fitnessExercise { get; set; }
        public DbSet<FitnessPlan> fitnessPlan { get; set; }
        public DbSet<Rating> rating { get; set; }
    }
}