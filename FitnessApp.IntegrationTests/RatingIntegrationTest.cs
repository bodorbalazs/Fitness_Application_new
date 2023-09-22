using Fitness_Application_new.Controllers;
using Fitness_Application_new.DTOs;
using FitnessApp.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
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
        public async Task GetAsync_ReturnsListOfRatingDto()
        {
            //// Arrange
            var ratingList = new List<Rating>();
            ratingList.Add(new Rating { Id = 10 ,value=2
                });
            
            Task<IEnumerable<Rating>> mockRatings = Task.FromResult<IEnumerable<Rating>>(ratingList);

            var mockRatingDtos = new List<RatingDto>();
            mockRatingDtos.Add(new RatingDto());

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

        public void Dispose()
        {
            _client.Dispose();
            _factory.Dispose();
        }
    }
}
