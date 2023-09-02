using AutoMapper;
using Fitness_Application_new.Controllers;
using Fitness_Application_new.DTOs;
using Fitness_Application_new.Interfaces;
using FitnessApp.API;
using FitnessApp.DAL.Data;
using FitnessApp.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace FitnessApp.Tests
{
    public class FavouriteItemTest
    {
        private readonly FavouriteItemController favouriteItemController;
        private readonly Mock<IFavouriteItemService> _favouriteItemServiceMock = new Mock<IFavouriteItemService>();
        private readonly Mock<IMapper> mockMapper = new Mock<IMapper>();
        public FavouriteItemTest()
        {
            favouriteItemController = new FavouriteItemController(null, _favouriteItemServiceMock.Object, mockMapper.Object);
        }

        [Fact]
        public async Task Get_ReturnsFavouriteItemDto()
        {
            // Arrange
            var itemId = 10;
            var mockFavouriteItem = new FavouriteItem();
            var mockFavouriteItemDto = new FavouriteItemDto();

            _favouriteItemServiceMock.Setup(service => service.GetFavouriteItemAsync(itemId)).ReturnsAsync(mockFavouriteItem);
            mockMapper.Setup(mapper => mapper.Map<FavouriteItemDto>(mockFavouriteItem)).Returns(mockFavouriteItemDto);

            // Act
            var result = await favouriteItemController.Get(itemId);

            // Assert
            var favouriteItemDto = Assert.IsType<FavouriteItemDto>(result);
            Assert.Equal(mockFavouriteItemDto, favouriteItemDto);
            _favouriteItemServiceMock.Verify(service => service.GetFavouriteItemAsync(itemId), Times.Once);
        }

        [Fact]
        public async Task GetAsync_ReturnsListOfFavouriteItemDto()
        {
            // Arrange
            var mockFavouriteItems = new List<FavouriteItem>();
            var mockFavouriteItemDtos = new List<FavouriteItemDto>();

            _favouriteItemServiceMock.Setup(service => service.GetFavouriteItemsAsync()).ReturnsAsync(mockFavouriteItems);
            mockMapper.Setup(mapper => mapper.Map<List<FavouriteItemDto>>(mockFavouriteItems)).Returns(mockFavouriteItemDtos);

            // Act
            var result = await favouriteItemController.GetAsync();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<FavouriteItemDto>>>(result);
            var model = Assert.IsType<List<FavouriteItemDto>>(actionResult.Value);
            Assert.Equal(mockFavouriteItemDtos, model);
            _favouriteItemServiceMock.Verify(service => service.GetFavouriteItemsAsync(), Times.Once);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent()
        {
            // Arrange
            var itemId = 10;

            _favouriteItemServiceMock.Setup(service => service.DeleteFavouriteItemAsync(itemId)).Returns(Task.CompletedTask);

            // Act
            var result = await favouriteItemController.Delete(itemId);

            // Assert
            var actionResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, actionResult.StatusCode);
            _favouriteItemServiceMock.Verify(service => service.DeleteFavouriteItemAsync(itemId), Times.Once);
        }
    }
}