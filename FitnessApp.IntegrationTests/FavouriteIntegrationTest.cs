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
    public class FavouriteIntegrationTest : IDisposable
    {
        private CustomWebApplicationFactory _factory;
        private HttpClient _client;

        public FavouriteIntegrationTest()
        {
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();

        }

        [Fact]
        public async Task GetAsync_ReturnsListOfFavouriteItemDto()
        {
            //// Arrange
            var favouriteList = new List<FavouriteItem>();
            favouriteList.Add(new FavouriteItem
            {
                Id = 10
            });
            var claims = new Dictionary<string, object>
            {
                { ClaimTypes.Name, "test@test.com" },
                { ClaimTypes.Role, "user" }
            };
            _client.SetFakeBearerToken(claims);
            Task<IEnumerable<FavouriteItem>> mockFavouriteItems = Task.FromResult<IEnumerable<FavouriteItem>>(favouriteList);

            _factory.FavouriteItemServiceMock
                .Setup(r => r.GetFavouriteItemsAsync())
                .Returns(mockFavouriteItems);

            //// Act
            var response = await _client.GetAsync("https://localhost:7252/api/FavouriteItem");

            //// Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var unpackData = JsonConvert.DeserializeObject<IEnumerable<FavouriteItemDto>>(await response.Content.ReadAsStringAsync());
            Assert.Collection(unpackData, r =>
            {
                Assert.Equal(10, r.Id);
            });
        }

        [Fact]
        public async Task Get_ReturnsFavourite()
        {
            // Arrange
            var favourite = new FavouriteItem
            {
                Id = 10
            };

            var claims = new Dictionary<string, object>
            {
                { ClaimTypes.Name, "test@test.com" },
                { ClaimTypes.Role, "user" }
            };
            _client.SetFakeBearerToken(claims);
            Task<FavouriteItem> mockFavouriteItems = Task.FromResult<FavouriteItem>(favourite);

            _factory.FavouriteItemServiceMock
                .Setup(r => r.GetFavouriteItemAsync(10))
                .Returns(mockFavouriteItems);

            //// Act
            var response = await _client.GetAsync("https://localhost:7252/api/FavouriteItem/10");

            //// Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var unpackData = JsonConvert.DeserializeObject<FavouriteItemDto>(await response.Content.ReadAsStringAsync());
            Assert.Equal(10, unpackData!.Id);
        }

        [Fact]
        public async Task Get_ReturnsUsersFavourites()
        {
            //// Arrange
            var favouriteList = new List<FavouriteItem>();
            favouriteList.Add(new FavouriteItem
            {
                Id = 10,
                ApplicationUserId="testId"
                
            });
            favouriteList.Add(new FavouriteItem
            {
                Id = 11,
                ApplicationUserId = "testId"
            });
            var claims = new Dictionary<string, object>
            {
                { ClaimTypes.Name, "test@test.com" },
                { ClaimTypes.Role, "user" },
                { ClaimTypes.NameIdentifier, "testId" }
            };
            _client.SetFakeBearerToken(claims);
            Task<IEnumerable<FavouriteItem>> mockFavouriteItems = Task.FromResult<IEnumerable<FavouriteItem>>(favouriteList);

            _factory.FavouriteItemServiceMock
                .Setup(r => r.GetUsersFavouriteItemsAsync("testId"))
                .Returns(mockFavouriteItems);

            //// Act
            var response = await _client.GetAsync("https://localhost:7252/api/FavouriteItem/GetUsersFavourites");

            //// Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var unpackData = JsonConvert.DeserializeObject<IEnumerable<FavouriteItemDto>>(await response.Content.ReadAsStringAsync());
            Assert.Collection(unpackData,
                r =>
            {
                Assert.Equal(10, r.Id);
                Assert.Equal("testId", r.ApplicationUserId);
            },
            r =>
            {
                Assert.Equal(11, r.Id);
                Assert.Equal("testId", r.ApplicationUserId);
            }
            );
        }

        [Fact]
        public async Task Post_ReturnsOkWithCreatedId()
        {
            //// Arrange
            var favourite = new FavouriteItem
            {
                Id = 10,
                FitnessPlanId = 2
                
            };
            Task<FavouriteItem> mockFavouriteItem = Task.FromResult<FavouriteItem>(favourite);

            var claims = new Dictionary<string, object>
            {
                { ClaimTypes.Name, "test@test.com" },
                { ClaimTypes.Role, "user" },
                { ClaimTypes.NameIdentifier, "testId" }
            };
            _client.SetFakeBearerToken(claims);


            _factory.FavouriteItemServiceMock
                .Setup(r => r.InsertFavouriteItemAsync(It.Is<FavouriteItem>(f=>f.Id==10)))
                .Returns(mockFavouriteItem)
                ;

            //// Act
            var response = await _client.PostAsync("https://localhost:7252/api/FavouriteItem", JsonContent.Create(favourite));

            //// Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent()
        {
            // Arrange
            var itemId = 10;
            var claims = new Dictionary<string, object>
            {
                { ClaimTypes.Name, "test@test.com" },
                { ClaimTypes.Role, "user" },
                { ClaimTypes.NameIdentifier, "testId" }
            };
            _client.SetFakeBearerToken(claims);

            _factory.FavouriteItemServiceMock.Setup(service => service.DeleteFavouriteItemAsync(itemId)).Returns(Task.CompletedTask);

            // Act
            var response = await _client.DeleteAsync("https://localhost:7252/api/FavouriteItem/10");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task Put_ReturnsNoContentResult()
        {
            // Arrange
            var Originalfavourite = new FavouriteItem
            {
                Id = 10,
                FitnessPlanId = 10
            };

            var Changedfavourite = new FavouriteItem
            {
                Id = 10,
                FitnessPlanId = 15
            };
            var claims = new Dictionary<string, object>
            {
                { ClaimTypes.Name, "test@test.com" },
                { ClaimTypes.Role, "user" },
                { ClaimTypes.NameIdentifier, "testId" }
            };
            _client.SetFakeBearerToken(claims);

            // Act
            var response = await _client.PutAsync("https://localhost:7252/api/FavouriteItem/10", JsonContent.Create(Changedfavourite));

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
