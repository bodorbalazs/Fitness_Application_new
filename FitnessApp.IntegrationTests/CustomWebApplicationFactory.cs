using Fitness_Application_new.Interfaces;
using FitnessApp.DAL.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebMotions.Fake.Authentication.JwtBearer;

namespace FitnessApp.IntegrationTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        public Mock<IRatingService> RatingServiceMock { get; }
        public Mock<IFavouriteItemService> FavouriteItemServiceMock { get; }
        public Mock<IFitnessExerciseService> FitnessExerciseServiceMock { get; }
        public CustomWebApplicationFactory()
        {
            FavouriteItemServiceMock = new Mock<IFavouriteItemService>();
            RatingServiceMock= new Mock<IRatingService>();
            FitnessExerciseServiceMock= new Mock<IFitnessExerciseService>();
        }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
            builder.UseTestServer()
                .ConfigureTestServices(collection =>
                {
                    collection.AddAuthentication(FakeJwtBearerDefaults.AuthenticationScheme).AddFakeJwtBearer();
                });
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton(RatingServiceMock.Object);
                services.AddSingleton(FavouriteItemServiceMock.Object);
                services.AddSingleton(FitnessExerciseServiceMock.Object);
            });

        }

    }
}
