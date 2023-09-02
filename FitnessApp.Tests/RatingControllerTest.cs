using AutoMapper;
using Fitness_Application_new.Controllers;
using Fitness_Application_new.DTOs;
using Fitness_Application_new.Interfaces;
using FitnessApp.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Tests
{
    public class RatingControllerTest
    {
        private readonly RatingController ratingController;
        private readonly Mock<IRatingService> _ratingServiceMock = new Mock<IRatingService>();
        private readonly Mock<IMapper> mockMapper = new Mock<IMapper>();

        public RatingControllerTest()
        {
            ratingController = new RatingController(null, _ratingServiceMock.Object, mockMapper.Object);
        }
        [Fact]
        public async Task GetAsync_ReturnsListOfRatingDto()
        {
            // Arrange
            var mockRatings = new List<Rating>();
            mockRatings.Add(new Rating());
            var mockRatingDtos = new List<RatingDto>();
            mockRatingDtos.Add(new RatingDto());

            _ratingServiceMock.Setup(service => service.GetRatingsAsync()).ReturnsAsync(mockRatings);
            mockMapper.Setup(mapper => mapper.Map<List<RatingDto>>(mockRatings)).Returns(mockRatingDtos);

            // Act
            var result = await ratingController.GetAsync();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<RatingDto>>>(result);
            var model = Assert.IsType<List<RatingDto>>(actionResult.Value);
            Assert.Equal(mockRatingDtos.Count, model.Count);
            _ratingServiceMock.Verify(service => service.GetRatingsAsync(), Times.Once);
        }

        [Fact]
        public async Task Get_ReturnsRatingDto()
        {
            // Arrange
            var ratingId = 10;
            var mockRating = new Rating();
            var mockRatingDto = new RatingDto();

            _ratingServiceMock.Setup(service => service.GetRatingAsync(ratingId)).ReturnsAsync(mockRating);
            mockMapper.Setup(mapper => mapper.Map<RatingDto>(mockRating)).Returns(mockRatingDto);

            // Act
            var result = await ratingController.Get(ratingId);

            // Assert
            var ratingDto = Assert.IsType<RatingDto>(result);
            Assert.Equal(mockRatingDto, ratingDto);
            _ratingServiceMock.Verify(service => service.GetRatingAsync(ratingId), Times.Once);
        }
        [Fact]
        public async Task Delete_ReturnsNoContentResult()
        {
            // Arrange
            var ratingId = 10;

            // Act
            var result = await ratingController.Delete(ratingId);

            // Assert
            var actionResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(StatusCodes.Status204NoContent, actionResult.StatusCode);
            _ratingServiceMock.Verify(service => service.DeleteRatingAsync(ratingId), Times.Once);
        }

        [Fact]
        public async Task DeletePlansRating_ReturnsNoContentResult()
        {
            // Arrange
            var planId = 10;

            // Act
            var result = await ratingController.DeletePlansRating(planId);

            // Assert
            var actionResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(StatusCodes.Status204NoContent, actionResult.StatusCode);
            _ratingServiceMock.Verify(service => service.DeletePlansRatingAsync(planId), Times.Once);
        }

    }


}
