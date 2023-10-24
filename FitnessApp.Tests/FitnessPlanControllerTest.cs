using AutoMapper;
using Fitness_Application_new.Controllers;
using Fitness_Application_new.DTOs;
using Fitness_Application_new.Interfaces;
using Fitness_Application_new.Services;
using FitnessApp.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Tests
{
    public class FitnessPlanControllerTest
    {
        private readonly FitnessPlanController fitnessPlanController;
        private readonly Mock<IFitnessPlanService> _fitnessPlanServiceMock = new Mock<IFitnessPlanService>();
        private readonly Mock<IMapper> mockMapper = new Mock<IMapper>();
        private readonly Mock<IFavouriteItemService> FavoriteItemServiceMock = new Mock<IFavouriteItemService>();
        private readonly Mock<IRatingService> ratingServiceMock = new Mock<IRatingService>();
        private readonly Mock<IFitnessExerciseService> fitnessExerciseServiceMock = new Mock<IFitnessExerciseService>();
        public FitnessPlanControllerTest()
        {
            fitnessPlanController = new FitnessPlanController(
                null,
                _fitnessPlanServiceMock.Object,
                mockMapper.Object,
                ratingServiceMock.Object,
                FavoriteItemServiceMock.Object,
                fitnessExerciseServiceMock.Object
                );
        }
        [Fact]
        public async Task GetAsync_ReturnsListOfFitnessPlanDto()
        {
            // Arrange
            var mockFitnessPlans = new List<FitnessPlan>();
            var mockFitnessPlanDtos = new List<FitnessPlanDto>();

            _fitnessPlanServiceMock.Setup(service => service.GetFitnessPlansAsync()).ReturnsAsync(mockFitnessPlans);
            mockMapper.Setup(mapper => mapper.Map<List<FitnessPlanDto>>(mockFitnessPlans)).Returns(mockFitnessPlanDtos);

            // Act
            var result = await fitnessPlanController.GetAsync();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<FitnessPlanDto>>>(result);
            var model = Assert.IsType<List<FitnessPlanDto>>(actionResult.Value);
            Assert.Equal(mockFitnessPlanDtos, model);
            _fitnessPlanServiceMock.Verify(service => service.GetFitnessPlansAsync(), Times.Once);
        }

        [Fact]
        public async Task Get_ReturnsFitnessPlanDto()
        {
            // Arrange
            var planId = 10;
            var mockFitnessPlan = new FitnessPlan();
            var mockFitnessPlanDto = new FitnessPlanDto();

            _fitnessPlanServiceMock.Setup(service => service.GetFitnessPlanAsync(planId)).ReturnsAsync(mockFitnessPlan);
            mockMapper.Setup(mapper => mapper.Map<FitnessPlanDto>(mockFitnessPlan)).Returns(mockFitnessPlanDto);

            // Act
            var result = await fitnessPlanController.Get(planId);

            // Assert
            var fitnessPlanDto = Assert.IsType<FitnessPlanDto>(result);
            Assert.Equal(mockFitnessPlanDto, fitnessPlanDto);
            _fitnessPlanServiceMock.Verify(service => service.GetFitnessPlanAsync(planId), Times.Once);
        }
        [Fact]
        public async Task Delete_ReturnsNoContentResult()
        {
            // Arrange
            var planId = 10;

            // Act
            var result = await fitnessPlanController.Delete(planId);

            // Assert
            var actionResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(StatusCodes.Status204NoContent, actionResult.StatusCode);
            _fitnessPlanServiceMock.Verify(service => service.DeleteFitnessPlanAsync(planId), Times.Once);
        }
    }
}
