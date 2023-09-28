using Fitness_Application_new.DTOs;
using FitnessApp.DAL.Models;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Security.Claims;

namespace FitnessApp.IntegrationTests
{
    public class FitnessExerciseIntegrationTest :IDisposable
    {
        private CustomWebApplicationFactory _factory;
        private HttpClient _client;

        public FitnessExerciseIntegrationTest()
        {
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();

        }

        [Fact]
        public async Task GetAsync_ReturnsListOfFitnessExercise()
        {
            //// Arrange
            var fitnessExerciseList = new List<FitnessExercise>();
            fitnessExerciseList.Add(new FitnessExercise
            {
                Id = 10,
            });

            Task<IEnumerable<FitnessExercise>> mockFitnessExercises = Task.FromResult<IEnumerable<FitnessExercise>>(fitnessExerciseList);

            _factory.FitnessExerciseServiceMock
                .Setup(r => r.GetFitnessExercisesAsync())
                .Returns(mockFitnessExercises);

            //// Act
            var response = await _client.GetAsync("https://localhost:7252/api/FitnessExercise");

            //// Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var unpackData = JsonConvert.DeserializeObject<IEnumerable<FitnessExerciseDto>>(await response.Content.ReadAsStringAsync());
            Assert.Collection(unpackData, r =>
            {
                Assert.Equal(10, r.Id);
            });
        }

        [Fact]
        public async Task Get_ReturnsFitnessExercise()
        {
            // Arrange
            var exercise = new FitnessExercise
            {
                Id = 10,
            };

            Task<FitnessExercise> mockFitnessExercises = Task.FromResult<FitnessExercise>(exercise);

            _factory.FitnessExerciseServiceMock
                .Setup(r => r.GetFitnessExerciseAsync(10))
                .Returns(mockFitnessExercises);

            //// Act
            var response = await _client.GetAsync("https://localhost:7252/api/FitnessExercise/10");

            //// Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var unpackData = JsonConvert.DeserializeObject<FitnessExerciseDto>(await response.Content.ReadAsStringAsync());
            Assert.Equal(10, unpackData!.Id);
        }

        [Fact]
        public async Task Post_ReturnsOkWithCreatedId()
        {
            //// Arrange
            var exercise = new FitnessExercise
            {
                Id = 10,
            };
            Task<FitnessExercise> mockFitnessExercise = Task.FromResult<FitnessExercise>(exercise);
            var claims = new Dictionary<string, object>
            {
                { ClaimTypes.Name, "test@test.com" },
                { ClaimTypes.Role, "user" },
                { ClaimTypes.NameIdentifier, "testId" }
            };
            _client.SetFakeBearerToken(claims);


            _factory.FitnessExerciseServiceMock
                .Setup(r => r.InsertFitnessExerciseAsync(It.Is<FitnessExercise>(e => e.Id == 10)))
                .Returns(mockFitnessExercise)
                ;

            //// Act
            var response = await _client.PostAsync("https://localhost:7252/api/FitnessExercise", JsonContent.Create(exercise));

            //// Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent()
        {
            // Arrange
            var itemId = 10;

            _factory.FitnessExerciseServiceMock.Setup(service => service.DeleteFitnessExerciseAsync(itemId)).Returns(Task.CompletedTask);

            // Act
            var response = await _client.DeleteAsync("https://localhost:7252/api/FitnessExercise/10");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task Put_ReturnsNoContentResult()
        {
            // Arrange
            var Originalexercise = new FitnessExercise
            {
                Id = 10,
                FitnessPlanId = 10,
            };

            var Changedexercise = new FitnessExercise
            {
                Id = 10,
                FitnessPlanId = 15,

            };
            var claims = new Dictionary<string, object>
            {
                { ClaimTypes.Name, "test@test.com" },
                { ClaimTypes.Role, "user" },
                { ClaimTypes.NameIdentifier, "testId" }
            };
            _client.SetFakeBearerToken(claims);

            // Act
            var response = await _client.PutAsync("https://localhost:7252/api/FitnessExercise/10", JsonContent.Create(Changedexercise));

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
        public void Dispose()
        {
            _client.Dispose();
            _factory.Dispose();
        }
    }
}
