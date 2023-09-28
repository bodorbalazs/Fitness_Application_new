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
    public class FitnessPlanIntegrationTest :IDisposable
    {
        private CustomWebApplicationFactory _factory;
        private HttpClient _client;

        public FitnessPlanIntegrationTest()
        {
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();

        }
        [Fact]
        public async Task GetAsync_ReturnsListOfFitnessPlan()
        {
            //// Arrange
            var fitnessPlanList = new List<FitnessPlan>();
            fitnessPlanList.Add(new FitnessPlan
            {
                Id = 10,
            });

            Task<IEnumerable<FitnessPlan>> mockFitnessPlans = Task.FromResult<IEnumerable<FitnessPlan>>(fitnessPlanList);

            _factory.FitnessPlanServiceMock
                .Setup(r => r.GetFitnessPlansAsync())
                .Returns(mockFitnessPlans);

            //// Act
            var response = await _client.GetAsync("https://localhost:7252/api/FitnessPlan");

            //// Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var unpackData = JsonConvert.DeserializeObject<IEnumerable<FitnessPlanDto>>(await response.Content.ReadAsStringAsync());
            Assert.Collection(unpackData, r =>
            {
                Assert.Equal(10, r.Id);
            });
        }

        [Fact]
        public async Task Get_ReturnsFitnessPlan()
        {
            // Arrange
            var plan = new FitnessPlan
            {
                Id = 10,
            };

            Task<FitnessPlan> mockFitnessPlans = Task.FromResult<FitnessPlan>(plan);

            _factory.FitnessPlanServiceMock
                .Setup(r => r.GetFitnessPlanAsync(10))
                .Returns(mockFitnessPlans);

            //// Act
            var response = await _client.GetAsync("https://localhost:7252/api/FitnessPlan/10");

            //// Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var unpackData = JsonConvert.DeserializeObject<FitnessPlanDto>(await response.Content.ReadAsStringAsync());
            Assert.Equal(10, unpackData!.Id);
        }

        [Fact]
        public async Task Post_ReturnsOkWithCreatedId()
        {
            //// Arrange
            var plan = new FitnessPlan
            {
                Id = 10,
            };
            Task<FitnessPlan> mockFitnessPlan = Task.FromResult<FitnessPlan>(plan);
            var claims = new Dictionary<string, object>
            {
                { ClaimTypes.Name, "test@test.com" },
                { ClaimTypes.Role, "user" },
                { ClaimTypes.NameIdentifier, "testId" }
            };
            _client.SetFakeBearerToken(claims);


            _factory.FitnessPlanServiceMock
                .Setup(r => r.InsertFitnessPlanAsync(It.Is<FitnessPlan>(e => e.Id == 10)))
                .Returns(mockFitnessPlan)
                ;

            //// Act
            var response = await _client.PostAsync("https://localhost:7252/api/FitnessPlan", JsonContent.Create(plan));

            //// Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent()
        {
            // Arrange
            var itemId = 10;

            _factory.FitnessPlanServiceMock.Setup(service => service.DeleteFitnessPlanAsync(itemId)).Returns(Task.CompletedTask);

            // Act
            var response = await _client.DeleteAsync("https://localhost:7252/api/FitnessPlan/10");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task Put_ReturnsNoContentResult()
        {
            // Arrange
            var Originalplan = new FitnessPlan
            {
                Id = 10,
                Name = "test1",
            };

            var Changedplan = new FitnessPlan
            {
                Id = 10,
                Name = "test2",

            };
            var claims = new Dictionary<string, object>
            {
                { ClaimTypes.Name, "test@test.com" },
                { ClaimTypes.Role, "user" },
                { ClaimTypes.NameIdentifier, "testId" }
            };
            _client.SetFakeBearerToken(claims);

            // Act
            var response = await _client.PutAsync("https://localhost:7252/api/FitnessPlan/10", JsonContent.Create(Changedplan));

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
