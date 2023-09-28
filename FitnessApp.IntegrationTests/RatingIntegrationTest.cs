using Fitness_Application_new.Controllers;
using Fitness_Application_new.DTOs;
using FitnessApp.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FitnessApp.IntegrationTests
{
    public class RatingIntegrationTest :IDisposable
    {
        private CustomWebApplicationFactory _factory;
        private HttpClient _client;

        public RatingIntegrationTest()
        {
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();

        }

        [Fact]
        public async Task GetAsync_ReturnsListOfRating()
        {
            //// Arrange
            var ratingList = new List<Rating>();
            ratingList.Add(new Rating { Id = 10 ,value=2
                });
            
            Task<IEnumerable<Rating>> mockRatings = Task.FromResult<IEnumerable<Rating>>(ratingList);

            _factory.RatingServiceMock
                .Setup(r => r.GetRatingsAsync())
                .Returns(mockRatings);

            //// Act
            var response = await _client.GetAsync("https://localhost:7252/api/Rating");

            //// Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var unpackData = JsonConvert.DeserializeObject<IEnumerable<RatingDto>>(await response.Content.ReadAsStringAsync());
            Assert.Collection(unpackData, r =>
            {
                Assert.Equal(10, r.Id);
                Assert.Equal(2, r.value);
            });
        }

        [Fact]
        public async Task Get_ReturnsRating()
        {
            // Arrange
            var rating = new Rating
            {
                Id = 10,
                value = 2
            };

            Task<Rating> mockRatings = Task.FromResult<Rating>(rating);

            _factory.RatingServiceMock
                .Setup(r => r.GetRatingAsync(10))
                .Returns(mockRatings);

            //// Act
            var response = await _client.GetAsync("https://localhost:7252/api/Rating/10");
            
            //// Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var unpackData = JsonConvert.DeserializeObject<RatingDto>(await response.Content.ReadAsStringAsync());
            Assert.Equal(2, unpackData!.value);
            Assert.Equal(10,unpackData!.Id);
        }

        [Fact]
        public async Task Post_ReturnsOkWithCreatedId()
        {
            //// Arrange
            var rating = new Rating
            {
                Id = 10,
                value=3
            };
            Task<Rating> mockRating = Task.FromResult<Rating>(rating);
            var claims = new Dictionary<string, object>
            {
                { ClaimTypes.Name, "test@test.com" },
                { ClaimTypes.Role, "user" },
                { ClaimTypes.NameIdentifier, "testId" }
            };
            _client.SetFakeBearerToken(claims);


            _factory.RatingServiceMock
                .Setup(r => r.InsertRatingAsync(It.Is<Rating>(rating => rating.Id == 10)))
                .Returns(mockRating)
                ;

            //// Act
            var response = await _client.PostAsync("https://localhost:7252/api/Rating", JsonContent.Create(rating));

            //// Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent()
        {
            // Arrange
            var itemId = 10;

            _factory.RatingServiceMock.Setup(service => service.DeleteRatingAsync(itemId)).Returns(Task.CompletedTask);

            // Act
            var response = await _client.DeleteAsync("https://localhost:7252/api/Rating/10");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task Put_ReturnsNoContentResult()
        {
            // Arrange
            var Originalrating = new Rating
            {
                Id = 10,
                FitnessPlanId = 10,
                value=1
            };

            var Changedrating = new Rating
            {
                Id = 10,
                FitnessPlanId = 15,
                value=2,
                
            };
            var claims = new Dictionary<string, object>
            {
                { ClaimTypes.Name, "test@test.com" },
                { ClaimTypes.Role, "user" },
                { ClaimTypes.NameIdentifier, "testId" }
            };
            _client.SetFakeBearerToken(claims);

            // Act
            var response = await _client.PutAsync("https://localhost:7252/api/Rating/10", JsonContent.Create(Changedrating));

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
